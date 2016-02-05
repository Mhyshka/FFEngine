using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Network.Message
{
    internal enum ERequestErrorCode
    {
        Success,
        Failed,

        Unknown,

        LocalCanceled,
        RemoteCanceled,
        
        IllegalArgument,
        IllegalState,
        Forbidden,
        Timeout,

        LocalConnectionIssue,
        RemoteConnectionIssue
    }

    internal class ResponseFail : MessageData
	{
		#region Properties
		internal ERequestErrorCode errorCode = ERequestErrorCode.Unknown;
        internal int detailErrorCode = -1;
        internal override EDataType Type
        {
            get
            {
                return EDataType.R_Fail;
            }
        }
        #endregion

        internal ResponseFail(ERequestErrorCode a_errorCode, int a_detailErrorCode = -1) : base()
		{
			errorCode = a_errorCode;
            detailErrorCode = a_detailErrorCode;
        }

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
		{
			base.SerializeData(stream);
			stream.Write((int)errorCode);
            stream.Write(detailErrorCode);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData(stream);
			errorCode = (ERequestErrorCode)stream.TryReadInt();
            detailErrorCode = stream.TryReadInt();
        }
        #endregion
    }
}