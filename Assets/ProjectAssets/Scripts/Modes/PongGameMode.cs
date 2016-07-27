using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Multiplayer;

namespace FF.Pong
{
    internal enum EPongGameState
    {
        ServiceChallenge,
        Service,
        Gameplay,
        Score,
        Result
    }

    internal class PongGameMode : NetworkGameMode
    {
        #region Inspector Properties
        public PongGameSettings gameSettings = null;
        public ABall ball = null;
        #endregion

        #region Properties
        internal bool needsReset = true;

        protected PongBoard _board = null;
        internal PongBoard Board
        {
            get
            {
                return _board;
            }
        }

        internal int serverPlayerId = 0;

        protected PongMatchData _score;
        internal PongMatchData Score
        {
            get
            {
                return _score;
            }
        }

        protected int _currendRoundIndex;
        internal int CurrentRoundIndex
        {
            get
            {
                return _currendRoundIndex;
            }
        }

        protected long _serviceTimestamp;
        internal long ServiceTimestamp
        {
            get
            {
                return _serviceTimestamp;
            }
            set
            {
                _serviceTimestamp = value;
            }
        }
        #endregion

        #region GameMode Methods
        protected override void Enter()
        {
            base.Enter();
            LoadGameSettings();
        }

        protected void LoadGameSettings()
        {
            ABall.Settings = gameSettings.ballSettings;
            RacketMotor.Settings = gameSettings.racketSettings;
        }

        internal void OnLoadingComplete()
        {
            ball.Init(this);
            InitRackets();
            _board.blueLifeLights.Init(gameSettings.requiredPointsToWin);
            _board.purpleLifeLights.Init(gameSettings.requiredPointsToWin);
            EnableGameplay();
        }

        internal override void OnLoadingExit()
        {
            NewMatch();
        }
        #endregion

        internal void RegisterBoard(PongBoard a_board)
        {
            _board = a_board;
        }

        #region Rackets & Controllers
        internal void InitRackets()
        {
            List<FFNetworkPlayer> players;

            players = Engine.Network.CurrentRoom.Teams[GameConstants.BLUE_TEAM_INDEX].Players;
            for(int i = 0; i < players.Count; i++)
            {
                _board.blueRackets[i].Init(players[i].ID, ball);
            }

            
            players = Engine.Network.CurrentRoom.Teams[GameConstants.PURPLE_TEAM_INDEX].Players;
            for (int i = 0; i < players.Count; i++)
            {
                _board.purpleRackets[i].Init(players[i].ID, ball);
            }
        }

        internal RacketMotor LocalRacket
        {
            get
            {
                return Board.RacketForId(Engine.Network.NetPlayer.ID);
            }
        }
        #endregion

        #region Gameplay
        internal void EnableGameplay()
        {
            if (LocalRacket != null)
                LocalRacket.Enable();
        }

        internal void DisableGameplay()
        {
            if (LocalRacket != null)
                LocalRacket.Disable();
        }

        internal void NewMatch()
        {
            serverPlayerId = 0;
            _score = new PongMatchData(gameSettings.requiredPointsToWin);
            _currendRoundIndex = 0;
            _board.blueLifeLights.ResetLives();
            _board.purpleLifeLights.ResetLives();
        }

        internal void ResetRackets()
        {
            foreach (RacketMotor each in Board.Rackets)
            {
                each.HardSetCurrentRatio(0.5f);
            }
        }

        internal void NextRound()
        {
            _currendRoundIndex++;
            ball.ResetBall();
            PongServiceChallengeState serviceChal = _states[(int)EPongGameState.ServiceChallenge] as PongServiceChallengeState;
            serverPlayerId = serviceChal.NextServerId();
            EnableGameplay();
            ResetRackets();
        }

        internal PongRoundData CurrentRound
        {
            get
            {
                return Score.Rounds[_currendRoundIndex];
            }
        }
        #endregion
    }
}
