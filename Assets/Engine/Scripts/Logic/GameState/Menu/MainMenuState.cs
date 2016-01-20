using UnityEngine;
using System.Collections;
using FF.Network;

namespace FF
{
	internal class MainMenuState : AMenuGameState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		#endregion

		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.Main;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log(EDbgCat.Logic,"Main menu state enter.");
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
			Engine.Events.RegisterForEvent ("OnPlayPressed", OnPlayPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			Engine.Events.UnregisterForEvent ("OnPlayPressed", OnPlayPressed);
		}

		
		internal void OnEvent(FFEventParameter a_args)
		{
			Debug.Log ("test event");
			Debug.Log (a_args);
		}


		internal void OnPlayPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Main menu state - OnPlayPressed");
			RequestState ((int)EMenuStateID.ModeSelection);
		}
		#endregion

	}
}