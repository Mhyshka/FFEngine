using UnityEngine;
using System.Collections;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
	[System.Serializable]
	internal struct UITeamRef
	{
        public UILabel teamNameLabel;
		public PlayerSlotWidget[] slots;
	}
	
	internal class FFMenuRoomPanel : FFPanel
	{
        #region Inspector Properties
        public UIButton startButton = null;
		public UILabel roomNameLabel = null;
		public UITeamRef[] teams = null;
        #endregion

        #region Properties
        protected bool _isReadyChecking = false;
        #endregion

        protected override void Awake ()
		{
			base.Awake ();
			for(int i = 0 ; i < teams.Length ; i++)
			{
				for(int j = 0 ; j < teams[i].slots.Length ; j++)
				{
					SlotRef data = new SlotRef();
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
			teams[teamIndex].slots[slotIndex].SetPlayer(null);
		}

        internal PlayerSlotWidget SlotForId(int a_networkId)
        {
            foreach (UITeamRef aTeam in teams)
            {
                foreach (PlayerSlotWidget aSlot in aTeam.slots)
                {
                    if (aSlot.Player != null && aSlot.Player.ID == a_networkId)
                    {
                        return aSlot;
                    }
                }
            }
            return null;
        }

        #region Ready Check
        internal void StartReadyCheck()
        {
            _isReadyChecking = true;
            foreach (UITeamRef aTeam in teams)
            {
                foreach (PlayerSlotWidget aSlot in aTeam.slots)
                {
                    if (aSlot.Player != null)
                    {
                        aSlot.readyCheck.Show();
                        aSlot.readyCheck.SetPending();
                    }
                }
            }
        }

        internal void StopReadyCheck()
        {
            _isReadyChecking = false;
            foreach (UITeamRef aTeam in teams)
            {
                foreach (PlayerSlotWidget aSlot in aTeam.slots)
                {
                    if (aSlot.Player != null)
                    {
                        aSlot.readyCheck.Hide();
                    }
                }
            }
        }
        #endregion

        internal void UpdateWithRoom(Room a_room)
		{
			roomNameLabel.text = a_room.roomName;
			
			for(int i = 0 ; i < teams.Length ; i++)
			{
                teams[i].teamNameLabel.text = a_room.teams[i].teamName;
				for(int j = 0 ; j < teams[i].slots.Length ; j++)
				{
					teams[i].slots[j].SetPlayer(a_room.teams[i].Slots[j].netPlayer);
				}
			}

            UpdateStartButtonState(a_room);
		}

        protected void UpdateStartButtonState(Room a_room)
        {
            if (Engine.Network.IsServer)
            {
                if (!startButton.gameObject.activeInHierarchy)
                {
                    startButton.gameObject.SetActive(true);
                }

                if (a_room.CanStart)
                {
                    startButton.isEnabled = !_isReadyChecking;
                }
                else
                {
                    startButton.isEnabled = false;
                }
            }
            else
            {
                if (startButton.gameObject.activeInHierarchy)
                {
                    startButton.gameObject.SetActive(false);
                }
            }
        }
	}
}