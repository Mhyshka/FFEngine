﻿using UnityEngine;
using System.Collections;

using FF.Multiplayer;
using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongScoreState : APongServerState
    {
        #region Inspector Properties
        public float duration = 3f;
        #endregion

        #region properties
        protected PongScorePanel _scorePanel = null;
        #endregion

        internal override int ID
        {
            get
            {
                return (int)EPongGameState.Score;
            }
        }

        #region State Methods
        internal override void Enter()
        {
            base.Enter();
            _pongServerGm.DisableGameplay();
            _scorePanel = Engine.UI.GetPanel("ScorePanel") as PongScorePanel;
            DisplayScore();
        }

        internal override int Manage()
        {
            if (_timeElapsedSinceEnter > duration)
            {
                if (_pongServerGm.Score.HighestScore == _pongServerGm.Score.RequiredPointsToWin)
                    RequestState(outState.ID);
                else
                {
                    _pongServerGm.NextRound();
                    RequestState((int)EPongGameState.Service);
                }
            }

            return base.Manage();
        }
        #endregion

        #region Events
        void DisplayScore()
        {
            PongRoundData round = _pongServerGm.CurrentRound;
            FFNetworkPlayer player = Engine.Game.CurrentRoom.GetPlayerForId(round.strikerId);
            ESide playerSide = player.slot.team.teamIndex == GameConstants.BLUE_TEAM_INDEX ? ESide.Left : ESide.Right;
            _scorePanel.SetScore(player.player.username,
                                    playerSide,
                                    2);
            _scorePanel.DisplayDefaultComment();
        }
        #endregion
    }
}