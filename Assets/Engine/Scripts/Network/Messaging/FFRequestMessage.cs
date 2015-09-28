using UnityEngine;
using System.Collections;

namespace FF.Networking
{
	internal abstract class FFRequestMessage : FFMessage
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