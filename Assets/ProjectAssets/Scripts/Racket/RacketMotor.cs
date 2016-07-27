using UnityEngine;
using System.Collections;

using FF.Handler;
using FF.Network.Message;

namespace FF.Pong
{
    internal delegate void RacketMotorCallback(RacketMotor a_motor);

    [System.Serializable]
    internal class RacketSettings
    {
        [Header("Service")]
        [Tooltip("Ball displacement up and down during service.")]
        public float serviceMaxOffsetX = 0.5f;

        [Header("Movement")]
        [Tooltip("Max x and -x position on the field for this GO")]
        public float maxPositionX = 7.5f;
        public float moveSpeed = 5f;

        [Header("Hit")]
        [Tooltip("The max angle at which the ball can bounce off. In degree")]
        public float maxBounceAngle = 60f;
    }

    internal class RacketMotor : MonoBehaviour
    {
        internal static RacketSettings Settings = null;

        #region Inspector Properties
        [Header("Ball Snap")]
        public Transform ballSnapLocator = null;

        #region References
        [Header("Script references")]
        public RacketTouchController touchController = null;
        public RacketMouseController mouseController = null;
        public RacketRemoteController remoteController = null;
        public RacketNetworkController networkController = null;
        public RacketSmashTiming smashTiming = null;
        #endregion

        #region Movement
        [Header("Movement")]
        public float startingRatio = 1f;

        #endregion
        #endregion

        #region NetworkProperties
        protected int _playerId = -1;
        internal int PlayerId
        {
            get
            {
                return _playerId;
            }
        }
        internal ABall ball;
        internal ESide side;
        #endregion

        #region Properties
        protected float _targetRatio;
        internal float TargetRatio
        {
            get
            {
                return _targetRatio;
            }
            set
            {
                _targetRatio = Mathf.Clamp01(value);
            }
        }

        protected float _currentRatio;
        internal float CurrentRatio
        {
            get
            {
                return _currentRatio;
            }
            set
            {
                _currentRatio = value;
                UpdatePosition();
            }
        }
        internal void HardSetCurrentRatio(float a_ratio)
        {
            CurrentRatio = a_ratio;
            _targetRatio = a_ratio;
            if (_currentController != null)
            {
                _currentController.ForceRatio(a_ratio);
            }
        }

        internal ARacketComponent _currentController;
        internal ARacketComponent CurrentController
        {
            get
            {
                return _currentController;
            }
        }
        #endregion

        #region Main
        void Awake()
        {
            _currentRatio = startingRatio;
            _targetRatio = _currentRatio;

            touchController.enabled = false;
            remoteController.enabled = false;
            networkController.enabled = false;
            mouseController.enabled = false;

            foreach (ARacketComponent each in GetComponentsInChildren<ARacketComponent>())
            {
                each.motor = this;
            }
        }

        void Update()
        {
            _currentRatio = Mathf.MoveTowards(_currentRatio, _targetRatio, Time.deltaTime * Settings.moveSpeed);
            UpdatePosition();
        }

        void UpdatePosition()
        {
            if (_currentController != null)
                transform.localPosition = _currentController.UpdatePosition(-Settings.maxPositionX, Settings.maxPositionX, _currentRatio);
        }
        #endregion

        #region Smash
        internal SimpleCallback onTrySmash = null;
        internal SimpleCallback onDidSmash = null;
        internal void TrySmash()
        {
            if (smashTiming.TrySmash())
            {
                if (onTrySmash != null)
                    onTrySmash();
            }
        }
        #endregion

        #region Enable & Disable
        internal void Init(int a_clientId, ABall a_ball)
        {
            _playerId = a_clientId;
            ball = a_ball;

            if (_playerId == Engine.Network.NetworkId)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                _currentController = mouseController;
#else
                if (Engine.Inputs.HasJoystickConnected)
                    _currentController = remoteController;
                else
                    _currentController = touchController;
#endif
            }
            else
            {
                _currentController = networkController;
                Enable();
            }
        }

        internal void Enable()
        {
            if (_currentController != null)
            {
                _currentController.Activate();
                _currentController.enabled = true;
            }
        }

        internal void Disable()
        {
            if (_currentController != null)
            {
                _currentController.TearDown();
                _currentController.enabled = false;
            }
        }
        #endregion

        #region Network
        [Header("Network")]
        public int refreshPerSecond = 20;
        public float minRatioDelta = 0.02f;

        protected float _lastSentRatio = 0f;
        protected float _lastRefreshTime = 0f;
        protected float RefreshInterval
        {
            get
            {
                if (refreshPerSecond > 0)
                {
                    return 1f / (float)refreshPerSecond;
                }

                return 1f;
            }
        }

        void FixedUpdate()
        {
            if (Time.time - _lastRefreshTime > RefreshInterval &&
                Mathf.Abs(_targetRatio - _lastSentRatio) > minRatioDelta)
            {
                NetworkUpdate();
            }
        }

        void NetworkUpdate()
        {
            _lastSentRatio = _targetRatio;
            _lastRefreshTime = Time.time;

            MessageRacketMovementData data = new MessageRacketMovementData(_currentRatio, _targetRatio);
           

            if (!(CurrentController is RacketNetworkController)) //Local's player racket
            {
                if (!Engine.Network.IsServer)
                {
                    SentMessage message = new SentMessage(data,
                                                   EMessageChannel.RacketPosition.ToString() + _playerId.ToString(),
                                                   false,
                                                   false);
                    Engine.Network.MainClient.QueueMessage(message);
                }
                else
                {
                    SentBroadcastMessage message = new SentBroadcastMessage(Engine.Network.CurrentRoom.GetPlayersIds(),
                                                                            data,
                                                                            EMessageChannel.RacketPosition.ToString() + _playerId.ToString(),
                                                                            false,
                                                                            false);
                    message.Broadcast();
                }
            }
        }
        #endregion
    }
}