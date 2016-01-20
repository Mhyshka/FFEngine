using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Network.Message
{
	internal class ResponseFail : AResponse
	{
		#region Properties
		public int errorCode = -1;

        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }

        #endregion

        public ResponseFail()
		{
		
		}
		
		internal ResponseFail(int a_errorCode)
		{
			errorCode = a_errorCode;
		}

		#region Message
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.ResponseFail;
			}
		}
        #endregion

        #region Serialization
        public override void SerializeData(FFByteWriter stream)
		{
			base.SerializeData(stream);
			stream.Write(errorCode);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData(stream);
			errorCode = stream.TryReadInt();
		}
		#endregion
		
	}
}