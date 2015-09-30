using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	internal class FFJoinRoomFail : FFResponseMessage
	{
		#region Properties
		public string errorMessage = null;
		#endregion
		
		public FFJoinRoomFail()
		{
		
		}
		
		internal FFJoinRoomFail(string a_errorMessage)
		{
			errorMessage = a_errorMessage;
		}
		
		#region Message
		internal override EMessageType Type
		{
			get
			{
				return EMessageType.JoinRoomFail;
			}
		}
		
		internal override void Read(FFTcpClient a_tcpClient, FFRequestMessage a_request)
		{
			FFJoinRoomRequest req = a_request as FFJoinRoomRequest;
			if(req.onDeny != null)
				req.onDeny(errorMessage);
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