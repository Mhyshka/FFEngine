using UnityEngine;
using System.Collections;

namespace FFEngine
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
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
		}

		
		internal void OnEvent(FFEventParameter a_args)
		{
		}
		#endregion

	}
}