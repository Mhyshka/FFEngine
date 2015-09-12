using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using FF.UI;

namespace FF
{
	internal class GameRoomSearchState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion
		
		#region Properties
		protected FFHostListPanel _hostListPanel;
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
			
			if(_hostListPanel == null)
				_hostListPanel = FFEngine.UI.GetPanel("HostListPanel") as FFHostListPanel;
			
			if(FFEngine.Network.IsConnectedToLan())
			{
				_navigationPanel.setTitle ("Looking for games");
				FFEngine.Network.StartLookingForGames ();
			}
			else
			{
				_navigationPanel.setTitle ("No network");
			}
		}

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
			
			FFEngine.Network.StopLookingForGames ();
			ZeroconfManager.Instance.Client.onRoomAdded -= OnRoomAdded;
			ZeroconfManager.Instance.Client.onRoomLost -= OnRoomLost;
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

		protected void OnRoomAdded (ZeroconfRoom aRoom)
		{
			FFLog.LogError ("OnRoomAdded : " + aRoom.roomName);
			_hostListPanel.AddRoom (aRoom);
		}

		protected void OnRoomLost(ZeroconfRoom aRoom)
		{
			_hostListPanel.RemoveRoom (aRoom);
		}


		#endregion

	}
}