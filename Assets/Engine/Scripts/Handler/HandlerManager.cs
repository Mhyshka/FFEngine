using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF.Handler
{
    internal class HandlerManager : BaseManager
    {
        #region Manager
        internal HandlerManager()
        {
            _handlers = new Dictionary<int, ABaseHandler>();
            _handlersToRemove = new Queue<int>();
        }

        internal override void DoFixedUpdate()
        {
            lock (_handlers)
            {
                foreach (ABaseHandler each in _handlers.Values)
                {
                    each.DoUpdate();
                }

                lock (_handlersToRemove)
                {
                    while (_handlersToRemove.Count > 0)
                    {
                        int id = _handlersToRemove.Dequeue();
                        _handlers.Remove(id);
                    }
                }
            }
        }

        #endregion

        #region Handlers Management
        protected Dictionary<int, ABaseHandler> _handlers;
        protected Queue<int> _handlersToRemove;

        internal void RegisterHandler(ABaseHandler a_handler)
        {
            lock (_handlers)
            {
                _handlers.Add(a_handler.ID, a_handler);
            }
        }

        internal void UnregisterHandler(ABaseHandler a_handler)
        {
            lock(_handlersToRemove)
            {
                _handlersToRemove.Enqueue(a_handler.ID);
            }
        }
        #endregion
    }
}