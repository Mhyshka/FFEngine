using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

using Zeroconf;
using FF.UI;
using FF.Networking;

namespace FF
{
	internal class MenuRoomSearchState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion
		
		#region Properties
		protected FFHostListPanel _hostListPanel;
		protected FFJoinRoomRequest _joinRequest;
		#endregion
		
		#region States Methods
		internal override int ID {
			get {
				return (int)EMenuStateID.SearchForRooms;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log (EDbgCat.Logic, "Host List state enter.");
			
			if(_hostListPanel == null)
				_hostListPanel = FFEngine.UI.GetPanel("MenuRoomListPanel") as FFHostListPanel;
				
			if(FFEngine.Inputs.ShouldUseNavigation)
			{
				_navigationPanel.FocusBackButton();
			}

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
			_joinRequest = null;
			_hostListPanel.ClearRoomsCells();

            if (FFEngine.NetworkStatus.IsConnectedToLan)
			{
				_navigationPanel.SetTitle ("Looking for games");
				FFEngine.Network.StartLookingForGames ();
				
				FFEngine.Network.onNewRoomReceived += OnRoomAdded;
				FFEngine.Network.onRoomLost += OnRoomLost;

                _navigationPanel.ShowLoader("Searching");
                _navigationPanel.HideWifiWarning();
            }
			else
			{
                _navigationPanel.HideLoader();
                _navigationPanel.ShowWifiWarning();
            }
		}
		
		protected void TearDown()
		{
            _hostListPanel.ClearRoomsCells();
            FFEngine.Network.StopLookingForGames ();
            _navigationPanel.HideLoader();
        }
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent(EEventType.Connect, OnConnectButtonPressed);

            FFEngine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;
        }

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent(EEventType.Connect, OnConnectButtonPressed);
			
			FFEngine.Network.onNewRoomReceived -= OnRoomAdded;
			FFEngine.Network.onRoomLost -= OnRoomLost;

            FFEngine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;
        }
		
		internal void OnConnectButtonPressed(FFEventParameter a_args)
		{
			FFLog.LogError("Connect Callback");
            if (a_args.data == null)
            {
                FFLog.LogError("Room is null");
                return;
            }

			FFRoom selectedRoom = a_args.data as FFRoom;
            FFEngine.Network.SetMainClient(selectedRoom);

            RequestState(outState.ID);
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

        internal void OnLanStatusChanged(bool a_state)
        {
            if (a_state)
            {
                ResetState();
            }
            else
            {
                TearDown();
                _navigationPanel.ShowWifiWarning();
            }
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

        #region focus
        internal override void OnGetFocus()
        {
            base.OnGetFocus();
            if (FFEngine.Inputs.ShouldUseNavigation)
            {
                _navigationPanel.FocusBackButton();
            }
        }
        #endregion
    }
}