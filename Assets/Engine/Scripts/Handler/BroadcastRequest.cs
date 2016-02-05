using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;
using FF.Network.Message;

namespace FF.Handler
{
    internal abstract class BroadcastRequestHandler : ABaseHandler
    {
        /*internal delegate void BroadcastClientCallback(FFTcpClient a_client,
                                                       SentRequest a_request);
        internal delegate void BroadcastCallback(List<FFTcpClient> _successClients,
                                                 List<FFTcpClient> a_failedClients,
                                                 SentRequest a_request);

        #region Properties
        internal BroadcastClientCallback onSuccessForClient = null;
        internal BroadcastClientCallback onFailForClient = null;

        internal BroadcastCallback onFail = null;
        internal BroadcastCallback onSuccess = null;

        protected List<FFTcpClient> _targets;
        protected List<FFTcpClient> _failedClients;
        protected List<FFTcpClient> _successClients;

        protected SentRequest _request;
        #endregion

        #region Constructor
        protected virtual List<FFTcpClient> TargetClients
        {
            get
            {
                return Engine.Network.Server.GetPlayersClients();
            }
        }

        protected BroadcastRequestHandler(SentRequest a_request)
        {
            _request = a_request;
            _targets = TargetClients;
            _failedClients = new List<FFTcpClient>();
            _successClients = new List<FFTcpClient>();
        }
        #endregion

        #region Request callbacks per client
        protected virtual void OnSuccessForClient(FFTcpClient a_client)
        {
            _successClients.Add(a_client);

            if (onSuccessForClient != null)
                onSuccessForClient(a_client, _requestData);

            OnResponseReceived();
        }

        protected virtual void OnFailForClient(FFTcpClient a_client)
        {
            _failedClients.Add(a_client);

            if (onFailForClient != null)
                onFailForClient(a_client, _request);

            OnResponseReceived();
        }

        protected virtual void OnTimeoutForClient(FFTcpClient a_client)
        {
            OnFailForClient(a_client);
        }

        protected virtual void OnCancelForClient(FFTcpClient a_client)
        {
            OnFailForClient(a_client);
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

            if (onSuccess != null)
                onSuccess(_successClients, _failedClients);
        }

        protected virtual void OnFail()
        {
            _isComplete = true;

            if (onFail != null)
                onFail(_successClients, _failedClients);
        }

        internal override void Complete()
        {
            _targets.Clear();
            _failedClients.Clear();
            _successClients.Clear();

            onSuccessForClient = null;
            onFailForClient = null;

            onFail = null;
            onSuccess = null;

            base.Complete();
        }
        #endregion*/
    }
}