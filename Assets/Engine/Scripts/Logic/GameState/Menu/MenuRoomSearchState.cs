using UnityEngine;
using System.Collections;

using FF.UI;
using FF.Multiplayer;

namespace FF
{
    //HostList
	internal class MenuRoomSearchState : ANavigationMenuState
	{
		#region Inspector Properties
		#endregion
		
		#region Properties
		protected FFSearchRoomPanel _searchRoomPanel;
		#endregion
		
		#region States Methods
		internal override int ID
        {
			get
            {
				return (int)EMenuStateID.SearchForRooms;
			}
		}

		internal override void Enter ()
		{
			base.Enter ();
			FFLog.Log (EDbgCat.Logic, "Host List state enter.");
			
			if(_searchRoomPanel == null)
				_searchRoomPanel = Engine.UI.GetPanel("MenuSearchRoomPanel") as FFSearchRoomPanel;
				
			if(Engine.Inputs.ShouldUseNavigation)
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
            if (Engine.NetworkStatus.IsConnectedToLan)
			{
				_navigationPanel.SetTitle ("Looking for games");

                if (!Engine.Network.IsLookingForRoom)// Not yet looking for room.
                {
                    _searchRoomPanel.ClearRoomsCells();
                    Engine.Network.StartLookingForGames();

                    Engine.Network.onNewRoomReceived += OnRoomAdded;
                    Engine.Network.onRoomLost += OnRoomLost;
                }

                _navigationPanel.ShowLoader("Searching");
                _navigationPanel.HideWifiWarning();
            }
			else
			{
                _navigationPanel.HideLoader();
                _navigationPanel.ShowWifiWarning();
            }
		}

        internal override void GoBack()
        {
            Engine.Network.StopLookingForGames();
            base.GoBack();
        }

        protected void TearDown()
		{
            _navigationPanel.HideLoader();

            if (!Engine.Network.IsLookingForRoom)// Not looking for room anymore.
            {
                _searchRoomPanel.ClearRoomsCells();
                Engine.Network.onNewRoomReceived -= OnRoomAdded;
                Engine.Network.onRoomLost -= OnRoomLost;
            }
        }
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.Events.RegisterForEvent(FFEventType.Connect, OnConnectButtonPressed);

            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;
        }

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			Engine.Events.UnregisterForEvent(FFEventType.Connect, OnConnectButtonPressed);

            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;
        }
		
		internal void OnConnectButtonPressed(FFEventParameter a_args)
		{
            if (a_args.data == null)
            {
                FFLog.LogError(EDbgCat.Logic, "Room is null");
                return;
            }

			Room selectedRoom = a_args.data as Room;
            Engine.Network.SetMainClient(selectedRoom);

            RequestState(outState.ID);
		}
		#endregion
		
		#region List Management
		internal void OnRoomAdded (Room aRoom)
		{
            FFLog.LogError(EDbgCat.Logic,"Room added");
			_searchRoomPanel.AddRoom (aRoom);
		}
		
		internal void OnRoomLost(Room aRoom)
		{
			_searchRoomPanel.RemoveRoom (aRoom);
            _navigationPanel.FocusBackButton();
		}

        internal void OnLanStatusChanged(bool a_state)
        {
            if (a_state)
            {
                ResetState();
            }
            else
            {
                Engine.Network.StopLookingForGames();
                TearDown();
                _navigationPanel.ShowWifiWarning();
            }
        }
		#endregion
		
		#region Pause
		internal override void OnPause ()
		{
			base.OnPause ();
            Engine.Network.StopLookingForGames();
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
            if (Engine.Inputs.ShouldUseNavigation)
            {
                _navigationPanel.FocusBackButton();
            }
        }
        #endregion
    }
}