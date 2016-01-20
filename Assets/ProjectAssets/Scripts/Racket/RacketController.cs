using UnityEngine;
using System.Collections;

namespace FF.Pong
{
    internal abstract class ARacketComponent : MonoBehaviour
    {
        #region Inspector Properties
        #endregion

        #region Properties
        internal RacketMotor motor = null;
        #endregion

        internal abstract void Activate();
        internal abstract void TearDown();
    }
}