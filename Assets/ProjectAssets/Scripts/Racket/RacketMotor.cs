using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal delegate void RacketMotorCallback(RacketMotor a_motor);

    internal class RacketMotor : MonoBehaviour
    {
        #region Inspector Properties
        [Header("Ball Snap")]
        public float maxOffsetX = 0.5f;
        public Transform ballSnapLocator = null;

        #region References
        [Header("Script references")]
        public RacketTouchController touchController = null;
        public RacketMouseController mouseController = null;
        public RacketRemoteController remoteController = null;
        public RacketNetworkController networkController = null;
        public RacketSmashRange smashRange = null;
        #endregion

        #region Movement
        [Header("Movement")]
        public float startingRatio = 1f;

        public float lerpSpeed = 1f;
        public float moveTowardSpeed = 0f;

        public float maxPositionX = 7.5f;
        #endregion

        #region References
        [Header("Bounce")]
        [Range(0.2f,1f)]
        public float maxBounceFactorX = 0.5f;

        [Range(1f, 2f)]
        public float smashVelocityMultiplier = 1.5f;
        #endregion
        #endregion

        #region NetworkProperties
        internal int clientId;
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
                _targetRatio = value;
                _currentRatio = value;
                UpdatePosition();
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

        internal SimpleCallback onTrySmash = null;
        internal SimpleCallback onDidSmash = null;
        #endregion

        #region Main
        void Awake()
        {
            _currentRatio = startingRatio;
            _targetRatio = _currentRatio;
            UpdatePosition();

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
            _currentRatio = Mathf.Lerp(_currentRatio, _targetRatio, Time.deltaTime * lerpSpeed);
            _currentRatio = Mathf.MoveTowards(_currentRatio, _targetRatio, Time.deltaTime * moveTowardSpeed);
            UpdatePosition();
        }

        void UpdatePosition()
        {
            Vector3 position = transform.localPosition;
            position.x = Mathf.Lerp(-maxPositionX, maxPositionX, _currentRatio);
            transform.localPosition = position;
        }

        internal void TrySmash()
        {
            if (!smashRange.didSmash)
            {
                if (smashRange.IsInSmashRange)
                {
                    smashRange.didSmash = true;

                    if (onDidSmash != null)
                        onDidSmash();

                    /*if (!Engine.Network.IsServer && clientId == Engine.Network.NetworkID)
                    {
                        Network.Message.MessageDidSmash message = new Network.Message.MessageDidSmash(-1);
                        Engine.Network.MainClient.QueueMessage(message);
                    }*/
                }
                else
                {
                    if (onTrySmash != null)
                        onTrySmash();

                    if (Engine.Network.IsServer)
                    {
                        Network.Message.MessageTrySmash message = new Network.Message.MessageTrySmash(clientId);
                        Engine.Network.Server.BroadcastMessage(message);
                    }
                    else if (clientId == Engine.Network.NetworkID)
                    {
                        Network.Message.MessageTrySmash message = new Network.Message.MessageTrySmash(clientId);
                        Engine.Network.MainClient.QueueMessage(message);
                    }
                }
            }
        }
        #endregion

        #region Enable & Disable
        internal void Init(int a_clientId, ABall a_ball)
        {
            clientId = a_clientId;
            ball = a_ball;

            if (clientId == Engine.Network.NetworkID)
            {
#if UNITY_EDITOR
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

            Multiplayer.FFNetworkPlayer player = Engine.Game.CurrentRoom.GetPlayerForId(clientId);
            if (player.slot.team.teamIndex == GameConstants.BLUE_TEAM_INDEX)
            {
                side = ESide.Left;
            }
            else if (player.slot.team.teamIndex == GameConstants.PURPLE_TEAM_INDEX)
            {
                side = ESide.Right;
            }
        }

        internal void Enable()
        {
            _currentController.Activate();
            _currentController.enabled = true;
        }

        internal void Disable()
        {
            _currentController.TearDown();
            _currentController.enabled = false;
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
                Mathf.Abs(_currentRatio - _lastSentRatio) > minRatioDelta)
            {
                NetworkUpdate();
            }
        }

        void NetworkUpdate()
        {
            _lastSentRatio = _currentRatio;
            _lastRefreshTime = Time.time;

            if (!Engine.Network.IsServer && !(CurrentController is RacketNetworkController)) //Local's player racket
            {
                Engine.Network.MainClient.QueueMessage(new FF.Network.Message.MessageFloatData(_targetRatio, clientId));
            }
            else if (Engine.Network.IsServer)
            {
                Engine.Network.Server.BroadcastMessage(new FF.Network.Message.MessageFloatData(_targetRatio, clientId));
            }
        }
        #endregion
    }
}