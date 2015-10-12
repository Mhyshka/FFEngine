using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	internal class FFRequestFail : FFResponseMessage
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

        public FFRequestFail()
		{
		
		}
		
		internal FFRequestFail(int a_errorCode)
		{
			errorCode = a_errorCode;
		}

		#region Message
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.RequestFail;
			}
		}
		
		internal override void Read(FFRequestMessage a_request)
		{
			if(a_request.onFail != null)
                a_request.onFail(errorCode);
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