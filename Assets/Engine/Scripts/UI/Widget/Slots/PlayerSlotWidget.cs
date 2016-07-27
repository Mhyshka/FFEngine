using UnityEngine;
using System.Collections;
using System;

using FF.Network;
using FF.Multiplayer;

namespace FF.UI
{
	internal class PlayerSlotWidget : MonoBehaviour
	{
        #region Inspector Properties
        [Header("Widgets references")]
        public UILabel usernameLabel = null;
        public UISprite useTVImage = null;
        public UISprite isHostImage = null;
        public FFRatingWidget rating = null;
        public FFButtonEvent button = null;
        public FFReadyCheckWidget readyCheck = null;

        [Header("States")]
        public PlayerSlotStateWidget usedState = null;
        public PlayerSlotStateWidget selfState = null;
        public PlayerSlotDcedStateWidget dcedState = null;
        public PlayerSlotStateWidget emptyState = null;
        #endregion

        #region Properties
        protected UIButton _uiButton;

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

        protected PlayerSlotStateWidget _currentState;
        #endregion

        protected void Awake()
		{
            _uiButton = GetComponent<UIButton>();

            emptyState.gameObject.SetActive(false);
            usedState.gameObject.SetActive(false);
            selfState.gameObject.SetActive(false);
            dcedState.gameObject.SetActive(false);

            _currentState = emptyState;
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
                if (a_player.ID == Engine.Network.NetworkId)
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
            usernameLabel.text = a_player.player.username;
            LoadState(usedState);
            rating.Value = a_player.player.Rating;

            isHostImage.enabled = a_player.isHost;
            useTVImage.enabled = a_player.useTV;
        }

        protected void SetSelf(FFNetworkPlayer a_player)
        {
            usernameLabel.text = a_player.player.username;
            LoadState(selfState);
            rating.Value = a_player.player.Rating;

            isHostImage.enabled = a_player.isHost ;
            useTVImage.enabled = a_player.useTV;

        }

        protected void SetEmpty()
		{
            usernameLabel.text = "Empty";
            LoadState(emptyState);
            rating.Value = 0;
        }


        protected void LoadState(PlayerSlotStateWidget a_state)
        {
            _currentState.gameObject.SetActive(false);

            _currentState = a_state;

            _currentState.gameObject.SetActive(true);
            _uiButton.tweenTarget = _currentState.backgroundImage.gameObject;
            rating.gameObject.SetActive(_currentState.needsRating);
            isHostImage.enabled = _currentState.needsHostIcon;
            useTVImage.enabled = _currentState.needsTvIcon;
            usernameLabel.color = _currentState.usernameColor;
        }


        #region Dced
        protected void SetDCed(FFNetworkPlayer a_player)
        {
            usernameLabel.text = a_player.player.username;

            if(dcedState != _currentState)
                _dcedTimeElapsed = 0f;

            LoadState(dcedState);
            SetTimeDCed();
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
        #endregion
    }
}