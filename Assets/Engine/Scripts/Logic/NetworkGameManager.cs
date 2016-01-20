using UnityEngine;
using System.Collections;

using FF.Multiplayer;

namespace FF.Logic
{
	/// <summary>
	/// Basic Game Manager that handles loading of GameModes.
	/// </summary>
	internal class NetworkGameManager : GameManager
	{
        #region Player Properties
        //Placeholder
        protected FFPlayer _player;
        internal FFPlayer Player
        {
            get
            {
                return _player;
            }
        }

        protected FFNetworkPlayer _netPlayer;
        internal FFNetworkPlayer NetPlayer
        {
            get
            {
                if (_netPlayer == null)// Not created by the server
                {
                    _netPlayer = new FFNetworkPlayer(Engine.Network.NetworkID, Player);
                    _netPlayer.useTV = Engine.MultiScreen.UseTV;
                }
                return _netPlayer;
            }
        }
        #endregion

        #region Room Properties
        protected Room _currentRoom;
        internal Room CurrentRoom
        {
            get
            {
                return _currentRoom;
            }
        }

        protected NetworkLoadingManager _loadingManager;
        internal NetworkLoadingManager Loading
        {
            get
            {
                return _loadingManager;
            }
        }
        #endregion

        #region Manager
        internal NetworkGameManager()
        {
            _player = new FFPlayer();
            _player.username = SystemInfo.deviceName;
            _loadingManager = new NetworkLoadingManager();
        }
        #endregion

        #region Game Mode Management

        #endregion

        #region Room Management
        internal Room PrepareRoom(string a_roomName)
        {
            _currentRoom = new Room();
            _currentRoom.roomName = a_roomName;

            _currentRoom.AddTeam(new Team("Left Side", 2, 1));
            _currentRoom.AddTeam(new Team("Right Side", 2, 1));

            Team spectators = new Team("Spectators", 2, 0);
            spectators.Slots[0].isPlayableSlot = false;
            spectators.Slots[1].isPlayableSlot = false;
            _currentRoom.AddTeam(spectators);

            _currentRoom.SetPlayer(0, 0, NetPlayer);
            return _currentRoom;
        }

        internal void SetCurrentRoom(Room a_room)
        {
            _currentRoom = a_room;
        }
        #endregion
    }
}