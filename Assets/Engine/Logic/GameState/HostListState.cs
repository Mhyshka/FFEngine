using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;



namespace FF
{
	internal class HostListState : AMenuGameState
	{
		#region Inspector Properties
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
			FFLog.Log (EDbgCat.Logic, "Host List state enter.");

			FFNavigationBarPanel lNavigationBarPanel = FFEngine.UI.GetPanel ("NavigationBarPanel") as FFNavigationBarPanel;
			lNavigationBarPanel.setTitle ("Alex est un blaireaudoudou");

			FFEngine.Network.StartLookingForGames ();
			ZeroconfManager.Instance.Client.onRoomAdded += OnRoomAdded;
			ZeroconfManager.Instance.Client.onRoomLost += OnRoomLost;
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


		protected void OnRoomAdded (ZeroconfRoom aRoom)
		{
			Debug.Log ("OnRoomAdded" + aRoom.roomName);
			FFHostListPanel lPanel = FFEngine.UI.GetPanel ("HostListPanel") as FFHostListPanel;
			Debug.Log ("host list panel get");
			lPanel.AddRoom (aRoom);
		}

		protected void OnRoomLost(ZeroconfRoom aRoom)
		{
			FFHostListPanel lPanel = FFEngine.UI.GetPanel ("HostListPanel") as FFHostListPanel;
			lPanel.RemoveRoom (aRoom);
		}


		#endregion

	}
}