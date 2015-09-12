using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	public class Team 
	{
		#region properties
		public List<Slot> slots = new List<Slot> ();
		public string teamName;
		public int totalSlots;
		#endregion

		#region public methods
		public Team (int aTotalSlots, string aTeamName)
		{
			totalSlots = aTotalSlots;
			
			for (int i = 0; i < totalSlots; i++)
			{
				Slot aSlot = new Slot ();
				slots.Add (aSlot);
			}
	
			teamName = aTeamName;
		}
		#endregion
	}
}

