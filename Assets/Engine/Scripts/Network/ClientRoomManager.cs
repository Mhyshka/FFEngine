using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using Zeroconf;
using FF.Multiplayer;
using FF.Network.Message;
using FF.Network.Receiver;

internal enum EFarewellCode
{
    Shuttingdown,
}

namespace FF.Network
{
    internal class ClientRoomManager : BaseManager
    {
        #region Properties
        protected Dictionary<IPEndPoint, FFClientWrapper> _roomClients;
        internal Dictionary<IPEndPoint, FFClientWrapper> Clients
        {
            get
            {
                return _roomClients;
            }
        }

        protected Dictionary<IPEndPoint, Room> _rooms;

        internal Multiplayer.RoomCallback onRoomRemoved;
        internal Multiplayer.RoomCallback onRoomAdded;

        protected bool _isLookingForRoom = false;
        internal bool IsLookingForRoom
        {
            get
            {
                return _isLookingForRoom;
            }
        }

        protected NetworkManager _networkManager;

        protected GenericMessageReceiver _lookingForRoomReceiver = null;
        protected GenericMessageReceiver _currentRoomReceiver = null;
        protected GenericMessageReceiver _farewellReceiver = null;
        #endregion

        internal ClientRoomManager(NetworkManager a_networkManager)
        {
            _networkManager = a_networkManager;

            _rooms = new Dictionary<IPEndPoint, Room>();
            _roomClients = new Dictionary<IPEndPoint, FFClientWrapper>();

            _lookingForRoomReceiver = new GenericMessageReceiver(OnLookingForGamesRoomInfosReceived);
            _currentRoomReceiver = new GenericMessageReceiver(OnInGameRoomInfosReceived);
            _farewellReceiver = new GenericMessageReceiver(OnFarewellMessageReceived);
        }

        #region Manager
        internal override void TearDown()
        {
            base.TearDown();

            StopLookingForGames();
            ClearData();

            onRoomAdded = null;
            onRoomRemoved = null;
        }

        internal override void DoUpdate()
        {
            base.DoUpdate();

            foreach (FFClientWrapper each in _roomClients.Values)
            {
                if (each != null)
                    each.DoUpdate();
            }
        }

        protected void ClearData()
        {
            FFLog.Log(EDbgCat.RoomDiscovery, "Clearing data.");
            foreach (IPEndPoint endpoint in _roomClients.Keys)
            {
                _roomClients[endpoint].Close();
            }
            _roomClients.Clear();
            _rooms.Clear();
        }
        #endregion

        #region Interface
        internal void StartLookingForGames(string a_gameProtocol)
        {
            if (!_isLookingForRoom)
            {

                ZeroconfManager.Instance.Client.onStartDiscoverySuccess += OnStartDiscoverySuccess;
                ZeroconfManager.Instance.Client.onStartDiscoveryFailed += OnStartDiscoveryFailed;
                ZeroconfManager.Instance.Client.StartDiscovery(a_gameProtocol);
                _isLookingForRoom = true;
                Engine.Receiver.RegisterReceiver(EMessageChannel.RoomInfos.ToString(),
                                                    _lookingForRoomReceiver);
                FFLog.Log(EDbgCat.RoomDiscovery, "Start looking for games");
            }
            else
            {
                FFLog.LogWarning(EDbgCat.RoomDiscovery, "Can't start looking for games - Already looking for games.");
            }
        }

        internal void StopLookingForGames()
        {
            if (_isLookingForRoom)
            {
                _isLookingForRoom = false;
                ZeroconfManager.Instance.Client.StopDiscovery();
                ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
                ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
                ZeroconfManager.Instance.Client.onRoomAdded -= OnServiceFound;
                ZeroconfManager.Instance.Client.onRoomLost -= OnServiceLost;

                ClearData();

                Engine.Receiver.UnregisterReceiver(EMessageChannel.RoomInfos.ToString(),
                                                    _lookingForRoomReceiver);

                FFLog.Log(EDbgCat.RoomDiscovery, "Stop looking for games");
            }
            else
            {
                FFLog.LogWarning(EDbgCat.RoomDiscovery, "Can't stop looking for games - Wasn't looking for games.");
            }
        }

        protected void OnStartDiscoverySuccess()
        {
            ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
            ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
            FFLog.Log(EDbgCat.RoomDiscovery, "Start discovery success. Looking for rooms.");

            ZeroconfManager.Instance.Client.onRoomAdded += OnServiceFound;
            ZeroconfManager.Instance.Client.onRoomLost += OnServiceLost;
        }

        protected void OnStartDiscoveryFailed()
        {
            ZeroconfManager.Instance.Client.onStartDiscoverySuccess -= OnStartDiscoverySuccess;
            ZeroconfManager.Instance.Client.onStartDiscoveryFailed -= OnStartDiscoveryFailed;
            FFLog.LogWarning(EDbgCat.RoomDiscovery, "Start discovery failed.");
        }
        #endregion

        #region Room Discovery
        protected FFClientWrapper ContactRoom(ZeroconfRoom a_room)
        {
            FFClientWrapper newClient = new FFClientWrapper(new IPEndPoint(_networkManager.PreferedNetworkAddress, 0),
                                                                            a_room.EndPoint);
            newClient.Connect();
            return newClient;
        }

        protected void OnServiceFound(ZeroconfRoom a_room)
        {
            _roomClients.Add(a_room.EndPoint, ContactRoom(a_room));
            FFLog.Log(EDbgCat.RoomDiscovery, "Service found. Adding new room : " + a_room.roomName.ToString() + " - " + a_room.EndPoint.ToString());
        }

        protected void OnServiceLost(ZeroconfRoom a_room)
        {
            FFLog.Log(EDbgCat.RoomDiscovery, "Service lost. Removing room : " + a_room.roomName.ToString());
            if (a_room.EndPoint != null)
            {
                FFClientWrapper client = null;
                if (_roomClients.TryGetValue(a_room.EndPoint, out client))
                {
                    _roomClients[a_room.EndPoint].Close();
                    _roomClients.Remove(a_room.EndPoint);
                }

                Room room = null;
                if (_rooms.TryGetValue(a_room.EndPoint, out room))
                {
                    _rooms.Remove(a_room.EndPoint);
                    if (onRoomRemoved != null)
                        onRoomRemoved(room);
                }
            }
        }

        protected void OnLookingForGamesRoomInfosReceived(ReadMessage a_message)
        {
            if (a_message.Data.Type == EDataType.Room)
            {
                MessageRoomData data = a_message.Data as MessageRoomData;
                Room room = data.Room;
                room.serverEndPoint = a_message.Client.Remote;

                if (_rooms.ContainsKey(room.serverEndPoint))//A room in the list was updated
                {
                    FFLog.Log(EDbgCat.RoomDiscovery, "Room infos received -> Updating room infos");
                    _rooms[room.serverEndPoint].UpdateWithRoom(room);
                }
                else//A new room can be display in the list
                {
                    FFLog.Log(EDbgCat.RoomDiscovery, "Room infos received -> Room Added");
                    _rooms.Add(room.serverEndPoint, room);
                    if (onRoomAdded != null)
                        onRoomAdded(room);
                }
            }
        }

        protected void OnInGameRoomInfosReceived(ReadMessage a_message)
        {
            if (a_message.Data.Type == EDataType.Room)
            {
                MessageRoomData data = a_message.Data as MessageRoomData;
                Room room = data.Room;
                room.serverEndPoint = a_message.Client.Remote;

                if (room.serverEndPoint == Engine.Network.CurrentRoom.serverEndPoint)
                {
                    Engine.Network.CurrentRoom.UpdateWithRoom(room);
                    FFLog.Log(EDbgCat.RoomDiscovery, "Current room updated.");
                }

                if (Engine.Network.CurrentRoom.PlayerForId(Engine.Network.MainClient.NetworkID) == null)//Removed from room
                {
                    Engine.Network.LeaveCurrentRoom(true);
                }
            }
        }
        #endregion

        internal void JoinRoom(IPEndPoint a_roomEndpoint, ref Room a_currentRoom, ref FFClientWrapper a_mainClient)
        {
            FFLog.Log(EDbgCat.RoomDiscovery, "Joining room.");

            if (_rooms.TryGetValue(a_roomEndpoint, out a_currentRoom))
            {
                _rooms.Remove(a_roomEndpoint);
            }

            if (_roomClients.TryGetValue(a_roomEndpoint, out a_mainClient))
            {
                _roomClients.Remove(a_roomEndpoint);
            }
            RegisterRoomReceivers();
        }

        internal void LeaveCurrentRoom()
        {
            FFLog.Log(EDbgCat.RoomDiscovery, "Leaving room.");
            UnregisterRoomReceivers();
        }

        internal void RegisterRoomReceivers()
        {
            Engine.Receiver.RegisterReceiver(EMessageChannel.RoomInfos.ToString(),
                                                    _currentRoomReceiver);

            Engine.Receiver.RegisterReceiver(EMessageChannel.Farewell.ToString(),
                                                _farewellReceiver);
        }

        void UnregisterRoomReceivers()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.RoomInfos.ToString(),
                                                                _currentRoomReceiver);
            Engine.Receiver.UnregisterReceiver(EMessageChannel.Farewell.ToString(),
                                                _farewellReceiver);
        }

        protected void OnFarewellMessageReceived(ReadMessage a_message)
        {
            if (a_message.Data.Type == EDataType.Integer)
            {
                MessageIntegerData data = a_message.Data as MessageIntegerData;
                //TODO Localize
                EFarewellCode code = (EFarewellCode)data.Data;
                Engine.Network.MainClient.Disconnect();
            }
        }
    }
}

/*
//CURRENT ROOM UPDATED
if (_mainClient != null && a_room.serverEndPoint == _mainClient.Remote)
{
    FFLog.Log(EDbgCat.RoomDiscovery, "Updating main room infos");
    Engine.Network.CurrentRoom.UpdateWithRoom(a_room);
    if (Engine.Network.CurrentRoom.PlayerForId(NetworkID) == null)
    {
        _mainClient.Disconnect();
    }
}
*/
