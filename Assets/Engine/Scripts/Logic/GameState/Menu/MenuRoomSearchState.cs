using UnityEngine;
using System.Collections;
using System.Net;

using FF.Network;
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

                if (!Engine.Network.ClientRoomManager.IsLookingForRoom)// Not yet looking for room.
                {
                    Engine.Network.StartLookingForGames();
                    _searchRoomPanel.ClearRoomsCells();
                    Engine.Network.ClientRoomManager.onRoomAdded += OnRoomAdded;
                    Engine.Network.ClientRoomManager.onRoomRemoved += OnRoomLost;
                }

                LoadingIndicatorPanel loadingIndicator = Engine.UI.GetPanel("LoadingIndicatorPanel") as LoadingIndicatorPanel;
                loadingIndicator.SetDescription("Searching");
                Engine.UI.RequestDisplay("LoadingIndicatorPanel");

                Engine.UI.HideSpecificPanel("WifiWarningPanel");
            }
			else
			{
                Engine.UI.HideSpecificPanel("LoadingIndicatorPanel");
                Engine.UI.RequestDisplay("WifiWarningPanel");
            }
		}

        protected void TearDown()
		{
            Engine.UI.HideSpecificPanel("LoadingIndicatorPanel");

            if (Engine.Network.ClientRoomManager.IsLookingForRoom)
            {
                Engine.Network.StopLookingForGames();

                _searchRoomPanel.ClearRoomsCells();
                Engine.Network.ClientRoomManager.onRoomAdded -= OnRoomAdded;
                Engine.Network.ClientRoomManager.onRoomRemoved -= OnRoomLost;
            }

        }
		#endregion

		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			Engine.Events.RegisterForEvent(FFEventType.Connect, OnConnectButtonPressed);
            Engine.Events.RegisterForEvent("DirectConnect", OnDirectConnectPressed);

            Engine.NetworkStatus.onLanStatusChanged += OnLanStatusChanged;
        }

		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			Engine.Events.UnregisterForEvent(FFEventType.Connect, OnConnectButtonPressed);
            Engine.Events.UnregisterForEvent("DirectConnect", OnDirectConnectPressed);

            Engine.NetworkStatus.onLanStatusChanged -= OnLanStatusChanged;
        }
		
		void OnConnectButtonPressed(FFEventParameter a_args)
		{
            if (a_args.data == null)
            {
                FFLog.LogError(EDbgCat.Logic, "Room is null");
                return;
            }

			Room selectedRoom = a_args.data as Room;
            Engine.Network.JoinRoom(selectedRoom.serverEndPoint);

            RequestState(outState.ID);
		}

        void OnDirectConnectPressed(FFEventParameter a_args)
        {
            if (_searchRoomPanel.directConnectInputField.IsValid)
            {
                string input = _searchRoomPanel.directConnectInputField.Value;
                string ipString = input.Substring(0, input.Length - 6);
                string portString = input.Substring(input.Length - 5, 5);

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipString), int.Parse(portString));
                MenuDirectConnectState directCoState = _gameMode.StateForId((int)EMenuStateID.DirectConnect) as MenuDirectConnectState;
                directCoState.targetEndpoint = endpoint;

                RequestState((int)EMenuStateID.DirectConnect);
            }
            else
            {
                FFMessageToast.RequestDisplay("IP Endpoint is invalid.");
            }
        }
        #endregion

        #region List Management
        internal void OnRoomAdded (Room aRoom)
		{
            FFLog.LogError(EDbgCat.Logic,"Room added");
			_searchRoomPanel.AddRoom (aRoom);

            FFClientWrapper serverClient = null;
            if (Engine.Network.ClientRoomManager.Clients.TryGetValue(aRoom.serverEndPoint, out serverClient))
            {
                serverClient.Clock.onLatencyUpdate += OnLatencyUpdate;
            }
        }
		
		internal void OnRoomLost(Room aRoom)
		{
			_searchRoomPanel.RemoveRoom (aRoom);
            _navigationPanel.FocusBackButton();

            FFClientWrapper serverClient = null;
            if (Engine.Network.ClientRoomManager.Clients.TryGetValue(aRoom.serverEndPoint, out serverClient))
            {
                serverClient.Clock.onLatencyUpdate -= OnLatencyUpdate;
            }
        }

        protected void OnLatencyUpdate(FFNetworkClient a_client, float a_latency)
        {
            FFRoomCellWidget widget = _searchRoomPanel.RoomWidgetForEP(a_client.Remote);
            if(widget != null)
                widget.UpdateLatency(a_latency);
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
                Engine.UI.RequestDisplay("WifiWarningPanel");
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
            if (Engine.Inputs.ShouldUseNavigation)
            {
                _navigationPanel.FocusBackButton();
            }
        }
        #endregion
    }
}