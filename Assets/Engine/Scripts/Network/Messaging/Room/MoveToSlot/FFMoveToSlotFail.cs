using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFMoveToSlotFail : FFResponseMessage
	{
		#region Properties
		internal string errorMessage = null;
		#endregion
		
		public FFMoveToSlotFail()
		{
			
		}
		
		internal FFMoveToSlotFail(string a_errorMessage)
		{
			errorMessage = a_errorMessage;
		}
		
		#region Message
		internal override void Read(FFTcpClient a_tcpClient, FFRequestMessage a_request)
		{
			FFMoveToSlotRequest req = a_request as FFMoveToSlotRequest;
			if(req.onFail != null)
				req.onFail(errorMessage);
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.MoveToSlotFail;
			}
		}
		#endregion
		
		#region Serialization
		public override void SerializeData(FFByteWriter stream)
		{
			base.SerializeData(stream);
			stream.Write(errorMessage);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			base.LoadFromData(stream);
			errorMessage = stream.TryReadString();
		}
		#endregion
	}
}