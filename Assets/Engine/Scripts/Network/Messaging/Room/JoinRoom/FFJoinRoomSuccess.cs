using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	internal class FFJoinRoomSuccess : FFResponseMessage
	{
		#region Properties
		#endregion
		
		public FFJoinRoomSuccess()
		{
		
		}
		
		#region Message
		internal override void Read(FFTcpClient a_tcpClient, FFRequestMessage a_request)
		{
			FFJoinRoomRequest req = a_request as FFJoinRoomRequest;
			if(req.onSuccess != null)
				req.onSuccess();
		}
		
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.JoinRoomSuccess;
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