using UnityEngine;

using FF.UI;
using FF.Multiplayer;

namespace FF
{
    internal delegate void RoomCellCallback(FFRoomCellWidget a_widget);

    internal class FFRoomCellWidget : MonoBehaviour 
	{
        #region Inspector Properties
        public string deviceSprite = null;
		public string tvSprite = null;
		
		public UILabel gameNameLabel = null;
		public UILabel playerCountLabel = null;
        public UILabel spectatorCountLabel = null;
        public UILabel latencyLabel = null;
		public UISprite deviceImage = null;
		
		public FFRoomSelectionButton button = null;

        public UITweener deathTween = null;
        #endregion


        #region Properties

        private Room _room;
        internal Room Room
        {
            get
            {
                return _room;
            }
        }
		#endregion

		internal void UpdateWithRoom (Room aRoom)
		{
			_room = aRoom;
			button.room = _room;
			gameNameLabel.text = aRoom.roomName;
			playerCountLabel.text = "Players : " + aRoom.TotalPlayers + " / " + aRoom.TotalPlayableSlots;
            spectatorCountLabel.text = "Spectators : " + aRoom.TotalSpectatorPlayers + " / " + aRoom.TotalSpectatorSlots;
            if (aRoom.IsSecondScreenActive)
			{
				deviceImage.spriteName = tvSprite;
			}
			else
			{
				deviceImage.spriteName = deviceSprite;
			}
		}
		
		internal void UpdateLatency(double latency)
		{
			if(!latencyLabel.gameObject.activeSelf)
				latencyLabel.gameObject.SetActive(true);
				
			latencyLabel.text = latency.ToString("0.") + " ms";
		}

        #region Destroy
        internal void Destroy()
        {
            button.enabled = false;
            deathTween.onFinished.Add(new EventDelegate(OnHidden));
            deathTween.PlayForward();
        }

        public void OnHidden()
        {
            deathTween.onFinished.Clear();
            Destroy(gameObject);
        }
        #endregion
    }
}