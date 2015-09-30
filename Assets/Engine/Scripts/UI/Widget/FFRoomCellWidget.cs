using UnityEngine;
using System.Collections;
using Zeroconf;
using UnityEngine.UI;

using FF.Networking;
using FF.UI;

namespace FF
{
    internal delegate void RoomCellCallback(FFRoomCellWidget a_widget);

    internal class FFRoomCellWidget : MonoBehaviour 
	{
        #region Inspector Properties
        public Sprite deviceSprite = null;
		public Sprite tvSprite = null;
		
		public Text gameNameLabel = null;
		public Text playerCountLabel = null;
		public Text latencyLabel = null;
		public Image deviceImage = null;
		
		public FFRoomSelectionButton button = null;

        public FFTween deathTween = null;
        #endregion


        #region Properties
        //internal RoomCellCallback onDestroy;

        private FFRoom _room;
        internal FFRoom Room
        {
            get
            {
                return _room;
            }
        }
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

        #region Destroy
        internal void Destroy()
        {
            button.enabled = false;
            deathTween.onTransitionForwardComplete += OnHidden;
            deathTween.PlayForward();
        }

        public void OnHidden()
        {
            deathTween.onTransitionForwardComplete -= OnHidden;
            Destroy(gameObject);
        }
        #endregion
    }
}