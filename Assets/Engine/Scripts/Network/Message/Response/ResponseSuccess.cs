using UnityEngine;
using System.Collections;

namespace FF.Network.Message
{
	internal class ResponseSuccess : AResponse
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

        public ResponseSuccess()
		{
			
		}
		
		#region Message
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.ResponseSuccess;
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