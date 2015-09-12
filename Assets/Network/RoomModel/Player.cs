using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	public class Player : Peer
	{
		#region properties
		public string username;
		#endregion


		#region public methods
		public Player (IPEndPoint anIpEndpoint, string aUsername)
		{
			this.ipEndPoint = anIpEndpoint;
			this.username = aUsername;
		}
		#endregion
	}
}
