using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	internal class FFPeer : IByteStreamSerialized
	{
		#region properties
		public IPEndPoint ipEndPoint;
		#endregion
		
		internal FFPeer()
		{
		
		}
		
		internal FFPeer(IPEndPoint a_ep)
		{
			ipEndPoint = a_ep;
		}
		
		#region Serialization
		public virtual void SerializeData(FFByteWriter stream)
		{
		}
		
		public virtual void LoadFromData(FFByteReader stream)
		{
		}
		#endregion
	}
}
