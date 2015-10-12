using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF.Networking
{
    internal class FFHandlerManager
    {
        internal FFHandlerManager()
        {
            _handlers = new Dictionary<int, FFHandler>();
            _handlersToRemove = new Queue<int>();
        }

        #region Message Handlers
        protected Dictionary<int, FFHandler> _handlers;
        protected Queue<int> _handlersToRemove;

        internal void DoUpdate()
        {
            lock (_handlers)
            {
                foreach (FFHandler each in _handlers.Values)
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

        internal void RegisterHandler(FFHandler a_handler)
        {
            lock (_handlers)
            {
                _handlers.Add(a_handler.ID, a_handler);
            }
        }

        internal void UnregisterHandler(FFHandler a_handler)
        {
            lock(_handlersToRemove)
            {
                _handlersToRemove.Enqueue(a_handler.ID);
            }
        }
        #endregion
    }
}