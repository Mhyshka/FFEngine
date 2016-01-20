using UnityEngine;
using System.Collections;

using FF.Logic;
using System;

namespace FF.Pong
{
    internal class PongServiceChallengeState : APongServerState
    {
        internal override int ID
        {
            get
            {
                return (int)EPongGameState.ServiceChallenge;
            }
        }


    }
}