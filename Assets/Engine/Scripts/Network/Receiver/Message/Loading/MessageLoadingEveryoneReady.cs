using UnityEngine;
using System.Collections;

namespace FF.Network.Receiver
    {
        internal class MessageLoadingEveryoneReady : AReceiver<Message.MessageLoadingEveryoneReady>
        {
            #region Properties
            SimpleCallback _onReceived;
            #endregion

            internal MessageLoadingEveryoneReady(SimpleCallback a_onReceived)
            {
                _onReceived = a_onReceived;
            }

            protected override void HandleMessage()
            {
                if (_onReceived != null)
                    _onReceived();
            }
        }
    }