using UnityEngine;
using System.Collections;
using System;

using FF.Utils;

internal delegate void InternetStatusCallback(bool a_canReachInternet);

internal class FFInternetStatusManager
{
	#region Properties
	protected bool _canReachInternet = false;
	protected DateTime _lastUpdate;
	protected static int UPDATE_TIMESPAN = 1;
	protected TimeSpan _timespan;
	#endregion

	#region Constructor
	internal FFInternetStatusManager()
	{
		_canReachInternet = false;
		_timespan = new TimeSpan(0,0,UPDATE_TIMESPAN);
	}
	#endregion

	#region Methods
	internal void Update()
	{
		if(onInternetStatusChanged != null && onInternetStatusChanged.GetInvocationList().Length > 0)
		{
			bool previousState = _canReachInternet;
			bool newState = CanReachInternet;
			if(previousState != newState)
			{
				onInternetStatusChanged(_canReachInternet);
			}
		}
	}

	internal InternetStatusCallback onInternetStatusChanged = null;
	internal bool CanReachInternet
	{
		get
		{
			if(DateTime.Now - _lastUpdate > _timespan)
			{
				_lastUpdate = DateTime.Now;
				_canReachInternet = RefreshConnectionStatus();
			}
			return _canReachInternet;
		}
	}

	internal bool RefreshConnectionStatus()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		return FFAndroidUtils.HasInternetConnection();
		#else
		return Application.internetReachability != NetworkReachability.NotReachable;
		#endif
	}
	
	internal bool IsWifiInternetConnection
	{
		get
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			return FFAndroidUtils.HasWifiInternetConnection();
			#else
			return Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork;
			#endif
		}
	}
	#endregion
}
