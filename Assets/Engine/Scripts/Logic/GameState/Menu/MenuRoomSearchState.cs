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

                if (!Engine.Network.IsLookingForRoom)// Not yet looking for room.
                {
                    _searchRoomPanel.ClearRoomsCells();
                    Engine.Network.StartLookingForGames();

                    Engine.Network.onNewRoomReceived += OnRoomAdded;
                    Engine.Network.onRoomLost += OnRoomLost;
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

        internal override void GoBack()
        {
            Engine.Network.StopLookingForGames();
            base.GoBack();
        }

        protected void TearDown()
		{
            Engine.UI.HideSpecificPanel("LoadingIndicatorPanel");

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

            _searchRoomPanel.ClearRoomsCells();

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

            FFTcpClient serverClient = null;
            if (Engine.Network.Clients.TryGetValue(aRoom.serverEndPoint, out serverClient))
            {
                serverClient.onLatencyUpdate += OnLatencyUpdate;
            }
        }
		
		internal void OnRoomLost(Room aRoom)
		{
			_searchRoomPanel.RemoveRoom (aRoom);
            _navigationPanel.FocusBackButton();

            FFTcpClient serverClient = null;
            if (Engine.Network.Clients.TryGetValue(aRoom.serverEndPoint, out serverClient))
            {
                serverClient.onLatencyUpdate -= OnLatencyUpdate;
            }
        }

        protected void OnLatencyUpdate(FFTcpClient a_client, double a_latency)
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
                Engine.Network.StopLookingForGames();
                TearDown();
                Engine.UI.RequestDisplay("WifiWarningPanel");
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