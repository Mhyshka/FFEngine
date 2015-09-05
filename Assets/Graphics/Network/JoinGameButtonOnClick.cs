using UnityEngine;
using System.Collections;
using Zeroconf;


namespace Zeroconf
{
	internal class JoinGameButtonOnClick  : MonoBehaviour
	{
		public ZeroconfManager manager = null;

		public void onJoinGameButtonClicked ()
		{
			Debug.Log ("onJoinGameButtonClicked");
			
			Debug.Log (manager);
			Debug.Log (manager.Client);

			manager.Client.StartDiscovery("_http._tcp.");
		}
	}
}
