using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal class FFMessageFarewell : FFMessage
	{
		#region Properties
		public string reason = null;
		
	 	internal override EMessageType Type
	 	{
			get
			{
				return EMessageType.Farewell;
			}
		}
		#endregion
		
		public FFMessageFarewell()
		{
		}
		
		internal FFMessageFarewell(string a_reason)
		{
		
		}
		
		internal override void Read (FFTcpClient a_tcpClient)
		{
            a_tcpClient.EndConnection(reason);
		}
		
		#region Serialization
		public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(reason);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			reason = stream.TryReadString();
		}
		#endregion
	}
}