using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF;

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
        public ServerBall ball = null;
        #endregion

        #region Properties
        protected PongBoard _board = null;
        internal PongBoard Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
                InitRackets();
            }   
        }
        internal int serviceClientId = 0;
        #endregion

        #region GameMode Methods
        #endregion

        #region Rackets & Controllers
        internal void InitRackets()
        {
            List<FF.Multiplayer.FFNetworkPlayer> players;

            players = Engine.Game.CurrentRoom.teams[0].Players;
            if (players.Count > 0)
            {
                _board.blueRacket.Init(players[0].ID);
            }

            
            players = Engine.Game.CurrentRoom.teams[1].Players;
            if (players.Count > 0)
            {
                _board.purpleRacket.Init(players[0].ID);
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
        #endregion
    }
}
