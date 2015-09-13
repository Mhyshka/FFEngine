using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	internal class FFPlayer : FFPeer
	{
		#region properties
		public string username;
		#endregion

		#region public methods
		internal FFPlayer (IPEndPoint anIpEndpoint, string aUsername)
		{
			this.ipEndPoint = anIpEndpoint;
			this.username = aUsername;
		}
		#endregion
	}
}
