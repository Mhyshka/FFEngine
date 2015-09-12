using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;



public class iOSBackgroundTask 
{
	#region Background task
	[DllImport ("__Internal")]
	private static extern void startBackgroundTask(string alertTitle, string alertBody, string alertAction, int deltaDealTime);
	[DllImport ("__Internal")]
	private static extern float registerForPushNotification();


	// Use this for initialization
	internal static void ios_registerForPushNotification () 
	{
		#if UNITY_IOS && !UNITY_EDITOR
		registerForPushNotification ();
		#endif
	}
	
	// Update is called once per frame
	internal static void ios_startBackgroundTask (string alertTitle, string alertBody, string alertAction, int deltaDealTime) 
	{
		#if UNITY_IOS && !UNITY_EDITOR
		startBackgroundTask(alertTitle, alertBody, alertAction, deltaDealTime);
		#endif
	}
	#endregion
}
