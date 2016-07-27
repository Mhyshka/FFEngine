using UnityEngine;
using System.Collections;
using System.Net;

namespace FF.Logic
{
	/// <summary>
	/// Basic Game Manager that handles loading of GameModes.
	/// </summary>
	internal class NetworkGameManager : GameManager
	{
        #region Placeholder Profile
        internal int portNumber = 0;
        internal bool autoPort = true;
        internal IPAddress targetAddress = null;
        #endregion

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
        #endregion

        #region Room Properties
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
    }
}