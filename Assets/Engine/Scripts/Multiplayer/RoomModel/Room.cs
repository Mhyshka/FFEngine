using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

using FF.Network;
using FF.Network.Message;

namespace FF.Multiplayer
{	
	internal delegate void RoomCallback(Room a_room);
    internal delegate void RoomPlayerCallback(Room a_room, FFNetworkPlayer a_player);

    internal class Room : IByteStreamSerialized
	{
		#region Properties
		internal string roomName;
		internal List<Team> teams;
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

        internal Dictionary<int, FFNetworkPlayer> players = null;

        protected HashSet<int> _bannedId = null;
		#endregion
		
		public Room()
		{
			teams = new List<Team> ();
            players = new Dictionary<int, FFNetworkPlayer>();
            _bannedId = new HashSet<int>();

            if (Engine.Network.IsServer)
            {
                Engine.Network.Server.onClientLost += OnClientConnectionLost;
                Engine.Network.Server.onClientReconnection += OnClientReconnection;
            } 
        }

        internal void TearDown()
        {
            onRoomUpdated = null;
            onPlayerRemoved = null;
            onPlayerAdded = null;

            if (Engine.Network.IsServer)
            {
                Engine.Network.Server.onClientLost -= OnClientConnectionLost;
                Engine.Network.Server.onClientReconnection -= OnClientReconnection;
            } 
        }

		#region Accessing Player
		internal FFNetworkPlayer GetPlayerForEndpoint(IPEndPoint a_endpoint)
		{
            FFNetworkPlayer player = null;
            foreach (FFNetworkPlayer each in players.Values)
            {
                if (each.IpEndPoint == a_endpoint)
                {
                    player = each;
                    break;
                }
            }

            if(player == null)
                FFLog.LogWarning(EDbgCat.Networking, "Couldn't found player in room for EP : " + a_endpoint);

            return player;
		}

        internal FFNetworkPlayer GetPlayerForId(int a_networkId)
        {
            FFNetworkPlayer player = null;
            if (!players.TryGetValue(a_networkId, out player))
            {
                FFLog.LogWarning(EDbgCat.Networking, "Couldn't found player in room for ID : " + a_networkId);
            }

            return player;
        }

        internal FFNetworkPlayer GetPlayerForSlot(SlotRef a_ref)
        {
            return GetSlotForRef(a_ref).netPlayer;
        }

        internal bool IsBanned(int a_id)
        {
            return _bannedId.Contains(a_id);
        }

        internal void BanId(int a_id)
        {
            _bannedId.Add(a_id);
        }

        internal void UnbanId(int a_id)
        {
            _bannedId.Remove(a_id);
        }
        #endregion

        #region Add Player
        internal void SetPlayer(SlotRef a_slotRef, FFNetworkPlayer a_player)
        {
            SetPlayer(a_slotRef.teamIndex, a_slotRef.slotIndex, a_player);
        }

        internal void SetPlayer (int a_teamIndex, int a_slotIndex, FFNetworkPlayer a_player)
		{
            FFLog.Log(EDbgCat.Networking, "Adding player to room : " + a_player.IpEndPoint.ToString() + " id : " + a_player.ID.ToString());
            players.Add(a_player.ID, a_player);

            teams[a_teamIndex].Slots[a_slotIndex].SetPlayer(a_player);
            if (onRoomUpdated != null)
				onRoomUpdated(this);

            if (onPlayerAdded != null)
                onPlayerAdded(this, a_player);
        }
        #endregion

        #region Removing Player
        internal void RemovePlayer (int a_teamIndex, int a_slotIndex)
		{
            RemovePlayer(new SlotRef(a_teamIndex, a_slotIndex));
		}

        internal void RemovePlayer(SlotRef a_slotRef)
        {
            FFNetworkPlayer player = GetPlayerForSlot(a_slotRef);
            if (player != null)
                RemovePlayer(player.ID);
            else
                FFLog.LogError("No player in slot : " + a_slotRef.ToString());
        }

        internal void RemovePlayer(int a_ID)
        {
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_ID, out player))
            {
                players.Remove(player.ID);
                player.slot.netPlayer = null;

                FFLog.Log("Removing player : " + player.ID.ToString());

                if (onRoomUpdated != null)
                    onRoomUpdated(this);

                if (onPlayerRemoved != null)
                    onPlayerRemoved(this, player);
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Couldn't remove player for ID : " + a_ID);
            }
        }
        #endregion

        #region Moving Player
        internal void MovePlayer(SlotRef from, SlotRef to)
		{
			if(teams[to.teamIndex].Slots[to.slotIndex].netPlayer == null)
			{
				if(teams[from.teamIndex].Slots[from.slotIndex].netPlayer != null)
				{
                    FFNetworkPlayer player = teams[from.teamIndex].Slots[from.slotIndex].netPlayer;
                    GetSlotForRef(to).SetPlayer(player);
                    GetSlotForRef(from).netPlayer = null;
					if(onRoomUpdated != null)
						onRoomUpdated(this);
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

                if (onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }

        internal void SwapPlayers(int a_firstPlayer, int a_secondPlayer)
        {
            FFNetworkPlayer firstPlayer = GetPlayerForId(a_firstPlayer);
            FFNetworkPlayer secondPlayer = GetPlayerForId(a_secondPlayer);
            if (firstPlayer != null && secondPlayer != null)
            {
                Slot firstSlot = firstPlayer.slot;
                Slot secondSlot = secondPlayer.slot;

                if (firstSlot.netPlayer != null && secondSlot.netPlayer != null)
                {
                    firstSlot.SetPlayer(firstPlayer);
                    secondSlot.SetPlayer(secondPlayer);

                    if (onRoomUpdated != null)
                        onRoomUpdated(this);
                }
            }
        }
        #endregion

        #region Teams Management
        internal void AddTeam(Team a_teamToAdd)
		{
			teams.Add(a_teamToAdd);
			a_teamToAdd.teamIndex = teams.Count-1;
		}
		#endregion
		
		#region Slots Properties
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
				foreach(Team each in teams)
				{
					count += each.TotalSlots;
				}
				return count;
			}
		}
		
		internal virtual Slot NextAvailableSlot()
		{
			Slot slot = null;
			foreach(Team aTeam in teams)
			{
				if(!aTeam.IsFull)
				{
					slot = aTeam.NextAvailableSlot();
					break;
				}
			}
			return slot;	
		}

        internal Slot GetSlotForRef(SlotRef a_ref)
        {
            return teams[a_ref.teamIndex].Slots[a_ref.slotIndex];
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

        internal int TotalSpectatorPlayers
        {
            get
            {
                int count = 0;
                foreach (Team aTeam in teams)
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

        #region Players

        internal int TotalPlayers
        {
            get
            {
                int count = 0;
                foreach (Team aTeam in teams)
                {
                    count += aTeam.TotalPlayers;
                }

                return count;
            }
        }

        internal int TotalPlayableSlots
        {
            get
            {
                int count = 0;
                foreach (Team each in teams)
                {
                    count += each.TotalPlayableSlots;
                }
                return count;
            }
        }

        internal List<FFNetworkPlayer> DcedPlayers
        {
            get
            {
                List<FFNetworkPlayer> players = new List<FFNetworkPlayer>();
                foreach (Team aTeam in teams)
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
        #endregion

        internal bool CanStart
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                bool canStart = true;
                foreach (Team each in teams)
                {
                    canStart = canStart && each.CanStart;
                }
                canStart = canStart && DcedPlayers.Count == 0;
                return canStart;
#endif
            }
        }

        internal void UpdateWithRoom(Room a_room)
		{
			teams = a_room.teams;
            UpdatePlayers();

            if (onRoomUpdated != null)
				onRoomUpdated(this);
		}

        internal void UpdatePlayers()
        {
            players.Clear();
            foreach (Team aTeam in teams)
            {
                foreach (Slot aSlot in aTeam.Slots)
                {
                    FFNetworkPlayer curPlayer = aSlot.netPlayer;
                    if (curPlayer != null)
                    {
                        players.Add(curPlayer.ID, curPlayer);
                    }
                }
            }
        }
		
#region Serialization
		public void SerializeData(FFByteWriter stream)
		{
			stream.Write(roomName);
			stream.Write(teams);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
            roomName = stream.TryReadString();
            teams = stream.TryReadObjectList<Team>();

		}
#endregion

#region Client Callbacks
        void OnClientReconnection(FFTcpClient a_client)
        {
            FFLog.Log(EDbgCat.Networking, "OnClientReconnection : " + a_client.NetworkID.ToString());
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_client.NetworkID, out player))
            {
                player.isDced = false;
                if (onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }

        void OnClientConnectionLost(FFTcpClient a_client)
        {
            FFLog.Log(EDbgCat.Networking, "OnClientConnectionLost : " + a_client.NetworkID.ToString());
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_client.NetworkID, out player))
            {
                player.isDced = true;
                if(onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }

        internal void OnNetworkIdReceived(FFTcpClient a_client)
        {
            if (players.ContainsKey(a_client.NetworkID))
            {
                RemovePlayer(a_client.NetworkID);
            }
        }
#endregion
    }
}
