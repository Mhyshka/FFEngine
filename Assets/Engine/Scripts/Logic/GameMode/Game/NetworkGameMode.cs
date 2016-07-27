using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network;

namespace FF
{
	internal class NetworkGameMode : AGameMode
	{
        #region Properties
        internal override int ID
        {
            get
            {
                return (int)EGameModeID.Game;
            }
        }
        #endregion
    }
}