using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
    internal class ResponseCancel : AResponse
    {
        #region properties
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.ResponseCancel;
            }
        }

        protected ARequest _request;

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Constructors
        public ResponseCancel()
        {
        }

        internal ResponseCancel(ARequest a_request)
        {
            _request = a_request;
            requestId = _request.requestId;
        }
        #endregion

        internal override void PostWrite()
        {
            FFLog.LogError("Request Cancel Write.");
            _request.Cancel(true);
            base.PostWrite();
        }

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
        {
            base.SerializeData(stream);
        }

        public override void LoadFromData(FFByteReader stream)
        {
            base.LoadFromData(stream);
        }
        #endregion
    }
}