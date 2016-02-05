using UnityEngine;
using System.Collections;

namespace FF.Handler
{
    internal abstract class ABaseHandler
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

        protected bool _isCompleted;
        internal bool IsCompleted
        {
            get
            {
                return _isCompleted;
            }
        }
        #endregion

        internal ABaseHandler()
        {
            _isCompleted = false;
            _id = NextId;
            Engine.Handler.RegisterHandler(this);
        }

        ~ABaseHandler()
        {
            FFLog.Log(EDbgCat.Handler, "Handler Destroyed : " + this.ToString());
        }

        internal virtual void DoUpdate()
        {
        }

        internal virtual void Complete()
        {
            _isCompleted = true;
            Engine.Handler.UnregisterHandler(this);
        }
    }
}
