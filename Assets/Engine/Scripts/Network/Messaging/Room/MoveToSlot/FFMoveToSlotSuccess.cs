using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFMoveToSlotSuccess : FFResponseMessage
	{
		#region Properties
		#endregion
		
		public FFMoveToSlotSuccess()
		{
			
		}
		
		#region Message
		internal override void Read(FFTcpClient a_tcpClient, FFRequestMessage a_request)
		{
			FFMoveToSlotRequest req = a_request as FFMoveToSlotRequest;
			if(req.onSuccess != null)
				req.onSuccess();
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.MoveToSlotSuccess;
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