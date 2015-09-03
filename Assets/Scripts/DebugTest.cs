using UnityEngine;
using System.Collections;
using Zeroconf;
using FFNetworking;

internal class DebugTest : MonoBehaviour
{
	public ZeroconfManager manager = null;
	
	public FFTcpServer _server;
	
	private float _timeElapsed;
	// Use this for initialization
	void Start ()
	{
		manager.Client.DebugMode = true;
		manager.Client.CanFindSelf = true;
		manager.Client.StartDiscovery("_http._tcp.");
		
		//manager.Host.SetDebugMode(true);
		
		_server = new FFTcpServer();
		
		manager.Host.StartAdvertising("_http._tcp.","My zeroconf room", _server.Port);
		
		
		
		manager.Client.onRoomAdded += OnRoomAdded;
	}
	
	void Update()
	{
		_timeElapsed += Time.deltaTime;
		if(_timeElapsed > 10f)
		{
			//_server.StopAcceptingConnections();
			/*if(manager.Client.State != EZeroconfClientState.Idle)
				manager.Client.StopDiscovery();*/
				
			/*if(manager.Host.State != EZeroconfHostState.Idle)
				manager.Host.StopAdvertising();*/
		}
	}
	
	void OnRoomAdded(ZeroconfRoom a_room)
	{
		FFLog.LogError("On Room Found : " + a_room.EndPoint.ToString());
		FFTcpClient client = new FFTcpClient(a_room);
	}
}
