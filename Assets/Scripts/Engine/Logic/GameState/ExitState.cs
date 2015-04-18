using UnityEngine;
using System.Collections;

// Custom editor
public class ExitState : AGameState
{
	#region Inspector Properties
	public string gameModeToLoad = "MenuGameMode";
	#endregion

	#region Properties
	#endregion

	#region States Methods
	internal override int ID
	{
		get
		{
			return (int)EStateID.Exit;
		}
	}

	internal override void Enter ()
	{
		base.Enter ();
		FFEngine.Game.RequestGameMode(gameModeToLoad);
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
	/*protected override void RegisterForEvent ()
	{
		base.RegisterForEvent ();
	}

	protected override void UnregisterForEvent ()
	{
		base.UnregisterForEvent ();
	}

	internal void OnEvent(FFEventParameter a_args)
	{
	}
	*/
	#endregion
	
}
