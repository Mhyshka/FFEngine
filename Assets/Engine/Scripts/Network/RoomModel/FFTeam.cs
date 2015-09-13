using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Networking
{	
	[System.Serializable]
	internal class FFTeam 
	{
		#region properties
		public List<FFSlot> slots = new List<FFSlot> ();
		public int TotalSlots
		{
			get
			{
				return slots.Count;
			}
		}
		
		public string teamName;
		#endregion

		#region Constructor
		internal FFTeam (int aTotalSlots, string aTeamName)
		{	
			for (int i = 0; i < aTotalSlots; i++)
			{
				FFSlot aSlot = new FFSlot ();
				slots.Add (aSlot);
			}
	
			teamName = aTeamName;
		}
		#endregion
	}
}

