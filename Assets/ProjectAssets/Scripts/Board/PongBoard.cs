using UnityEngine;

using System.Collections.Generic;
using System.Collections;

namespace FF.Pong
{
    internal class PongBoard : MonoBehaviour
    {
        #region
        public Camera gameplayCamera = null;

        [Header("Blue Side")]
        public RacketMotor blueRacket = null;
        public LifeLightRamp blueLifeLights = null;

        [Header("Blue Side")]
        public RacketMotor purpleRacket = null;
        public LifeLightRamp purpleLifeLights = null;
        #endregion

        #region Properties
        protected List<RacketMotor> _rackets;
        internal List<RacketMotor> Rackets
        {
            get
            {
                return _rackets;
            }
        }
        #endregion

        void Awake()
        {
            _rackets = new List<RacketMotor>();
            _rackets.Add(blueRacket);
            _rackets.Add(purpleRacket);

            PongGameMode pgm = Engine.Game.CurrentGameMode as PongGameMode;
            pgm.RegisterBoard(this);
        }

        internal RacketMotor RacketForId(int a_id)
        {
            foreach (RacketMotor each in Rackets)
            {
                if (each.clientId == a_id)
                    return each;
            }

            return null;
        }
    }
}
