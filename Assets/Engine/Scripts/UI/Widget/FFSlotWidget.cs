using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using FF.Networking;

namespace FF.UI
{
	internal class FFSlotWidget : MonoBehaviour
	{
		public Text usedName = null;
        public Text emptyName = null;
        public Text dcedName = null;

        public GameObject usedGO = null;
        public GameObject emptyGO = null;
        public GameObject dcedGO = null;
		
		public Image useTV = null;
		public Image isHost = null;
		public FFRatingWidget rating = null;
		
		public Image dcedBackground = null;
		public Image usedBackground = null;
		public Image emptyBackground = null;

        public Text dcTimer = null;
		
		public FFButtonEvent button = null;

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
		
		internal void Awake()
		{
            _uiButton = GetComponent<Button>();
            SetEmpty();
		}
		
		internal void SetPlayer(FFNetworkPlayer a_player)
		{
            if (a_player == null)
            {
                SetEmpty();
            }
            else if (a_player.isDCed)
            {
                SetDCed(a_player);
            }
            else
            {
                usedName.text = a_player.player.username;

                _uiButton.targetGraphic = usedBackground;
                usedGO.SetActive(true);
                dcedGO.SetActive(false);
                emptyGO.SetActive(false);
				
				isHost.enabled = a_player.isHost;
				useTV.enabled = a_player.useTV;

                rating.gameObject.SetActive(true);
                rating.Value = a_player.player.Rating;
			}
		}
		
		internal void SetEmpty()
		{
            emptyName.text = "Empty";

             _uiButton.targetGraphic = emptyBackground;
             usedGO.SetActive(false);
             emptyGO.SetActive(true);
             dcedGO.SetActive(false);

             rating.gameObject.SetActive(true);
             rating.Value = 0;
        }

        internal void SetDCed(FFNetworkPlayer a_player)
        {
            dcedName.text = a_player.player.username;

            _uiButton.targetGraphic = dcedBackground;
            usedGO.SetActive(false);
            emptyGO.SetActive(false);
            dcedGO.SetActive(true);

            rating.gameObject.SetActive(false);
            _dcedTimeElapsed = 0f;
            SetTimeDCed();
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