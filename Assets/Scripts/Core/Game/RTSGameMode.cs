using UnityEngine;
using System.Collections;

public class RTSGameMode : AGameMode
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
			return (int)EGameModeID.Game;
		}
	}

	protected override void Enter ()
	{
		base.Enter ();
	}

	protected override void Update ()
	{
		base.Update ();
	}

	protected override void Exit ()
	{
		base.Exit ();
	}
	#endregion

	#region Event Management
	protected override void RegisterForEvent ()
	{
		base.RegisterForEvent ();
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
	}

	/*
	internal void OnEvent(FFEventParameter a_args)
	{
	}
	*/
	#endregion

}
