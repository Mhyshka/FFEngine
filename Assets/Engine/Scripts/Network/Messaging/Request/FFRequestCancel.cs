using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
    internal class FFRequestCancel : FFResponseMessage
    {
        #region properties
        internal override EMessageType Type
        {
            get
            {
                return EMessageType.RequestCancel;
            }
        }

        protected FFRequestMessage _request;

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        #region Constructors
        public FFRequestCancel()
        {
        }

        internal FFRequestCancel(FFRequestMessage a_request)
        {
            _request = a_request;
            requestId = _request.requestId;
        }
        #endregion

        internal override void Read(FFRequestMessage a_request)
        {
            FFLog.LogError("Request Cancel Read.");
            a_request.Cancel(false);
        }

        internal override bool PostWrite()
        {
            FFLog.LogError("Request Cancel Write.");
            _request.Cancel(true);
            return base.PostWrite();
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