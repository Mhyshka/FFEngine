using UnityEngine;
using System.Collections;

namespace FF
{
	internal class FFPlayer : IByteStreamSerialized
	{
		internal string username;
		internal int Rating
		{
			get
			{
				return Random.Range(0,6);
			}
		}
		
		public FFPlayer()
		{
			
		}
		
		#region Serialization
		public void SerializeData(FFByteWriter stream)
		{
			stream.Write(username);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
			username = stream.TryReadString();
		}
		#endregion
	}
}