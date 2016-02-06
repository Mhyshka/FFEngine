using UnityEngine;
using System.Collections;
using System;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class RemovedFromRoomReceiver : BaseMessageReceiver
    {
        #region Properties
        protected SimpleCallback _onKick;
        protected SimpleCallback _onBan;
        #endregion

        internal RemovedFromRoomReceiver(SimpleCallback a_onKick, SimpleCallback a_onBan)
        {
            _onKick = a_onKick;
            _onBan = a_onBan;
        }

        protected override void HandleMessage()
        {
            if (_message.Data.Type == EDataType.Bool)
            {
                MessageBoolData data = _message.Data as MessageBoolData;//Value == Is A Ban
                if (!data.Data && _onKick != null)
                {
                    _onKick();
                }
                else if (data.Data && _onBan != null)
                {
                    _onBan();
                }
            }
        }
    }
}