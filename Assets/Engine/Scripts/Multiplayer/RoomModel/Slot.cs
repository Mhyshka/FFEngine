using UnityEngine;
using System.Collections;
using System.Net;

using FF.Multiplayer;

namespace FF.Multiplayer
{
    internal class Slot : IByteStreamSerialized
	{
		#region properties
		internal FFNetworkPlayer netPlayer;
		internal int slotIndex;
		
		internal Team team;
        internal bool isPlayableSlot = true;
		#endregion
		
		public Slot()
		{
		
		}
		
		internal Slot(Team a_team, int a_index)
		{
			team = a_team;
			slotIndex = a_index;
		}
		
		internal void SetPlayer(FFNetworkPlayer a_player)
		{
			netPlayer = a_player;
			netPlayer.slot = this;
		}
		
		#region Serialization
		public void SerializeData(FFByteWriter stream)
		{
			stream.Write(netPlayer);
			stream.Write(slotIndex);
            stream.Write(isPlayableSlot);
		}
		
		public void LoadFromData(FFByteReader stream)
		{
			netPlayer = stream.TryReadObject<FFNetworkPlayer>();
			slotIndex = stream.TryReadInt();
            isPlayableSlot = stream.TryReadBool();

            if (netPlayer != null)
				netPlayer.slot = this;
		}
		#endregion
	}
}
