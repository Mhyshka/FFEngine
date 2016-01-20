using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.UI;
using FF.Multiplayer;
using FF.Network.Message;
using System;

namespace FF.Logic
{
    internal class GameplayState : AGameState
    {
        #region Properties
        #endregion

        #region State Methods
        #endregion

        #region Events
        #endregion
        internal override int ID
        {
            get
            {
                return (int)EStateID.Game;
            }
        }
    }
}