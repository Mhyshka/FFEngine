using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
    internal enum ESimpleRequestErrorCode
    {
        Forbidden = -4,
        Canceled = -3,
        Timedout = -2,
        Disconnected = -1
    }

	internal abstract class ARequest : AMessage
	{
        #region Properties
        internal int requestId = -1;

        internal IntCallback onFail = null;
        internal SimpleCallback onCancel = null;
        internal SimpleCallback onSuccess = null;
        internal SimpleCallback onTimeout = null;

        protected float _timeElapsed = 0f;
        protected virtual float TimeoutDuration
        {
            get
            {
                return 5f;
            }
        }
        #endregion

        ~ARequest()
        {
            FFLog.Log(EDbgCat.Message,"Request destroyed.");
        }

        internal bool CheckForTimeout()
        {
            _timeElapsed += Time.deltaTime;
            if(_timeElapsed > TimeoutDuration)
            {
                if (onTimeout != null)
                    onTimeout();

                return true;
            }

            return false;
        }

        internal override void PostWrite()
        {
            _client.onConnectionLost += OnConnectionLost;
            base.PostWrite();
        }

        protected virtual void OnConnectionLost(FFTcpClient a_client)
        {
            ForceFail();
        }

        internal virtual void Unregister()
        {
            if(_client != null)
                _client.onConnectionLost -= OnConnectionLost;
        }

        protected virtual int DisconnectErrorCode
        {
            get
            {
                return (int)ESimpleRequestErrorCode.Disconnected;
            }
        }

        internal void Cancel(bool a_isSender)
        {
            if(a_isSender)
                _client.RemoveSentRequest(requestId);
            else
                _client.RemoveReadRequest(requestId);
            if (onCancel != null)
                onCancel();
        }

        internal void ForceFail()
        {
            int errorCode = DisconnectErrorCode;
            AResponse answer = new ResponseFail(errorCode);
            answer.requestId = requestId;
            _client.QueueReadMessage(answer);
        }

        #region Serialization
        public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(requestId);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			requestId = stream.TryReadInt();
		}
        #endregion


        #region ErrorCode
        internal static string MessageForErrorCode(int a_errorCode)
        {
            string errorMessage = "";

            if (a_errorCode == (int)ESimpleRequestErrorCode.Canceled)
                errorMessage = "Forbidden";
            else if (a_errorCode == (int)ESimpleRequestErrorCode.Canceled)
                errorMessage = "Canceled";
            else if (a_errorCode == (int)ESimpleRequestErrorCode.Timedout)
                errorMessage = "Timedout";
            else if (a_errorCode == (int)ESimpleRequestErrorCode.Disconnected)
                errorMessage = "Connection issues";

            return errorMessage;
        }
        #endregion
    }
}