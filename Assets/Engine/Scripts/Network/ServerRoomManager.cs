using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using Zeroconf;
using FF.Network.Message;
using FF.Network.Receiver;
using FF.Multiplayer;

namespace FF.Network
{
    internal class ServerRoomManager : BaseManager
    {
        protected bool _isBroadcastingGame = false;
        protected SimpleCallback _onSuccess = null;
        protected SimpleCallback _onFail = null;
        protected GenericMessageReceiver _leavingRoomReceiver = null;

        internal ServerRoomManager()
        {
            _leavingRoomReceiver = new GenericMessageReceiver(OnLeavingRoomReceived);
        }

        internal override void TearDown()
        {
            base.TearDown();
            StopBroadcastingGame();
        }

        #region Host
        internal void StartBroadcastingGame(string a_protocol, string a_roomName, int a_port, SimpleCallback a_onSuccess, SimpleCallback a_onfail)
        {
            if (!_isBroadcastingGame)
            {
                _isBroadcastingGame = true;
                _onSuccess = a_onSuccess;
                _onFail = a_onfail;
                ZeroconfManager.Instance.Host.onStartAdvertisingSuccess += OnStartAdvertisingSuccess;
                ZeroconfManager.Instance.Host.onStartAdvertisingFailed += OnStartAdvertisingFailed;
                ZeroconfManager.Instance.Host.StartAdvertising(a_protocol, a_roomName, a_port);
                FFLog.Log(EDbgCat.RoomBroadcast, "Start broadcasting game.");
            }
            else
            {
                FFLog.LogWarning(EDbgCat.RoomBroadcast, "Game is already broadcasted.");
            }
        }

        internal void StopBroadcastingGame()
        {
            if (_isBroadcastingGame)
            {
                _isBroadcastingGame = false;
                _onSuccess = null;
                _onFail = null;
                ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
                ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
                ZeroconfManager.Instance.Host.StopAdvertising();
                FFLog.Log(EDbgCat.RoomBroadcast, "Stop broadcasting game.");
            }
            else
            {
                FFLog.LogWarning(EDbgCat.RoomBroadcast, "Game isn't broadcasted yet.");
            }
        }
        #endregion

        #region Zeroconf Host
        protected void OnStartAdvertisingSuccess()
        {
            ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
            ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
            FFLog.Log(EDbgCat.RoomBroadcast, "Start advertising success.");

            if (_onSuccess != null)
                _onSuccess();
        }

        protected void OnStartAdvertisingFailed()
        {
            ZeroconfManager.Instance.Host.onStartAdvertisingSuccess -= OnStartAdvertisingSuccess;
            ZeroconfManager.Instance.Host.onStartAdvertisingFailed -= OnStartAdvertisingFailed;
            FFLog.LogError(EDbgCat.RoomBroadcast, "Start advertising failed.");

            if (_onFail != null)
                _onFail();
        }
        #endregion

        internal Room PrepareRoom(string a_roomName)
        {
            Room room = new Room();
            room.roomName = a_roomName;

            room.AddTeam(new Team("Left Side", 2, 1));
            room.AddTeam(new Team("Right Side", 2, 1));

            Team spectators = new Team("Spectators", 2, 0);
            spectators.Slots[0].isPlayableSlot = false;
            spectators.Slots[1].isPlayableSlot = false;
            room.AddTeam(spectators);
            return room;
        }

        internal void RegisterReceivers()
        {
            Engine.Receiver.RegisterReceiver(EMessageChannel.LeavingRoom.ToString(),
                                            _leavingRoomReceiver);
        }

        internal void UnregisterReceivers()
        {
            Engine.Receiver.UnregisterReceiver(EMessageChannel.LeavingRoom.ToString(),
                                                _leavingRoomReceiver);
        }

        protected void OnLeavingRoomReceived(ReadMessage a_message)
        {
            FFLog.Log(EDbgCat.RoomBroadcast, "Reading leaving room message.");
            Engine.Network.CurrentRoom.RemovePlayer(a_message.Client.NetworkID);
        }
    }
}