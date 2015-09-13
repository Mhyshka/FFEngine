using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	internal class FFRoom 
	{
		#region properties
		public string roomName;
		public List<FFPlayer> players = new List<FFPlayer> ();
		public List<FFTeam> teams = new List<FFTeam> ();
		public bool isSecondScreenActive = false;
		
		[System.NonSerialized]
		public List<FFPeer> peers = new List<FFPeer> ();
		#endregion

		#region Teams
		internal void AddTeam (int totalSlots, string teamName)
		{
			FFTeam lteam = new FFTeam (totalSlots, teamName);
			teams.Add (lteam);
		}
		#endregion

		#region Peer
		internal FFPeer RegisteredPeerWithIPEndPoint (IPEndPoint ip)
		{
			foreach (FFPeer aPeer in peers) 
			{
				if (aPeer.ipEndPoint.Equals (ip))
				{
					return aPeer;
				}
			}

			return null;
		}

		internal void AddPeer (FFPeer peer)
		{
			peers.Add (peer);
		}


		internal void RemovePeer (FFPeer peer)
		{
			peers.Remove (peer);
		}
		#endregion

		#region player
		internal bool AddPlayer (FFPlayer player, FFTeam team, FFSlot slot)
		{
			if (teams.Contains (team) && team.slots.Contains (slot) && slot.player != null)
			{
				players.Add (player);
				slot.player = player;
				
				return true;
			}
			
			return false;
		}


		internal bool RemovePlayer (FFPlayer player)
		{
			foreach (FFTeam aTeam in teams) 
			{
				foreach (FFSlot aSlot in aTeam.slots)
				{
					if (aSlot.player.Equals (player))
					{
						aSlot.player = null;

						players.Remove (player);
						
						return true;
					}
				}
			}

			return false;
		}
		
		internal int CurrentPlayerCount
		{
			get
			{
				return players.Count;
			}
		}
		
		internal int MaxPlayerAllowed
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
		#endregion
	}
}
