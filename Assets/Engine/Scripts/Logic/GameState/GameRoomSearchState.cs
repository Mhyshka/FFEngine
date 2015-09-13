using UnityEngine;
using System.Collections;
using Zeroconf;
using System.Collections.Generic;

using FF.UI;
using FF.Networking;

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

			ResetState();
		}

		internal override int Manage ()
		{
			return base.Manage ();
		}

		internal override void Exit ()
		{
			base.Exit ();
			
			TearDown();
		}
		#endregion
		
		#region Init & TearDown
		protected void ResetState()
		{
			_hostListPanel.ClearRoomsCells();
			
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
		
		protected void TearDown()
		{
			FFEngine.Network.StopLookingForGames ();
		}
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent(EEventType.Connect, OnConnectButtonPressed);
		}

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent(EEventType.Connect, OnConnectButtonPressed);
		}

		
		
		internal void OnConnectButtonPressed(FFEventParameter a_args)
		{
			FFLog.LogError("Connect Callback");
			if(a_args.data == null)
				FFLog.LogError("Room is null");
			FFRoom selectedRoom = a_args.data as FFRoom;
			FFEngine.Network.JoinGame(selectedRoom);
		}
		#endregion
		
		#region List Management
		internal void OnRoomAdded (FFRoom aRoom)
		{
			_hostListPanel.AddRoom (aRoom);
		}
		
		internal void OnRoomLost(FFRoom aRoom)
		{
			_hostListPanel.RemoveRoom (aRoom);
		}
		#endregion
		
		#region Pause
		internal override void OnPause ()
		{
			base.OnPause ();
			TearDown();
		}
		
		internal override void OnResume ()
		{
			base.OnResume ();
			ResetState();
		}
		#endregion
	}
}