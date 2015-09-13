using UnityEngine;
using System.Collections;
using Zeroconf;


namespace Zeroconf
{
	internal class HostGameButtonOnClick : MonoBehaviour
	{
		public ZeroconfManager manager = null;

		public void onHostGameButtonClicked ()
		{
			Debug.Log ("onHostGameButtonClicked");
			
			Debug.Log (manager);
			Debug.Log (manager.Host);
			manager.Host.StartAdvertising("_pong._tcp.local","My zeroconf room", 0);
		}
	}
}
