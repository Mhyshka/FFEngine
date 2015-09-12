using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	internal class Room 
	{
		#region properties
		public List<Player> players = new List<Player> ();
		public List<Peer> peers = new List<Peer> ();
		public List<Team> teams = new List<Team> ();
		public bool isSecondScreenActive = false;
		#endregion

		#region public methods
		public void addTeam (int totalSlots, string teamName)
		{
			Team lteam = new Team (totalSlots, teamName);
			teams.Add (lteam);
		}


		public Peer registeredPeerWithIPEndPoint (IPEndPoint ip)
		{
			foreach (Peer aPeer in peers) 
			{
				if (aPeer.ipEndPoint.Equals (ip))
				{
					return aPeer;
				}
			}

			return null;
		}


		public void addPeer (Peer peer)
		{
			peers.Add (peer);
		}


		public void removePeer (Peer peer)
		{
			peers.Remove (peer);
		}


		public bool addPlayer (Player player, Team team, Slot slot)
		{
			if (teams.Contains (team) && team.slots.Contains (slot) && slot.player != null)
			{
				players.Add (player);
				slot.player = player;
				
				return true;
			}
			
			return false;
		}


		public bool removePlayer (Player player)
		{
			foreach (Team aTeam in teams) 
			{
				foreach (Slot aSlot in aTeam.slots)
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
		#endregion
	}
}
