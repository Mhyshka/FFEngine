using UnityEngine;
using System.Collections;
using System;

using FF.Utils;

internal delegate void InternetStatusCallback(bool a_canReachInternet);

internal class FFNetworkStatusManager
{
	#region Properties
	protected bool _isConnectedToLan = false;
	protected DateTime _lastUpdate;
	protected static int UPDATE_TIMESPAN = 1;
	protected TimeSpan _timespan;
	#endregion

	#region Constructor
	internal FFNetworkStatusManager()
	{
		_isConnectedToLan = false;
		_timespan = new TimeSpan(0,0,UPDATE_TIMESPAN);
	}
	#endregion

	#region Methods
	internal void DoUpdate()
	{
		if(onLanStatusChanged != null && onLanStatusChanged.GetInvocationList().Length > 0)
		{
            bool previousState = _isConnectedToLan;
			bool newState = IsConnectedToLan;
			if(previousState != newState)
			{
				onLanStatusChanged(newState);
			}
		}
	}

	internal InternetStatusCallback onLanStatusChanged = null;
	internal bool IsConnectedToLan
	{
		get
		{
			if(DateTime.Now - _lastUpdate > _timespan)
			{
				_lastUpdate = DateTime.Now;
				_isConnectedToLan = RefreshLanConnectionStatus();
			}
			return _isConnectedToLan;
		}
	}

	protected bool RefreshLanConnectionStatus()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return FFAndroidUtils.HasWifiInternetConnection();
#else
        return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		#endif
	}
    #endregion

    #region Internet
    protected bool RefreshConnectionStatus()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
		return FFAndroidUtils.HasInternetConnection();
#else
        return Application.internetReachability != NetworkReachability.NotReachable;
#endif
    }
    #endregion
}
