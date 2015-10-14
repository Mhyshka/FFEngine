using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;

namespace FF.Networking
{	
	internal delegate void FFRoomCallback(FFRoom a_room);
	
	internal class FFRoom : IByteStreamSerialized
	{
		#region Properties
		internal string roomName;
		internal List<FFTeam> teams;
		internal bool IsSecondScreenActive
		{
			get
			{
				return false;
			}
		}
		
		internal IPEndPoint serverEndPoint;
		
		internal FFRoomCallback onRoomUpdated = null;

        internal Dictionary<int, FFNetworkPlayer> players = null;

        protected HashSet<int> _bannedId = null;
		#endregion
		
		public FFRoom()
		{
			teams = new List<FFTeam> ();
            players = new Dictionary<int, FFNetworkPlayer>();
            _bannedId = new HashSet<int>();

            if (FFEngine.Network.IsServer)
            {
                FFEngine.Network.Server.onClientLost += OnClientConnectionLost;
                FFEngine.Network.Server.onClientReconnection += OnClientReconnection;
                FFMessageNetworkID.onNetworkIdReceived += OnNetworkIdReceived;
            } 
        }

        internal void TearDown()
        {
            onRoomUpdated = null;
            if (FFEngine.Network.IsServer)
            {
                FFEngine.Network.Server.onClientLost -= OnClientConnectionLost;
                FFEngine.Network.Server.onClientReconnection -= OnClientReconnection;
                FFMessageNetworkID.onNetworkIdReceived -= OnNetworkIdReceived;
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

        internal FFNetworkPlayer GetPlayerForSlot(FFSlotRef a_ref)
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
        internal void SetPlayer(FFSlotRef a_slotRef, FFNetworkPlayer a_player)
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
        }
        #endregion

        #region Removing Player
        internal void RemovePlayer (int a_teamIndex, int a_slotIndex)
		{
            RemovePlayer(new FFSlotRef(a_teamIndex, a_slotIndex));
		}

        internal void RemovePlayer(FFSlotRef a_slotRef)
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

                FFLog.LogError("Removing player : " + player.ID.ToString());

                if (onRoomUpdated != null)
                    onRoomUpdated(this);
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Couldn't remove player for ID : " + a_ID);
            }
        }
        #endregion

        #region Moving Player
        internal void MovePlayer(FFSlotRef from, FFSlotRef to)
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

        internal void SwapPlayers(FFSlotRef a_firstPlayer, FFSlotRef a_secondPlayer)
        {
            FFSlot firstSlot = GetSlotForRef(a_firstPlayer);
            FFSlot secondSlot = GetSlotForRef(a_secondPlayer);
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
                FFSlot firstSlot = firstPlayer.slot;
                FFSlot secondSlot = secondPlayer.slot;

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
        internal void AddTeam(FFTeam a_teamToAdd)
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
		
		internal int TotalPlayers
		{
			get
			{
				int count = 0;
				foreach (FFTeam aTeam in teams) 
				{
					count += aTeam.TotalPlayers;
				}
				
				return count;
			}
		}
		
		internal int TotalSlots
		{
			get
			{
				int count = 0;
				foreach(FFTeam each in teams)
				{
					count += each.TotalSlots;
				}
				return count;
			}
		}
		
		internal virtual FFSlot NextAvailableSlot()
		{
			FFSlot slot = null;
			foreach(FFTeam aTeam in teams)
			{
				if(!aTeam.IsFull)
				{
					slot = aTeam.NextAvailableSlot();
					break;
				}
			}
			return slot;	
		}

        internal FFSlot GetSlotForRef(FFSlotRef a_ref)
        {
            return teams[a_ref.teamIndex].Slots[a_ref.slotIndex];
        }
		#endregion
		
		internal void UpdateWithRoom(FFRoom a_room)
		{
			teams = a_room.teams;
            UpdatePlayers();

            if (onRoomUpdated != null)
				onRoomUpdated(this);
		}

        internal void UpdatePlayers()
        {
            players.Clear();
            foreach (FFTeam aTeam in teams)
            {
                foreach (FFSlot aSlot in aTeam.Slots)
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
			teams = stream.TryReadObjectList<FFTeam>();
		}
        #endregion

        #region Client Callbacks
        void OnClientReconnection(FFTcpClient a_client)
        {
            FFLog.Log(EDbgCat.Networking, "OnClientReconnection : " + a_client.NetworkID.ToString());
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_client.NetworkID, out player))
            {
                player.isDCed = false;
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
                player.isDCed = true;
                if(onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }

        void OnNetworkIdReceived(FFTcpClient a_client)
        {
            FFLog.Log(EDbgCat.Networking, "NetworkIdReceived : " + a_client.NetworkID.ToString());

            if (players.ContainsKey(a_client.NetworkID))
            {
                RemovePlayer(a_client.NetworkID);
            }
        }
        #endregion
    }
}
