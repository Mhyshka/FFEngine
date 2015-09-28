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
		#region properties
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

        internal Dictionary<IPEndPoint, FFNetworkPlayer> players = null;
		#endregion
		
		public FFRoom()
		{
			teams = new List<FFTeam> ();
            players = new Dictionary<IPEndPoint, FFNetworkPlayer>();

            if (FFEngine.Network.Server != null)
            {
                FFEngine.Network.Server.onClientLost += OnClientConnectionLost;
                FFEngine.Network.Server.onClientReconnection += OnClientReconnection;
            }
        }

        ~FFRoom()
        {
            if (FFEngine.Network.Server != null)
            {
                FFEngine.Network.Server.onClientLost -= OnClientConnectionLost;
                FFEngine.Network.Server.onClientReconnection -= OnClientReconnection;
            }
        }

		#region Peer Management
		#endregion

		#region Player Management
		internal FFNetworkPlayer GetPlayerForEndpoint(IPEndPoint a_endpoint)
		{
            FFNetworkPlayer player = null;
            if (!players.TryGetValue(a_endpoint, out player))
            {
                FFLog.LogWarning(EDbgCat.Networking, "Couldn't found player in room for EP : " + a_endpoint);
            }
            return player;
		}

        internal FFNetworkPlayer GetPlayerForSlot(FFSlotRef a_ref)
        {
            return teams[a_ref.teamIndex].Slots[a_ref.slotIndex].netPlayer;
        }

        internal void SetPlayer (int a_teamIndex, int a_slotIndex, FFNetworkPlayer a_player)
		{
            FFLog.Log(EDbgCat.Networking, "Adding player to room : " + a_player.ipEndPoint.ToString());
            players.Add(a_player.ipEndPoint, a_player);

            teams[a_teamIndex].Slots[a_slotIndex].SetPlayer(a_player);
            if (onRoomUpdated != null)
				onRoomUpdated(this);
        }

        internal void SetPlayer(FFSlotRef a_slotRef, FFNetworkPlayer a_player)
        {
            SetPlayer(a_slotRef.teamIndex, a_slotRef.slotIndex, a_player);
        }

        internal void RemovePlayer (int a_teamIndex, int a_slotIndex)
		{


            if (teams[a_teamIndex].Slots[a_slotIndex].netPlayer != null)
            {
                FFLog.LogError("Removing player : " + teams[a_teamIndex].Slots[a_slotIndex].netPlayer.ipEndPoint.ToString());
                players.Remove(teams[a_teamIndex].Slots[a_slotIndex].netPlayer.ipEndPoint);
            }

			teams[a_teamIndex].Slots[a_slotIndex].netPlayer = null;
			if(onRoomUpdated != null)
				onRoomUpdated(this);
		}

        internal void RemovePlayer(FFSlotRef a_slotRef)
        {
            RemovePlayer(a_slotRef.teamIndex, a_slotRef.slotIndex);
        }

        internal void MovePlayer(FFSlotRef from, FFSlotRef to)
		{
			if(teams[to.teamIndex].Slots[to.slotIndex].netPlayer == null)
			{
				if(teams[from.teamIndex].Slots[from.slotIndex].netPlayer != null)
				{
					FFNetworkPlayer player = teams[from.teamIndex].Slots[from.slotIndex].netPlayer;
					teams[to.teamIndex].Slots[to.slotIndex].SetPlayer(player);
					teams[from.teamIndex].Slots[from.slotIndex].netPlayer = null;
					if(onRoomUpdated != null)
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
		#endregion
		
		internal void UpdateWithRoom(FFRoom a_room)
		{
			teams = a_room.teams;
			
			if(onRoomUpdated != null)
				onRoomUpdated(this);
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

        void OnClientReconnection(FFTcpClient a_client)
        {
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_client.Remote, out player))
            {
                player.isDCed = false;
                if (onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }

        void OnClientConnectionLost(FFTcpClient a_client)
        {
            FFNetworkPlayer player = null;
            if (players.TryGetValue(a_client.Remote, out player))
            {
                player.isDCed = true;
                if(onRoomUpdated != null)
                    onRoomUpdated(this);
            }
        }
	}
}
