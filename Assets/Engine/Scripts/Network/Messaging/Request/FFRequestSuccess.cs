using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFRequestSuccess : FFResponseMessage
	{
        #region Properties
        internal override bool HandleByMock
        {
            get
            {
                return true;
            }
        }
        #endregion

        public FFRequestSuccess()
		{
			
		}
		
		#region Message
		internal override void Read(FFRequestMessage a_request)
		{
			if(a_request.onSuccess != null)
                a_request.onSuccess();
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.RequestSuccess;
			}
		}
		#endregion
		
		#region Serialization
		public override void SerializeData(FFByteWriter stream)
		{
			base.SerializeData(stream);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData(stream);
		}
		#endregion
	}
}