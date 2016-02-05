using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ReceiverManager : BaseManager
    {
        #region Properties
        protected Dictionary<string, List<BaseReceiver>> _registeredReceiver;

        /*internal BaseReceiver RESPONSE_ALWAYS_SUCCESS = new RequestReceiverFixedResponse(new ResponseSuccess());
        internal BaseReceiver RESPONSE_ALWAYS_FAIL = new RequestReceiverFixedResponse(new ResponseFail((int)ERequestErrorCode.Forbidden));*/
        #endregion

        #region Manager
        internal ReceiverManager()
        {
            _registeredReceiver = new Dictionary<string, List<BaseReceiver>>();

            /*RegisterReceiver(EDataType.M_RoomInfos, new RoomInfosReceiver());

            RegisterReceiver(EDataType.R_Success, new SuccessReceiver());
            RegisterReceiver(EDataType.R_Fail, new FailReceiver());
            RegisterReceiver(EDataType.M_Cancel, new CancelReceiver());

            RegisterReceiver(EDataType.R_Heartbeat, new HeartbeatReceiver());

            RegisterReceiver(EDataType.Q_Empty, RESPONSE_ALWAYS_SUCCESS);
            RegisterReceiver(EDataType.IsIdleRequest, new IsIdleReceiver());

            RegisterReceiver(EDataType.M_Farewell, new FarewellReceiver());
            RegisterReceiver(EDataType.M_LeavingRoom, new LeavingRoomReceiver());*/


            _client.EndConnection(_data.reason);
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
        internal void RegisterReceiver(string a_channel, BaseReceiver a_receiver)
        {
            if (!_registeredReceiver.ContainsKey(a_channel))
            {
                _registeredReceiver.Add(a_channel, new List<BaseReceiver>()); 
            }

            _registeredReceiver[a_channel].Add(a_receiver);

            /*FFLog.LogError("Registered count : " + _registeredReceiver[a_type].Count);
            foreach (BaseReceiver each in _registeredReceiver[a_type])
            {
                if(each == null)
                    FFLog.LogError("each is null");
            }*/
        }

        internal void UnregisterReceiver(string a_channel, BaseReceiver a_receiver)
        {
            if (_registeredReceiver.ContainsKey(a_channel) && _registeredReceiver[a_channel].Count > 0)
            {
                _registeredReceiver[a_channel].Remove(a_receiver);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Receiver, "Trying to remove a receiver from " + a_channel.ToString() + " while no receiver are register for this channel.");
            }
        }
        #endregion

        internal List<BaseReceiver> ReceiversForType(string a_channel)
        {
            List<BaseReceiver> results = null;
            
            if (_registeredReceiver.TryGetValue(a_channel, out results))
            {
                return results;
            }

            return null;
        }
    }
}