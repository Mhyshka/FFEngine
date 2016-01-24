using UnityEngine;
using System.Collections;

using FF;
using FF.Network;

namespace FF.Pong
{
    internal class PongServerGameMode : PongGameMode
    {
        #region Inspector Properties
        internal ServerBall ServerBall
        {
            get
            {
                return ball as ServerBall;
            }
        }
        #endregion
    }
}
