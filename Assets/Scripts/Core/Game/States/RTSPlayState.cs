using UnityEngine;
using System.Collections;

public class RTSPlayState : AGameState 
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
			return (int)EStateID.Game;
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
		FFEngine.Inputs.RegisterOnEventKey(EInputEventKey.Quit, OnQuitPressed);
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
		FFEngine.Inputs.UnregisterOnEventKey(EInputEventKey.Quit, OnQuitPressed);
	}

	
	internal void OnQuitPressed()
	{
		RequestState(outState.ID);
	}
	
	#endregion

}
