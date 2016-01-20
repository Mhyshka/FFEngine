using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal class PongBoard : MonoBehaviour
    {
        #region
        public Camera gameplayCamera = null;
        public RacketMotor blueRacket = null;
        public RacketMotor purpleRacket = null;
        #endregion

        void Awake()
        {
            PongGameMode pgm = Engine.Game.CurrentGameMode as PongGameMode;
            pgm.Board = this;
        }
    }
}
