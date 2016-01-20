using UnityEngine;
using System.Collections;

namespace FF.Handler
{
    internal abstract class AHandler
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
        #endregion

        internal AHandler()
        {
            _id = NextId;
            Engine.Handler.RegisterHandler(this);
        }

        ~AHandler()
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
            Engine.Handler.UnregisterHandler(this);
        }
    }
}
