using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

using FF.Network;
using FF.Handler;
using FF.Network.Message;

namespace FF.Multiplayer
{	
	internal delegate void RoomCallback(Room a_room);
    internal delegate void RoomPlayerCallback(Room a_room, FFNetworkPlayer a_player);

    internal class Room : IByteStreamSerialized
	{
		#region Properties
		internal string roomName;
		internal bool IsSecondScreenActive
		{
			get
			{
				return false;
			}
		}
		
		internal IPEndPoint serverEndPoint;
		
		internal RoomCallback onRoomUpdated = null;
        internal RoomPlayerCallback onPlayerRemoved = null;
        internal RoomPlayerCallback onPlayerAdded = null;


        protected HashSet<IPAddress> _bannedIps = null;
		#endregion
		
		public Room()
		{
			_teams = new List<Team> ();
            _players = new Dictionary<int, FFNetworkPlayer>();
            _bannedIps = new HashSet<IPAddress>();

            _dcedPlayerIds = new HashSet<int>();

            if (Engine.Network.IsServer)
            {
                Engine.Network.GameServer.onClientConnectionLost += OnClientConnectionLost;
                Engine.Network.GameServer.onClientReconnected += OnClientReconnection;
                Engine.Network.GameServer.onClientRemoved += OnClientRemoved;
            } 
        }

        internal void TearDown()
        {
            onRoomUpdated = null;
            onPlayerRemoved = null;
            onPlayerAdded = null;

            if (Engine.Network.IsServer)
            {
                Engine.Network.GameServer.onClientConnectionLost -= OnClientConnectionLost;
                Engine.Network.GameServer.onClientReconnected -= OnClientReconnection;
                Engine.Network.GameServer.onClientRemoved -= OnClientRemoved;
            } 
        }

        #region Teams
        protected List<Team> _teams;

        internal void AddTeam(Team a_teamToAdd)
        {
            _teams.Add(a_teamToAdd);
            a_teamToAdd.teamIndex = _teams.Count - 1;
        }

        internal List<Team> Teams
        {
            get
            {
                return _teams;
            }
        }
        #endregion


        #region Slots
        #region Properties
        internal bool IsFull
        {
            get
            {
                return SlotsLeft == 0;
            }
        }

        internal int SlotsLeft
        {
            get
            {
                return TotalSlots - TotalPlayers;
            }
        }

        internal int TotalSlots
        {
            get
            {
                int count = 0;
                foreach (Team each in _teams)
                {
                    count += each.TotalSlots;
                }
                return count;
            }
        }

        internal virtual Slot NextAvailableSlot()
        {
            Slot slot = null;
            foreach (Team aTeam in _teams)
            {
                if (!aTeam.IsFull)
                {
                    slot = aTeam.NextAvailableSlot();
                    break;
                }
            }
            return slot;
        }

        internal Slot GetSlotForRef(SlotRef a_ref)
        {
            return _teams[a_ref.teamIndex].Slots[a_ref.slotIndex];
        }
        #endregion

        #region Spectators
        internal int TotalSpectatorSlots
        {
            get
            {
                return TotalSlots - TotalPlayableSlots;
            }
        }
        #endregion

        #region Players
        internal int TotalPlayableSlots
        {
            get
            {
                int count = 0;
                foreach (Team each in _teams)
                {
                    count += each.TotalPlayableSlots;
                }
                return count;
            }
        }
        #endregion
        #endregion

        #region Players
        protected Dictionary<int, FFNetworkPlayer> _players = null;
        internal Dictionary<int, FFNetworkPlayer> Players
        {
            get
            {
                return _players;
            }
        }

        #region Getters
        internal List<int> GetPlayersIds(bool a_includeSpectators = true)
        {
            List<int> ids = new List<int>();
            foreach (FFNetworkPlayer each in _players.Values)
            {
                if (each.slot.isPlayableSlot || a_includeSpectators)
                    ids.Add(each.ID);
            }
            return ids;
        }

        internal FFNetworkPlayer PlayerForId(int a_networkId)
        {
            FFNetworkPlayer player = null;
            if (!_players.TryGetValue(a_networkId, out player))
            {
                FFLog.LogWarning(EDbgCat.Room, "Couldn't found player in room for ID : " + a_networkId);
            }

            return player;
        }

        internal List<FFNetworkPlayer> DcedPlayers
        {
            get
            {
                List<FFNetworkPlayer> players = new List<FFNetworkPlayer>();
                foreach (Team aTeam in _teams)
                {
                    foreach (Slot aSlot in aTeam.Slots)
                    {
                        if (aSlot.netPlayer != null && aSlot.netPlayer.isDced)
                        {
                            players.Add(aSlot.netPlayer);
                        }
                    }
                }
                return players;
            }
        }

        internal FFNetworkPlayer GetPlayerForSlot(SlotRef a_ref)
        {
            return GetSlotForRef(a_ref).netPlayer;
        }

        internal int TotalPlayers
        {
            get
            {
                int count = 0;
                foreach (Team aTeam in _teams)
                {
                    count += aTeam.TotalPlayers;
                }

                return count;
            }
        }

        internal int TotalSpectatorPlayers
        {
            get
            {
                int count = 0;
                foreach (Team aTeam in _teams)
                {
                    foreach (Slot aSlot in aTeam.Slots)
                    {
                        if (!aSlot.isPlayableSlot && aSlot.netPlayer != null)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }
        #endregion

        #region Add
        internal void SetPlayer(SlotRef a_slotRef, FFNetworkPlayer a_player)
        {
            SetPlayer(a_slotRef.teamIndex, a_slotRef.slotIndex, a_player);
        }

        internal void SetPlayer(int a_teamIndex, int a_slotIndex, FFNetworkPlayer a_player)
        {
            FFLog.Log(EDbgCat.Room, "Adding player to room - id : " + a_player.ID.ToString());
            _players.Add(a_player.ID, a_player);

            _teams[a_teamIndex].Slots[a_slotIndex].SetPlayer(a_player);

            UpdateRoom();

            if (onPlayerAdded != null)
                onPlayerAdded(this, a_player);
        }
        #endregion

        #region Removing Player
        internal void RemovePlayer(int a_teamIndex, int a_slotIndex)
        {
            RemovePlayer(new SlotRef(a_teamIndex, a_slotIndex));
        }

        internal void RemovePlayer(SlotRef a_slotRef)
        {
            FFNetworkPlayer player = GetPlayerForSlot(a_slotRef);
            if (player != null)
                RemovePlayer(player.ID);
            else
                FFLog.LogError(EDbgCat.Room, "No player in slot : " + a_slotRef.ToString());
        }

        internal void RemovePlayer(int a_ID, bool a_disconnectClient = true)
        {
            FFNetworkPlayer player = null;
            if (_players.TryGetValue(a_ID, out player))
            {
                _players.Remove(player.ID);
                player.slot.netPlayer = null;

                FFLog.Log(EDbgCat.Room, "Removing player : " + player.ID.ToString());

                if (a_disconnectClient)
                    Engine.Network.GameServer.RemoveAndDisconnectClient(a_ID);

                UpdateRoom();

                if (onPlayerRemoved != null)
                    onPlayerRemoved(this, player);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Room, "Couldn't remove player for ID : " + a_ID);
            }
        }
        #endregion

        #region Moving Player
        internal void MovePlayer(SlotRef from, SlotRef to)
        {
            if (_teams[to.teamIndex].Slots[to.slotIndex].netPlayer == null)
            {
                if (_teams[from.teamIndex].Slots[from.slotIndex].netPlayer != null)
                {
                    FFNetworkPlayer player = _teams[from.teamIndex].Slots[from.slotIndex].netPlayer;
                    GetSlotForRef(to).SetPlayer(player);
                    GetSlotForRef(from).netPlayer = null;
                    UpdateRoom();
                }

            }
        }

        internal void SwapPlayers(SlotRef a_firstPlayer, SlotRef a_secondPlayer)
        {
            Slot firstSlot = GetSlotForRef(a_firstPlayer);
            Slot secondSlot = GetSlotForRef(a_secondPlayer);
            if (firstSlot.netPlayer != null && secondSlot.netPlayer != null)
            {
                FFNetworkPlayer tmp = firstSlot.netPlayer;
                firstSlot.SetPlayer(secondSlot.netPlayer);
                secondSlot.SetPlayer(tmp);

                UpdateRoom();
            }
        }

        internal void SwapPlayers(int a_firstPlayer, int a_secondPlayer)
        {
            FFNetworkPlayer firstPlayer = PlayerForId(a_firstPlayer);
            FFNetworkPlayer secondPlayer = PlayerForId(a_secondPlayer);
            if (firstPlayer != null && secondPlayer != null)
            {
                Slot firstSlot = firstPlayer.slot;
                Slot secondSlot = secondPlayer.slot;

                if (firstSlot.netPlayer != null && secondSlot.netPlayer != null)
                {
                    firstSlot.SetPlayer(firstPlayer);
                    secondSlot.SetPlayer(secondPlayer);

                    UpdateRoom();
                }
            }
        }
        #endregion
        #endregion
       

        #region Ban
        internal bool IsBanned(IPAddress a_ip)
        {
            return _bannedIps.Contains(a_ip);
        }

        internal void BanIP(IPAddress a_ip)
        {
            _bannedIps.Add(a_ip);
        }

        internal void UnbanId(IPAddress a_ip)
        {
            _bannedIps.Remove(a_ip);
        }
        #endregion

        internal bool CanStart
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                bool canStart = true;
                foreach (Team each in Teams)
                {
                    canStart = canStart && each.CanStart;
                }
                canStart = canStart && DcedPlayers.Count == 0;
                return canStart;
#endif
            }
        }

        #region Serialization
        public void SerializeData(FFByteWriter stream)
		{
			stream.Write(roomName);
			stream.Write(_teams);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
            roomName = stream.TryReadString();
            _teams = stream.TryReadObjectList<Team>();
		}

        internal void UpdateWithRoom(Room a_room)
        {
            _teams = a_room._teams;
            UpdatePlayers();

            UpdateRoom();
        }

        internal void UpdatePlayers()
        {
            _players.Clear();
            foreach (Team aTeam in _teams)
            {
                foreach (Slot aSlot in aTeam.Slots)
                {
                    FFNetworkPlayer curPlayer = aSlot.netPlayer;
                    if (curPlayer != null)
                    {
                        _players.Add(curPlayer.ID, curPlayer);
                    }
                }
            }
        }
        #endregion

        #region GameServer Callbacks
        void OnClientReconnection(int a_id)
        {
            FFLog.Log(EDbgCat.Room, "OnClientReconnection : " + a_id.ToString());
            FFNetworkPlayer player = null;
            if (_players.TryGetValue(a_id, out player))
            {
                player.isDced = false;

                UpdateRoom();
            }
        }

        void OnClientConnectionLost(int a_id)
        {
            FFLog.Log(EDbgCat.Room, "OnClientConnectionLost : " + a_id.ToString());
            FFNetworkPlayer player = null;
            if (_players.TryGetValue(a_id, out player))
            {
                player.isDced = true;

                UpdateRoom();
            }
        }

        void OnClientRemoved(int a_id)
        {
            RemovePlayer(a_id);
        }

        protected void UpdateRoom(bool a_broadcast = true)
        {
            if (onRoomUpdated != null)
                onRoomUpdated(this);

            if (a_broadcast && Engine.Network.IsServer)
            {
                MessageRoomData roomInfo = new MessageRoomData(this);
                SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.GameServer.GetClientsIds(),
                                                                        roomInfo,
                                                                        EMessageChannel.RoomInfos.ToString());
                message.Broadcast();
            }
        }
        #endregion

        #region In-Game Deconnection Handling
        protected HashSet<int> _dcedPlayerIds = null; 
        internal void EnableHandleDeconnectionOnly()
        {
            Engine.Network.GameServer.onClientConnectionLost += OnInGameDeconnection;
            Engine.Network.GameServer.onClientReconnected += OnInGameReconnection;
            Engine.Network.GameServer.EnableReconnectionOnlyMode();
        }

        internal void DisableHandleDeconnectionOnly()
        {
            Engine.Network.GameServer.onClientConnectionLost -= OnInGameDeconnection;
            Engine.Network.GameServer.onClientReconnected -= OnInGameReconnection;
            Engine.Network.GameServer.DisableReconnectionOnlyMode();
        }

        protected void OnInGameDeconnection(int a_id)
        {
            _dcedPlayerIds.Add(a_id);
            Engine.Network.TcpServer.ResumeAcceptingConnections();
        }

        protected void OnInGameReconnection(int a_id)
        {
            _dcedPlayerIds.Remove(a_id);

            if (_dcedPlayerIds.Count == 0)
            {
                Engine.Network.TcpServer.PauseAcceptingConnections();
            }
        }
        #endregion
    }
}
