using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Message
{
	internal abstract class AResponse : AMessage
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
	}
}