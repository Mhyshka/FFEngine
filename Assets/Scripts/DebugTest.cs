using UnityEngine;
using System.Collections;
using Zeroconf;

public class DebugTest : MonoBehaviour
{
	public ZeroconfManager manager = null;
	
	private float _timeElapsed;
	// Use this for initialization
	void Start ()
	{
		manager.Client.SetDebugMode(true);
		manager.Client.CanFindSelf = true;
		manager.Client.StartDiscovery("_tcp.");
		
		manager.Host.SetDebugMode(true);
		manager.Host.StartAdvertising("_tcp.","My zeroconf room");
	}
	
	void Update()
	{
		_timeElapsed += Time.deltaTime;
		if(_timeElapsed > 10f)
		{
			/*if(manager.Client.State != EZeroconfClientState.Idle)
				manager.Client.StopDiscovery();*/
				
			if(manager.Host.State != EZeroconfHostState.Idle)
				manager.Host.StopAdvertising();
		}
	}
}
