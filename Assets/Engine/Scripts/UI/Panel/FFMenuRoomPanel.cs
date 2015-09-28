using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using FF.Networking;

namespace FF.UI
{
	[System.Serializable]
	internal struct UITeamRef
	{
        public Text teamNameLabel;
		public FFSlotWidget[] slots;
	}
	
	internal class FFMenuRoomPanel : FFPanel
	{
		public Text roomNameLabel = null;
		public UITeamRef[] teams = null;
		
		protected override void Awake ()
		{
			base.Awake ();
			for(int i = 0 ; i < teams.Length ; i++)
			{
				for(int j = 0 ; j < teams[i].slots.Length ; j++)
				{
					FFSlotRef data = new FFSlotRef();
					data.teamIndex = i;
					data.slotIndex = j;
					teams[i].slots[j].button.Data = data;
				}
			}
		}
		
		internal void SetSlotPlayer(int teamIndex, int slotIndex, FFNetworkPlayer a_player)
		{
			teams[teamIndex].slots[slotIndex].SetPlayer(a_player);
		}
		
		internal void SetSlotEmpty(int teamIndex, int slotIndex)
		{
			teams[teamIndex].slots[slotIndex].SetEmpty();
		}
		
		internal void UpdateWithRoom(FFRoom a_room)
		{
			roomNameLabel.text = a_room.roomName;
			
			for(int i = 0 ; i < teams.Length ; i++)
			{
				for(int j = 0 ; j < teams[i].slots.Length ; j++)
				{
					teams[i].slots[j].SetPlayer(a_room.teams[i].Slots[j].netPlayer);
				}
			}
		}
	}
}