using UnityEngine;
using System.Collections;
using Zeroconf;
using FFNetworking;
using System.Net;

internal class DebugTest : MonoBehaviour
{
	public ZeroconfManager manager = null;
	
	public FFTcpServer _server;
	
	private float _timeElapsed;
	// Use this for initialization
	void Start ()
	{
		/*manager.Client.DebugMode = true;
		manager.Client.CanFindSelf = true;
		manager.Host.DebugMode = true;*/
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
	
	void OnDestroy()
	{
		if(_server != null)
			_server.Close();
	}
	
	
	#region Buttons callback
	bool _isDiscovering = false;
	void StartDiscover()
	{
		_isDiscovering = true;
		manager.Client.StartDiscovery("_http._tcp.");
		manager.Client.onRoomAdded += OnRoomAdded;
		
		if(_server != null)
		{
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.15"),_server.Port);
			if(ep != null)
			{
				FFTcpClient client = new FFTcpClient(ep);
				if(client.Connect())
				{
					client.StartWorkers();
				}
			}
		}
	}
	
	void StopDiscover()
	{
		_isDiscovering = false;
		manager.Client.StopDiscovery();
		manager.Client.onRoomAdded -= OnRoomAdded;
	}
	
	void OnRoomAdded(ZeroconfRoom a_room)
	{
		FFLog.LogError("On Room Found : " + a_room.EndPoint.ToString());		
		/*IPEndPoint ep = a_room.EndPoint;
		if(ep != null)
		{
			FFTcpClient client = new FFTcpClient(ep);
			if(client.Connect())
			{
				client.StartWorkers();
			}
		}*/
	}
	
	bool _isAdvertising = false;
	void StartAdvertising()
	{
		_isAdvertising = true;
		_server = new FFTcpServer();
		_server.StartAcceptingConnections();
		manager.Host.StartAdvertising("_http._tcp.","My zeroconf room", _server.Port);
	}
	
	void StopAdvertising()
	{
		_isAdvertising = false;
		manager.Host.StopAdvertising();
	}
	#endregion
	
	#region Buttons Call
	public void OnDiscoverButtonPressed()
	{
		if(_isDiscovering)
			StopDiscover();
		else
			StartDiscover();
	}
	
	public void OnAdvertisingButtonPressed()
	{
		if(_isAdvertising)
			StopAdvertising();
		else
			StartAdvertising();
	}
	#endregion
}
