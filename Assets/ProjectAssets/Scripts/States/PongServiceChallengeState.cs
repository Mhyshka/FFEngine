using UnityEngine;

namespace FF.Pong
{
    internal class PongServiceChallengeState : APongState
    {
        #region Inspector Properties
        public float lerpSpeed = 3f;
        public float moveSpeed = 1f;
        #endregion

        #region Properties
        protected int _bounceCount = 0;
        protected int _blueTeamIndex = 0;
        protected int _purpleTeamIndex = 0;

        protected float _currentLerpValue = 0f;
        protected int _currentBounceValue = 0;

        protected int _currentRacketId = 0;

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.ServiceChallenge;
            }
        }
        #endregion

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _bounceCount = -1;
            _blueTeamIndex = 0;
            _purpleTeamIndex = 0;

            _currentLerpValue = 0f;
            _currentBounceValue = 0;
        }

        internal override int Manage()
        {
            if (_bounceCount != -1)
            {
                UpdateServerRacket();
            }

            return base.Manage();
        }

        internal override void Exit()
        {
            base.Exit();
        }
        #endregion

        protected void UpdateServerRacket()
        {
            _currentLerpValue = Mathf.Lerp(_currentLerpValue, _bounceCount, Time.deltaTime * lerpSpeed);
            _currentLerpValue = Mathf.MoveTowards(_currentLerpValue, _bounceCount, Time.deltaTime * moveSpeed);

            for (int i = _currentBounceValue; i < Mathf.FloorToInt(_currentLerpValue); i++)
            {
                _currentBounceValue++;
                if (i % 2 == 0)//Left Side
                {
                    _blueTeamIndex = (_blueTeamIndex + 1) % Engine.Game.CurrentRoom.teams[GameConstants.BLUE_TEAM_INDEX].Players.Count;
                    _currentRacketId = _pongGm.Board.blueRackets[_blueTeamIndex].clientId;
                    _pongGm.ball.SnapToLocator(_pongGm.Board.blueRackets[_blueTeamIndex].ballSnapLocator);
                }
                else//Right Side
                {
                    _purpleTeamIndex = (_purpleTeamIndex + 1) % Engine.Game.CurrentRoom.teams[GameConstants.PURPLE_TEAM_INDEX].Players.Count;
                    _currentRacketId = _pongGm.Board.purpleRackets[_purpleTeamIndex].clientId;
                    _pongGm.ball.SnapToLocator(_pongGm.Board.purpleRackets[_purpleTeamIndex].ballSnapLocator);
                }
            }

            if (_currentBounceValue == _bounceCount)
            {
                OnAnimationComplete();
            }
        }

        internal int NextServerId()
        {
            _currentBounceValue++;
            if (_currentBounceValue % 2 == 0)//Left Side
            {
                _blueTeamIndex = (_blueTeamIndex + 1) % Engine.Game.CurrentRoom.teams[GameConstants.BLUE_TEAM_INDEX].Players.Count;
                _currentRacketId = _pongGm.Board.blueRackets[_blueTeamIndex].clientId;
                _pongGm.ball.SnapToLocator(_pongGm.Board.blueRackets[_blueTeamIndex].ballSnapLocator);
            }
            else//Right Side
            {
                _purpleTeamIndex = (_purpleTeamIndex + 1) % Engine.Game.CurrentRoom.teams[GameConstants.PURPLE_TEAM_INDEX].Players.Count;
                _currentRacketId = _pongGm.Board.purpleRackets[_purpleTeamIndex].clientId;
                _pongGm.ball.SnapToLocator(_pongGm.Board.purpleRackets[_purpleTeamIndex].ballSnapLocator);
            }

            return _currentRacketId;
        }

        protected virtual void OnAnimationComplete()
        {
            _pongGm.serviceClientId = _currentRacketId;
        }
    }
}