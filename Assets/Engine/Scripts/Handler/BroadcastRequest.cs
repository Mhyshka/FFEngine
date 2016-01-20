using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal abstract class BroadcastRequest : AHandler
    {
        #region Properties
        protected List<FFTcpClient> _targets;
        protected List<FFTcpClient> _failedClients;
        protected List<FFTcpClient> _successClients;

        protected FFClientCallback _onSuccessForClient = null;
        protected FFClientCallback _onFailForClient = null;

        protected FFClientsBroadcastCallback _onFail = null;
        protected FFClientsBroadcastCallback _onSuccess = null;
        #endregion

        #region Constructor
        protected virtual List<FFTcpClient> TargetClients
        {
            get
            {
                return Engine.Network.Server.GetPlayersClients();
            }
        }

        protected BroadcastRequest(FFClientsBroadcastCallback a_onSuccess, FFClientsBroadcastCallback a_onFail, FFClientCallback a_onSuccessForClient, FFClientCallback a_onFailForClient )
        {
            _targets = TargetClients;
            _failedClients = new List<FFTcpClient>();
            _successClients = new List<FFTcpClient>();

            _onSuccess = a_onSuccess;
            _onFail = a_onFail;
            _onSuccessForClient = a_onSuccessForClient;
            _onFailForClient = a_onFailForClient;

            foreach (FFTcpClient each in _targets)
            {
                if (each == null)
                {
                    OnFailForClient(each, (int)ESimpleRequestErrorCode.Disconnected);
                }
                else
                {
                    SendRequestToClient(each);
                }
            }
        }
        #endregion

        protected abstract void SendRequestToClient(FFTcpClient a_client);

        #region Request callbacks per client
        protected virtual void OnSuccessForClient(FFTcpClient a_client)
        {
            _successClients.Add(a_client);

            if (_onSuccessForClient != null)
                _onSuccessForClient(a_client);

            OnResponseReceived();
        }

        protected virtual void OnFailForClient(FFTcpClient a_client, int a_errorCode)
        {
            _failedClients.Add(a_client);

            if (_onFailForClient != null)
                _onFailForClient(a_client);

            OnResponseReceived();
        }

        protected virtual void OnTimeoutForClient(FFTcpClient a_client)
        {
            OnFailForClient(a_client, (int)ESimpleRequestErrorCode.Timedout);
        }

        protected virtual void OnCancelForClient(FFTcpClient a_client)
        {
            OnFailForClient(a_client, (int)ESimpleRequestErrorCode.Canceled);
        }

        protected virtual void OnResponseReceived()
        {
            if (_successClients.Count >= _targets.Count)
                OnSuccess();
            else if (_successClients.Count + _failedClients.Count >= _targets.Count)
                OnFail();
        }
        #endregion

        #region Completion
        protected virtual void OnSuccess()
        {
            _isComplete = true;

            if (_onSuccess != null)
                _onSuccess(_successClients, _failedClients);
        }

        protected virtual void OnFail()
        {
            _isComplete = true;

            if (_onFail != null)
                _onFail(_successClients, _failedClients);
        }

        internal override void OnComplete()
        {
            _targets.Clear();
            _failedClients.Clear();
            _successClients.Clear();

            _onSuccessForClient = null;
            _onFailForClient = null;

            _onFail = null;
            _onSuccess = null;

            base.OnComplete();
        }
        #endregion
    }
}