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
		protected FFRoom _currentRoom;
		internal FFRoom CurrentRoom
		{
			set
			{
				_currentRoom = value;
			}
			get
			{
				return _currentRoom;
			}
		}
		#endregion
	
		internal NetworkGameManager()
		{
		
		}
	
		#region Game Mode Management
		internal FFRoom PrepareRoom()
		{
			_currentRoom = new FFRoom();
			_currentRoom.roomName = "Partie de " + ((NetworkMenuGameMode)CurrentGameMode()).playerName;
			_currentRoom.AddTeam(1,"Left side");
			_currentRoom.AddTeam(1,"Right side");
			return _currentRoom;
		}
		#endregion
	}
}