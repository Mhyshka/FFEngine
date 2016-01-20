using UnityEngine;
using System.Collections;

namespace FF.Handler
{
    internal abstract class ASimpleSuccess : AHandler
    {
        protected SimpleCallback _onSuccess = null;

        internal override void OnComplete()
        {
            base.OnComplete();
            _onSuccess = null;
        }

        #region Callbacks
        protected virtual void OnSuccess()
        {
            _isComplete = true;
            if (_onSuccess != null)
                _onSuccess();
        }
        #endregion
    }
}
