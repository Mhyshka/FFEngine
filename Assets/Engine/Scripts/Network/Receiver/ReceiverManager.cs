using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ReceiverManager : BaseManager
    {
        #region Properties
        protected Dictionary<EMessageType, List<BaseReceiver>> _registeredReceiver;

        internal BaseReceiver RESPONSE_ALWAYS_SUCCESS = new RequestFixedResponse(new Message.ResponseSuccess());
        internal BaseReceiver RESPONSE_ALWAYS_FAIL = new RequestFixedResponse(new Message.ResponseFail((int)ESimpleRequestErrorCode.Forbidden));
        internal BaseReceiver RESPONSE_ALWAYS_CANCEL = new RequestFixedResponse(new Message.ResponseCancel());
        #endregion

        #region Manager
        internal ReceiverManager()
        {
            _registeredReceiver = new Dictionary<EMessageType, List<BaseReceiver>>();

            RegisterReceiver(EMessageType.RoomInfos, new MessageRoomInfos());

            RegisterReceiver(EMessageType.ResponseSuccess, new ResponseSuccess());
            RegisterReceiver(EMessageType.ResponseFail, new ResponseFail());
            RegisterReceiver(EMessageType.ResponseCancel, new ResponseCancel());

            RegisterReceiver(EMessageType.Heartbeat, new MessageHeartbeat());

            RegisterReceiver(EMessageType.RequestEmpty, RESPONSE_ALWAYS_SUCCESS);
            RegisterReceiver(EMessageType.IsIdleRequest, new RequestIsIdle());

            RegisterReceiver(EMessageType.Farewell, new MessageFarewell());
            RegisterReceiver(EMessageType.LeavingRoom, new MessageLeavingRoom());
        }

        internal override void TearDown()
        {
            foreach (List<BaseReceiver> each in _registeredReceiver.Values)
            {
                each.Clear();
            }
            _registeredReceiver.Clear();
        }
        #endregion

        #region Register / Unregister
        internal void RegisterReceiver(EMessageType a_type, BaseReceiver a_receiver)
        {
            if (!_registeredReceiver.ContainsKey(a_type))
            {
                _registeredReceiver.Add(a_type, new List<BaseReceiver>()); 
            }

            _registeredReceiver[a_type].Add(a_receiver);

            /*FFLog.LogError("Registered count : " + _registeredReceiver[a_type].Count);
            foreach (BaseReceiver each in _registeredReceiver[a_type])
            {
                if(each == null)
                    FFLog.LogError("each is null");
            }*/
        }

        internal void UnregisterReceiver(EMessageType a_type, BaseReceiver a_receiver)
        {
            if (_registeredReceiver.ContainsKey(a_type) && _registeredReceiver[a_type].Count > 0)
            {
                _registeredReceiver[a_type].Remove(a_receiver);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Receiver, "Trying to remove a receiver from " + a_type.ToString() + " while no receiver are register for this type.");
            }
        }
        #endregion

        internal List<BaseReceiver> ReceiversForType(EMessageType a_type)
        {
            if (_registeredReceiver[a_type] != null && _registeredReceiver[a_type].Count > 0)
            {
                return _registeredReceiver[a_type];
            }

            return null;
        }
    }
}