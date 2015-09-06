#define DEBUG_LOG

using UnityEngine;
using System.Collections;

internal enum EDbgCat
{
	Zeroconf,
	Networking,
	UI
}

internal class FFLog
{
	#region Log Debug
	internal static void Log(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		Debug.Log(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_tag, string a_text)
	{
#if DEBUG_LOG
		Debug.Log(a_tag + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_text)
	{
#if DEBUG_LOG
		Debug.Log(a_text);
#endif
	}
	#endregion
	
	#region Log Warning
	internal static void LogWarning(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		Debug.LogWarning(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_tag, string a_text)
	{
#if DEBUG_LOG
		Debug.LogWarning(a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_text)
	{
#if DEBUG_LOG
		Debug.LogWarning(a_text);
#endif
	}
	#endregion
	
	#region Log Error
	internal static void LogError(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		Debug.LogError(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_tag, string a_text)
	{
#if DEBUG_LOG
		Debug.LogError(a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_text)
	{
#if DEBUG_LOG
		Debug.LogError(a_text);
#endif
	}
	#endregion
}