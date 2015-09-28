using UnityEngine;
using System.Collections;
using System;
namespace FF.Networking
{
	internal abstract class FFResponseMessage : FFMessage
	{
		internal int requestId = -1;
		
		public override void SerializeData (FFByteWriter stream)
		{
			stream.Write(requestId);
		}
		
		public override void LoadFromData (FFByteReader stream)
		{
			requestId = stream.TryReadInt();
		}
		
		internal sealed override void Read (FFTcpClient a_tcpClient)
		{
			FFLog.LogError(EDbgCat.Networking, "Called read on a response message. You should call the Read that takes a request as parameter.");
			throw new NotImplementedException ();
		}
		
		internal abstract void Read(FFTcpClient a_tcpClient, FFRequestMessage a_request);
	}
}