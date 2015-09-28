using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	internal class FFSlot : IByteStreamSerialized
	{
		#region properties
		internal FFNetworkPlayer netPlayer;
		internal int slotIndex;
		
		internal FFTeam team;
		#endregion
		
		public FFSlot()
		{
		
		}
		
		internal FFSlot(FFTeam a_team, int a_index)
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
		}
		
		public void LoadFromData(FFByteReader stream)
		{
			netPlayer = stream.TryReadObject<FFNetworkPlayer>();
			slotIndex = stream.TryReadInt();
			
			if(netPlayer != null)
				netPlayer.slot = this;
		}
		#endregion
	}
}
