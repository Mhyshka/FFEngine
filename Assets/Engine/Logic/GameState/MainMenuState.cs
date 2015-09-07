using UnityEngine;
using System.Collections;
using FF.Networking;

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
			FFTcpServer server = new FFTcpServer(FFEngine.Network.NetworkIP);
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
			FFEngine.Events.RegisterForEvent ("OnPlayPressed", OnPlayPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent ("OnPlayPressed", OnPlayPressed);
		}

		
		internal void OnEvent(FFEventParameter a_args)
		{
			Debug.Log ("test event");
			Debug.Log (a_args);
		}


		internal void OnPlayPressed(FFEventParameter a_args)
		{
			FFLog.Log(EDbgCat.Logic,"Main menu state - OnPlayPressed");
			Debug.Log (a_args);
		}
		#endregion

	}
}