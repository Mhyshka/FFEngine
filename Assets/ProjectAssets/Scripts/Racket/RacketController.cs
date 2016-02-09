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

        internal virtual Vector3 UpdatePosition(float a_minPositionX, float a_maxPositionX, float a_currentRatio)
        {
            Vector3 position = transform.localPosition;
            position.x = Mathf.Lerp(a_minPositionX, a_maxPositionX, a_currentRatio);
            return position;
        }

        internal virtual void ForceRatio(float a_currentRatio)
        {
        }
    }
}