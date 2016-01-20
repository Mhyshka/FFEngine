using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
    internal enum ELoadingState
    {
        Loading,
        NotReady,
        Ready
    }

	internal class FFLoadingSlotWidget : MonoBehaviour
	{
        #region Inspector Properties
        public Text[] usernameLabels = null;

        public GameObject baseGO = null;
        public GameObject completeGO = null;
        public GameObject readyGo = null;
        public GameObject dcedGO = null;
		
		public FFRatingWidget rating = null;

        public Text rankingLabel = null;

        public GameObject kickGO = null;
        public FFButtonEvent kickButton = null;

        public Text dcTimer = null;
        #endregion

        #region Properties
        protected float _dcedTimeElapsed = 0f;

        protected FFNetworkPlayer _player;
        internal FFNetworkPlayer Player
        {
            get
            {
                return _player;
            }
        }
        #endregion
        internal void SetLoading(FFNetworkPlayer a_player)
        {
            _player = a_player;
            foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

            baseGO.SetActive(true);
            completeGO.SetActive(false);
            readyGo.SetActive(false);
            dcedGO.SetActive(false);

            rating.gameObject.SetActive(true);
            rating.Value = a_player.player.Rating;

            kickGO.SetActive(false);
        }

        internal void SetComplete(FFNetworkPlayer a_player, int a_rank)
        {
            _player = a_player;

            foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

            baseGO.SetActive(false);
            completeGO.SetActive(true);
            readyGo.SetActive(false);
            dcedGO.SetActive(false);

            rating.gameObject.SetActive(true);
            rating.Value = a_player.player.Rating;

            rankingLabel.text = a_rank.ToString();

            kickGO.SetActive(false);
        }

        internal void SetReady(FFNetworkPlayer a_player, int a_rank)
        {
            _player = a_player;

            foreach (Text each in usernameLabels)
            {
                each.text = a_player.player.username;
            }

            baseGO.SetActive(false);
            completeGO.SetActive(false);
            readyGo.SetActive(true);
            dcedGO.SetActive(false);

            rating.gameObject.SetActive(true);
            rating.Value = a_player.player.Rating;

            rankingLabel.text = a_rank.ToString();

            kickGO.SetActive(false);
        }

        internal void SetDCed(FFNetworkPlayer a_player)
        {
            if (!dcedGO.activeSelf)
            {
                foreach (Text each in usernameLabels)
                {
                    each.text = a_player.player.username;
                }

                baseGO.SetActive(false);
                completeGO.SetActive(false);
                readyGo.SetActive(false);
                dcedGO.SetActive(true);

                rating.gameObject.SetActive(false);

                if (Engine.Network.IsServer)
                {
                    kickGO.SetActive(true);
                }
                else
                {
                    kickGO.SetActive(false);
                }

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