using UnityEngine;
using System.Collections;
using System;

namespace FF.Network.Receiver
{
    internal class MessageRemovedFromRoom : AReceiver<Message.MessageRemovedFromRoom>
    {
        #region Properties
        protected SimpleCallback _onKick;
        protected SimpleCallback _onBan;
        #endregion

        internal MessageRemovedFromRoom(SimpleCallback a_onKick, SimpleCallback a_onBan)
        {
            _onKick = a_onKick;
            _onBan = a_onBan;
        }

        protected override void HandleMessage()
        {
            if (!_message.isBan && _onKick != null)
            {
                _onKick();
            }
            else if (_message.isBan && _onBan != null)
            {
                _onBan();
            }
        }
    }
}