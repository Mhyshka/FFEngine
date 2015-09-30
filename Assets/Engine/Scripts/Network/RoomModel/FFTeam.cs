using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Networking
{
	internal struct FFSlotRef : IByteStreamSerialized
	{
		internal int teamIndex;
		internal int slotIndex;

        public FFSlotRef(int a_teamId, int a_slotId)
        {
            teamIndex = a_teamId;
            slotIndex = a_slotId;
        }
		
		public void SerializeData(FFByteWriter stream)
		{
			stream.Write(teamIndex);
			stream.Write(slotIndex);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
			teamIndex = stream.TryReadInt();
			slotIndex = stream.TryReadInt();
		}
		
		public override string ToString ()
		{
			return string.Format("Team Ind : {0} / Slot Ind : {1}", teamIndex.ToString(), slotIndex.ToString());
		}
	}
	
	internal class FFTeam : IByteStreamSerialized
	{
		#region properties
		internal string teamName;
		internal int teamIndex;
		protected List<FFSlot> _slots;
		
		internal List<FFSlot> Slots
		{
			get
			{
				return _slots;
			}
		}
		
		public int TotalSlots
		{
			get
			{
				return _slots.Count;
			}
		}
		
		internal int TotalPlayers
		{
			get
			{
				int count = 0;
				foreach(FFSlot aSlot in _slots)
				{
					if(aSlot.netPlayer != null)
					{
						count++;
					}
				}
				
				return count;
			}
		}
		
		internal int SlotsLeft
		{
			get
			{
				return TotalSlots - TotalPlayers;
			}
		}
		
		internal bool IsFull
		{
			get
			{
				return SlotsLeft == 0;
			}
		}
		#endregion

		#region Constructor
		public FFTeam()
		{
		
		}
		
		internal FFTeam (string aTeamName, int aTotalSlots)
		{	
			teamName = aTeamName;
			
			_slots = new List<FFSlot> ();
			for (int i = 0; i < aTotalSlots; i++)
			{
				FFSlot aSlot = new FFSlot (this, i);
				_slots.Add (aSlot);
			}
		}
		#endregion
		
		internal FFSlot NextAvailableSlot()
		{
			foreach(FFSlot each in _slots)
			{
				if(each.netPlayer == null)
					return each;
			}
			return null;
		}
		
		#region Serialization
		public void SerializeData(FFByteWriter stream)
		{
			stream.Write(teamName);
			stream.Write(teamIndex);
			stream.Write(_slots);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
			teamName = stream.TryReadString();
			teamIndex = stream.TryReadInt();
			_slots = stream.TryReadObjectList<FFSlot>();
			
			foreach(FFSlot each in _slots)
			{
				each.team = this;
			}
		}
		#endregion
	}
}

