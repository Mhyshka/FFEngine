using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
	internal class FFSlotWidget : MonoBehaviour
	{
        #region Inspector Properties
        public Text[] usernameLabels = null;

        public GameObject usedGO = null;
        public GameObject selfGO = null;
        public GameObject emptyGO = null;
        public GameObject dcedGO = null;
		
		public Image[] useTVImages = null;
		public Image[] isHostImages = null;
		public FFRatingWidget rating = null;
		
		public Image dcedBackground = null;
		public Image usedBackground = null;
        public Image selfBackground = null;
        public Image emptyBackground = null;

        public Text dcTimer = null;
		
		public FFButtonEvent button = null;

        public FFReadyCheckWidget readyCheck = null;
        #endregion

        #region Properties
        protected Button _uiButton;

        protected float _dcedTimeElapsed = 0f;
		
		protected bool _isUsed = false;
		internal bool IsUsed
		{
			get
			{
				return _isUsed;
			}
		}

        protected FFNetworkPlayer _player;
        internal FFNetworkPlayer Player
        {
            get
            {
                return _player;
            }
        }
        #endregion

        protected void Awake()
		{
            _uiButton = GetComponent<Button>();
            SetEmpty();
		}
		
		internal void SetPlayer(FFNetworkPlayer a_player)
		{
            _player = a_player;
            if (a_player == null)
            {
                SetEmpty();
            }
            else if (a_player.isDced)
            {
                SetDCed(a_player);
            }
            else
            {
                if (a_player.ID == Engine.Network.NetworkID)
                {
                    SetSelf(a_player);
                }
                else
                {
                    SetUsed(a_player);
                }
            }
		}

        protected void SetUsed(FFNetworkPlayer a_player)
        {
            foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

            _uiButton.targetGraphic = usedBackground;
            usedGO.SetActive(true);
            selfGO.SetActive(false);
            dcedGO.SetActive(false);
            emptyGO.SetActive(false);

            foreach (Image each in isHostImages)
            {
                each.enabled = a_player.isHost;
            }

            foreach (Image each in useTVImages)
            {
                each.enabled = a_player.useTV;
            }

            rating.gameObject.SetActive(true);
            rating.Value = a_player.player.Rating;
        }

        protected void SetSelf(FFNetworkPlayer a_player)
        {
            foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

            _uiButton.targetGraphic = usedBackground;
            usedGO.SetActive(false);
            selfGO.SetActive(true);
            dcedGO.SetActive(false);
            emptyGO.SetActive(false);

            foreach (Image each in isHostImages)
            {
                each.enabled = a_player.isHost;
            }

            foreach (Image each in useTVImages)
            {
                each.enabled = a_player.useTV;
            }

            rating.gameObject.SetActive(true);
            rating.Value = a_player.player.Rating;
        }

        protected void SetEmpty()
		{
            foreach (Text each in usernameLabels)
            {
                each.text = "Empty";
            }

            _uiButton.targetGraphic = emptyBackground;
            usedGO.SetActive(false);
            selfGO.SetActive(false);
            emptyGO.SetActive(true);
            dcedGO.SetActive(false);

            rating.gameObject.SetActive(true);
            rating.Value = 0;
        }

        protected void SetDCed(FFNetworkPlayer a_player)
        {
            if (!dcedGO.activeSelf)
            {
                foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

                _uiButton.targetGraphic = dcedBackground;
                usedGO.SetActive(false);
                selfGO.SetActive(false);
                emptyGO.SetActive(false);
                dcedGO.SetActive(true);

                rating.gameObject.SetActive(false);
                _dcedTimeElapsed = 0f;
                SetTimeDCed();
            }
        }

        protected void Update()
        {
            if (dcedGO.activeSelf)
            {
                _dcedTimeElapsed += Time.deltaTime;
                SetTimeDCed();
            }
        }

        protected void SetTimeDCed()
        {
            TimeSpan span = TimeSpan.FromSeconds(_dcedTimeElapsed);
            dcTimer.text = string.Format("{0}:{1}", span.Minutes.ToString("00"), span.Seconds.ToString("00"));
        }
	}
}