#define DEBUG_LOG

using UnityEngine;
using System.Collections;

internal enum EDbgCat
{
	Zeroconf,
	Networking,
    Message,
    Handler,
	UI,
	Logic
}

internal enum EDbgLevel
{
	Debug,
	Warning,
	Error
}

internal class FFLog
{
	internal static EDbgLevel DBG_LEVEL = EDbgLevel.Debug;
    internal static EDbgCat[] DBG_CAT = new EDbgCat[]
    { 
        /*EDbgCat.Zeroconf,
        EDbgCat.Networking,
        EDbgCat.Message,
        EDbgCat.Handler,
        */ EDbgCat.UI/*,
        EDbgCat.Logic*/
    };

    internal static bool HasCatEnable(EDbgCat a_cat)
    {
        foreach (EDbgCat each in DBG_CAT)
        {
            if (each == a_cat)
                return true;
        }
        return false;
    }

    #region Log Debug
    internal static void Log(EDbgCat a_cat, string a_text)
	{
	
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Debug && HasCatEnable(a_cat))
			Debug.Log(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Debug)
			Debug.Log(a_tag + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Debug)
			Debug.Log(a_text);
#endif
	}
	#endregion
	
	#region Log Warning
	internal static void LogWarning(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning && HasCatEnable(a_cat))
			Debug.LogWarning(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning)
			Debug.LogWarning(a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning)
			Debug.LogWarning(a_text);
#endif
	}
	#endregion
	
	#region Log Error
	internal static void LogError(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error && HasCatEnable(a_cat))
			Debug.LogError(a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error)
			Debug.LogError(a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error)
			Debug.LogError(a_text);
#endif
	}
	#endregion
}