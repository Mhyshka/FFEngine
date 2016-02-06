using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;


namespace FF.Multiplayer
{
    internal struct SlotRef : IByteStreamSerialized
    {
        internal int teamIndex;
        internal int slotIndex;

        public SlotRef(int a_teamId, int a_slotId)
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

        public override string ToString()
        {
            return string.Format("Team Ind : {0} / Slot Ind : {1}", teamIndex.ToString(), slotIndex.ToString());
        }
    }

    internal class Team : IByteStreamSerialized
	{
        #region properties
        internal string teamName;
		internal int teamIndex;
        protected int _minimumPlayersToStart;
        protected List<Slot> _slots;
		
		internal List<Slot> Slots
		{
			get
			{
				return _slots;
			}
		}

        internal int TotalPlayableSlots
        {
            get
            {
                int count = 0;
                foreach (Slot aSlot in _slots)
                {
                    if (aSlot.isPlayableSlot)
                    {
                        count++;
                    }
                }

                return count;
            }
        }
		
		internal int TotalSlots
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
				foreach(Slot aSlot in _slots)
				{
					if(aSlot.isPlayableSlot && aSlot.netPlayer != null)
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

        internal List<FFNetworkPlayer> Players
        {
            get
            {
                List<FFNetworkPlayer> players = new List<FFNetworkPlayer>();
                foreach (Slot each in _slots)
                {
                    if (each.netPlayer != null)
                        players.Add(each.netPlayer);
                }
                return players;
            }
        }
		#endregion

		#region Constructor
		public Team()
		{
		
		}
		
		internal Team (string aTeamName, int aTotalSlots, int a_minimumPlayersToStart)
		{
            _minimumPlayersToStart = a_minimumPlayersToStart;
            teamName = aTeamName;
			
			_slots = new List<Slot> ();
			for (int i = 0; i < aTotalSlots; i++)
			{
				Slot aSlot = new Slot (this, i);
				_slots.Add (aSlot);
			}
		}

        internal bool CanStart
        {
            get
            {
                return TotalPlayers >= _minimumPlayersToStart;
            }
        }
		#endregion
		
		internal Slot NextAvailableSlot()
		{
			foreach(Slot each in _slots)
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
            _slots = stream.TryReadObjectList<Slot>();
			
			foreach(Slot each in _slots)
			{
				each.team = this;
			}
		}
		#endregion
	}
}

