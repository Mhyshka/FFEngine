#define DEBUG_LOG

using UnityEngine;
using System;
using System.Collections;

internal enum EDbgCat
{
    //Networking
    Zeroconf,

    Client,
    ClientClock,
    ClientConnection,
    ClientIdentification,

	Networking,
    Room,
    RoomDiscovery,
    RoomBroadcast,

    ServerTcp,
    ServerGame,
    ServerMock,
    ServerListening,

    Receiver,
    Message,
    Socket,
    NetworkSerialization,

    Handler,
	UI,
	Logic,
    Input
}

internal enum EDbgLevel
{
	Debug,
	Warning,
	Error
}

internal class FFLog
{
    protected static bool SHOW_TIMESTAMP = true;

	internal static EDbgLevel DBG_LEVEL = EDbgLevel.Debug;
    internal static EDbgCat[] DBG_CAT = new EDbgCat[]
    {
        /*EDbgCat.Zeroconf,

        EDbgCat.Client,
        EDbgCat.ClientClock,
        EDbgCat.ClientConnection,
        EDbgCat.ClientIdentification,

        /*EDbgCat.Networking,
        EDbgCat.Room,
        EDbgCat.RoomDiscovery,
        EDbgCat.RoomBroadcast,*/

        /*EDbgCat.ServerTcp,
        EDbgCat.ServerGame,*/
        //EDbgCat.ServerMock,
        /*EDbgCat.ServerListening,

        EDbgCat.Receiver,
        EDbgCat.Message,
        EDbgCat.Socket,
        EDbgCat.NetworkSerialization,

        EDbgCat.Handler,
        EDbgCat.UI,
        EDbgCat.Logic,*/
        //EDbgCat.Input
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
			Debug.Log((SHOW_TIMESTAMP ? DateTime.Now.ToLongTimeString() : "") + " - " +
                        a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Debug)
			Debug.Log((SHOW_TIMESTAMP ? DateTime.Now.ToLongTimeString() : "") + " - " +
                        a_tag + " : " + a_text);
#endif
	}
	
	internal static void Log(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Debug)
			Debug.Log((SHOW_TIMESTAMP ? DateTime.Now.ToLongTimeString() : "") + " - " +
                        a_text);
#endif
	}
	#endregion
	
	#region Log Warning
	internal static void LogWarning(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning && HasCatEnable(a_cat))
			Debug.LogWarning((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " +
                                a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning)
			Debug.LogWarning((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " +
                                a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogWarning(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Warning)
			Debug.LogWarning((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " +
                                a_text);
#endif
	}
	#endregion
	
	#region Log Error
	internal static void LogError(EDbgCat a_cat, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error && HasCatEnable(a_cat))
			Debug.LogError((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " +
                            a_cat.ToString() + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_tag, string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error)
			Debug.LogError((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " +
                            a_tag + " : " + a_text);
#endif
	}
	
	internal static void LogError(string a_text)
	{
#if DEBUG_LOG
		if((int)DBG_LEVEL <= (int)EDbgLevel.Error)
			Debug.LogError((SHOW_TIMESTAMP ? DateTime.Now.ToShortTimeString() : "") + " - " + 
                            a_text);
#endif
	}
	#endregion
}