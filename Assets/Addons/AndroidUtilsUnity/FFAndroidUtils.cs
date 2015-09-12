using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF.Utils
{
	internal class FFAndroidUtils
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaClass s_javaClient;
		#endif
		
		private static void PrepareJavaClient()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			if(s_javaClient == null)
				s_javaClient = new AndroidJavaClass("com.flamingfist.utils.AndroidUtils");
			#endif
		}
		
		internal static bool HasInternetConnection()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			PrepareJavaClient();
			return s_javaClient.CallStatic<bool>("HasInternetConnection");
			#else
			return false;
			#endif
		}
		
		internal static bool HasWifiInternetConnection()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			PrepareJavaClient();
			return s_javaClient.CallStatic<bool>("HasWifiInternetConnection");
			#else
			return false;
			#endif
		}
		
		internal static bool IsAndroidTV()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			PrepareJavaClient();
			return s_javaClient.CallStatic<bool>("IsAndroidTV");
			#else
			return false;
			#endif
		}
	}
}