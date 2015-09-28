using UnityEngine;
using System.Collections;
using System.Net;


namespace FF.Networking
{	
	internal class FFNetworkPlayer : FFPeer
	{
		#region properties
		internal FFPlayer player = null;
		internal bool isHost = false;
		internal bool useTV = false;
        internal bool isDCed = false;
		
		internal FFSlot slot = null;
		
		internal FFSlotRef SlotRef
		{
			get
			{
				FFSlotRef slotRef = new FFSlotRef();
				slotRef.slotIndex = slot.slotIndex;
				slotRef.teamIndex = slot.team.teamIndex;
				return slotRef;
			}
		}
		#endregion

		#region public methods
		public FFNetworkPlayer() : base()
		{
		
		}
		
		internal FFNetworkPlayer (IPEndPoint a_ep, FFPlayer a_player) : base(a_ep)
		{
			player = a_player;
		}
		#endregion
		
		#region Serialization
		public override void SerializeData(FFByteWriter stream)
		{
			stream.Write(player);
			stream.Write(isHost);
			stream.Write(useTV);
            stream.Write(isDCed);
		}
		
		public override void LoadFromData(FFByteReader stream)
		{
			player = stream.TryReadObject<FFPlayer>();
			isHost = stream.TryReadBool();
			useTV = stream.TryReadBool();
            isDCed = stream.TryReadBool();
		}
		#endregion
	}
}
