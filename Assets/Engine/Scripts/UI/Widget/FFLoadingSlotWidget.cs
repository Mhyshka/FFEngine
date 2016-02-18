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
        [Header("Widgets references")]
        public UILabel usernameLabel = null;
        public UILabel rankingLabel = null;
        public FFRatingWidget rating = null;
        public FFButtonEvent kickButton = null;

        public PlayerLoadingSlotStateWidget baseState = null;
        public PlayerLoadingSlotStateWidget completeState = null;
        public PlayerLoadingSlotStateWidget readyState = null;
        public PlayerLoadingSlotDcedStateWidget dcedState = null;
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


        protected PlayerLoadingSlotStateWidget _currentState;
        #endregion

        void Awake()
        {
            completeState.gameObject.SetActive(false);
            dcedState.gameObject.SetActive(false);
            readyState.gameObject.SetActive(false);
            baseState.gameObject.SetActive(false);

            _currentState = baseState;
        }

        internal void SetLoading(FFNetworkPlayer a_player)
        {
            _player = a_player;

            usernameLabel.text = a_player.player.username;

            LoadState(baseState);
            rating.Value = a_player.player.Rating;
        }

        internal void SetComplete(FFNetworkPlayer a_player, int a_rank)
        {
            _player = a_player;

            usernameLabel.text = a_player.player.username;

            LoadState(completeState);
            rating.Value = a_player.player.Rating;
            rankingLabel.text = a_rank.ToString();
        }

        internal void SetReady(FFNetworkPlayer a_player, int a_rank)
        {
            _player = a_player;

            usernameLabel.text = a_player.player.username;

            LoadState(readyState);
            rating.Value = a_player.player.Rating;
            rankingLabel.text = a_rank.ToString();
        }

        internal void SetDCed(FFNetworkPlayer a_player)
        {
            if (_currentState != dcedState)
            {
                usernameLabel.text = a_player.player.username;

                LoadState(dcedState);

                if (Engine.Network.IsServer)
                {
                    dcedState.kickGo.SetActive(true);
                }
                else
                {
                    dcedState.kickGo.SetActive(false);
                }

                _dcedTimeElapsed = 0f;
                SetTimeDCed();
            }
        }


        protected void LoadState(PlayerLoadingSlotStateWidget a_state)
        {
            _currentState.gameObject.SetActive(false);

            _currentState = a_state;

            _currentState.gameObject.SetActive(true);
            rating.gameObject.SetActive(_currentState.needsRating);
            rankingLabel.gameObject.SetActive(_currentState.needsRanking);
            usernameLabel.color = _currentState.usernameColor;
        }

        protected void Update()
        {
            if (_currentState == dcedState)
            {
                _dcedTimeElapsed += Time.deltaTime;
                SetTimeDCed();
            }
        }

        protected void SetTimeDCed()
        {
            TimeSpan span = TimeSpan.FromSeconds(_dcedTimeElapsed);
            dcedState.dcedTimer.text = string.Format("{0}:{1}", span.Minutes.ToString("00"), span.Seconds.ToString("00"));
        }
    }
}