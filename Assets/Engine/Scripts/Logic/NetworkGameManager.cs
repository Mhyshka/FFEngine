using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Networking;

namespace FF
{
	/// <summary>
	/// Basic Game Manager that handles loading of GameModes.
	/// </summary>
	internal class NetworkGameManager : GameManager
	{
		#region Properties
		//Placeholder
		internal FFPlayer player;
		#endregion
	
		internal NetworkGameManager()
		{
			player = new FFPlayer();
			player.username = SystemInfo.deviceName;
		}
	
		#region Game Mode Management
		#endregion
	}
}