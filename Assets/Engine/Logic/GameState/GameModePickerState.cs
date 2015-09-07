using UnityEngine;
using System.Collections;

namespace FFEngine
{
	internal class GameModePickerState : AMenuGameState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		#endregion

		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.GameModePicker;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker state enter.");
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
			FFEngine.Events.RegisterForEvent ("onHostPressed", OnHostPressed);
			FFEngine.Events.RegisterForEvent ("onJoinPressed", OnJoinPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent ("onHostPressed", OnHostPressed);
			FFEngine.Events.UnregisterForEvent ("onJoinPressed", OnJoinPressed);
		}

		
		internal void OnEvent(FFEventParameter a_args)
		{
			Debug.Log ("test event");
			Debug.Log (a_args);
		}


		internal void OnHostPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnHostPressed");
			Debug.Log (a_args);
		}
		
		
		internal void OnJoinPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Game Mode Picker - OnJoinPressed");
			Debug.Log (a_args);
		}
		#endregion

	}
}