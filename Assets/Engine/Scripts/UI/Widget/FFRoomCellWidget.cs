using UnityEngine;
using System.Collections;
using Zeroconf;
using UnityEngine.UI;

using FF.Networking;
using FF.UI;

namespace FF
{
	public class FFRoomCellWidget : MonoBehaviour 
	{
		#region Inspector Properties
		public Sprite deviceSprite = null;
		public Sprite tvSprite = null;
		
		public Text gameNameLabel = null;
		public Text playerCountLabel = null;
		public Text latencyLabel = null;
		public Image deviceImage = null;
		
		public FFRoomSelectionButton button = null;
		#endregion


		#region Properties
		private FFRoom _room;
		#endregion

		internal void UpdateWithRoom (FFRoom aRoom)
		{
			_room = aRoom;
			button.room = _room;
			gameNameLabel.text = aRoom.roomName;
			playerCountLabel.text = "Player count : " + aRoom.TotalPlayers + " / " + aRoom.TotalSlots;
			if(aRoom.IsSecondScreenActive)
			{
				deviceImage.sprite = tvSprite;
			}
			else
			{
				deviceImage.sprite = deviceSprite;
			}
		}
		
		internal void UpdateLatency(float latency)
		{
			if(!latencyLabel.gameObject.activeSelf)
				latencyLabel.gameObject.SetActive(true);
				
			latencyLabel.text = latency.ToString("0.") + " ms";
		}
	}
}