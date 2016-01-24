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

        internal int serviceClientId = 0;

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
        #endregion

        #region GameMode Methods
        protected override void Enter()
        {
            base.Enter();
            LoadGameSettings();
        }

        protected void LoadGameSettings()
        {
            ball.baseSpeed = gameSettings.ballBaseVelocity;
            ball.smashSpeedMultiplier = gameSettings.smashSpeedMultiplier;
        }

        internal override void OnLoadingComplete()
        {
            NewMatch();
        }
        #endregion

        internal void RegisterBoard(PongBoard a_board)
        {
            _board = a_board;

            InitRackets();
            _board.blueLifeLights.Init(gameSettings.requiredPointsToWin);
            _board.purpleLifeLights.Init(gameSettings.requiredPointsToWin);
        }

        #region Rackets & Controllers
        internal void InitRackets()
        {
            List<FF.Multiplayer.FFNetworkPlayer> players;

            players = Engine.Game.CurrentRoom.teams[0].Players;
            if (players.Count > 0)
            {
                _board.blueRacket.Init(players[0].ID, ball);
            }

            
            players = Engine.Game.CurrentRoom.teams[1].Players;
            if (players.Count > 0)
            {
                _board.purpleRacket.Init(players[0].ID, ball);
            }
        }

        internal RacketMotor RacketForPlayer(int a_clientId)
        {
            if (_board.blueRacket.clientId == a_clientId)
                return _board.blueRacket;
            else if (_board.purpleRacket.clientId == a_clientId)
                return _board.purpleRacket;
            return null;
        }

        internal RacketMotor LocalRacket
        {
            get
            {
                if (_board.blueRacket.clientId == Engine.Network.NetworkID)
                    return _board.blueRacket;
                else if (_board.purpleRacket.clientId == Engine.Network.NetworkID)
                    return _board.purpleRacket;
                return null;
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
            int random = Random.Range(0, Engine.Game.CurrentRoom.players.Count);

            int i = 0;
            FFNetworkPlayer player = null;
            foreach (FFNetworkPlayer each in Engine.Game.CurrentRoom.players.Values)
            {
                if (i == random)
                {
                    player = each;
                    break;
                }
                i++;
            }

            serviceClientId = player.ID;
            _score = new PongMatchData(gameSettings.requiredPointsToWin);
            _currendRoundIndex = 0;
            _board.blueLifeLights.ResetLives();
            _board.purpleLifeLights.ResetLives();
        }

        internal void ResetRackets()
        {
            foreach (RacketMotor each in Board.Rackets)
            {
                each.CurrentRatio = 0.5f;
            }
        }

        internal void NextRound()
        {
            _currendRoundIndex++;
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
