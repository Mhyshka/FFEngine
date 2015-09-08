using UnityEngine;
using System.Collections;

namespace FF
{
	internal class HostListState : AMenuGameState
	{
		#region Inspector Properties
		#endregion

		#region Properties
		#endregion

		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.HostList;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log(EDbgCat.Logic,"Host List state enter.");

			FFNavigationBarPanel lNavigationBarPanel = FFEngine.UI.GetPanel ("NavigationBarPanel") as FFNavigationBarPanel;
			lNavigationBarPanel.setTitle ("Alex est un blaireaudoudou");
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
			Debug.Log ("test event");
			Debug.Log (a_args);
		}


		#endregion

	}
}