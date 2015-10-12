using UnityEngine;
using System.Collections;

namespace FF.Networking
{
    internal abstract class FFHandler
    {
        #region Static
        protected static int _nextId = 0;
        protected static int NextId
        {
            get
            {
                return _nextId++;
            }
        }
        #endregion

        #region Properties
        protected int _id;
        internal int ID
        {
            get
            {
                return _id;
            }
        }

        protected bool _isComplete = false;

        protected SimpleCallback _onSuccess = null;
        #endregion

        internal FFHandler()
        {
            _id = NextId;
            FFEngine.Handler.RegisterHandler(this);
        }

        ~FFHandler()
        {
            FFLog.Log(EDbgCat.Handler, "Handler Destroyed : " + this.ToString());
        }

        internal virtual void DoUpdate()
        {
            if (IsComplete())
                OnComplete();
        }

        internal virtual bool IsComplete()
        {
            return _isComplete;
        }

        internal virtual void OnComplete()
        {
            _onSuccess = null;
            FFEngine.Handler.UnregisterHandler(this);
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
