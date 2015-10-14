using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal abstract class FFRequestMessage : FFMessage
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

        ~FFRequestMessage()
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

        internal override bool PostWrite()
        {
            _client.onConnectionLost += OnConnectionLost;
            return base.PostWrite();
        }

        protected virtual void OnConnectionLost(FFTcpClient a_client)
        {
            int errorCode = DisconnectErrorCode;
            FFResponseMessage answer = new FFRequestFail(errorCode);
            answer.requestId = requestId;
            _client.QueueReadMessage(answer);
        }

        internal virtual void Unregister()
        {
            if(_client != null)
                _client.onConnectionLost -= OnConnectionLost;
        }

        protected abstract int DisconnectErrorCode
        {
            get;
        }

        internal void Cancel(bool a_isSender)
        {
            if(a_isSender)
                _client.CancelSentRequest(requestId);
            else
                _client.CancelReadRequest(requestId);
            if (onCancel != null)
                onCancel();
        }

        internal void ForceFail()
        {
            // Queueing fail message.
            OnConnectionLost(_client);
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
    }
}