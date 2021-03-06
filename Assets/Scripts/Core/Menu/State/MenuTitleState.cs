﻿using UnityEngine;
using System.Collections;

internal class MenuTitleState : AMenuGameState
{
	#region Inspector Properties
	#endregion

	#region Properties
	#endregion

	#region States Methods
	internal override int ID
	{
		get
		{
			return (int)EMenuStateID.Title;
		}
	}

	internal override void Enter ()
	{
		base.Enter ();
	}

	internal override int Manage ()
	{
		return base.Manage ();
	}

	internal override void Exit ()
	{
		base.Exit ();
	}
	#endregion

	#region Event Management
	protected override void RegisterForEvent ()
	{
		base.RegisterForEvent ();
		FFEngine.Events.RegisterForEvent("Settings", OnSettingsSelected);
		FFEngine.Events.RegisterForEvent("Play", OnPlaySelected);
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
		FFEngine.Events.UnregisterForEvent("Settings", OnSettingsSelected);
		FFEngine.Events.RegisterForEvent("Play", OnPlaySelected);
	}

	
	internal void OnSettingsSelected(FFEventParameter a_args)
	{
		RequestState((int)EMenuStateID.Settings);
	}
	
	internal void OnPlaySelected(FFEventParameter a_args)
	{
		RequestGameMode("RTSGameMode");
	}
	#endregion

}
