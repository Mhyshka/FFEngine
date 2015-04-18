using UnityEngine;
using System.Collections;

internal class GameplayState : AGameState
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
		FFEngine.Events.RegisterForEvent(EEventType.NextState, OnNextState);
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
		FFEngine.Events.UnregisterForEvent(EEventType.NextState, OnNextState);
	}

	internal void OnNextState(FFEventParameter a_args)
	{
		RequestState(outState.ID);
	}
	#endregion
}
