using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using FF.Network.Message;

namespace FF.Network.Receiver
{
    internal class ReceiverManager : BaseManager
    {
        #region Properties
        protected Dictionary<int, List<BaseReceiver>> _registeredReceiver;

        internal BaseReceiver RESPONSE_ALWAYS_SUCCESS = new RequestReceiverFixedResponse(ERequestErrorCode.Success, new MessageEmptyData());
        internal BaseReceiver RESPONSE_ALWAYS_FAIL = new RequestReceiverFixedResponse(ERequestErrorCode.Failed, new MessageEmptyData());
        #endregion

        #region Manager
        internal ReceiverManager()
        {
            _registeredReceiver = new Dictionary<int, List<BaseReceiver>>();

            RegisterReceiver(EMessageChannel.CancelRequest.ToString(), new CancelReceiver());
            RegisterReceiver(EMessageChannel.Response.ToString(), new ResponseReceiver());

            RegisterReceiver(EMessageChannel.RoomInfos.ToString(), new RoomInfosReceiver());

            RegisterReceiver(EMessageChannel.Heartbeat.ToString(), new HeartbeatReceiver());

            RegisterReceiver(EMessageChannel.IsIdle.ToString(), new IsIdleReceiver());
            RegisterReceiver(EMessageChannel.IsAlive.ToString(), RESPONSE_ALWAYS_SUCCESS);

            RegisterReceiver(EMessageChannel.Farewell.ToString(), new FarewellReceiver());
            RegisterReceiver(EMessageChannel.LeavingRoom.ToString(), new LeavingRoomReceiver());
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
            int hash = a_channel.GetHashCode();
            if (!_registeredReceiver.ContainsKey(hash))
            {
                _registeredReceiver.Add(hash, new List<BaseReceiver>()); 
            }

            _registeredReceiver[hash].Add(a_receiver);

            /*FFLog.LogError("Registered count : " + _registeredReceiver[a_type].Count);
            foreach (BaseReceiver each in _registeredReceiver[a_type])
            {
                if(each == null)
                    FFLog.LogError("each is null");
            }*/
        }

        internal void UnregisterReceiver(string a_channel, BaseReceiver a_receiver)
        {
            int hash = a_channel.GetHashCode();
            if (_registeredReceiver.ContainsKey(hash) && _registeredReceiver[hash].Count > 0)
            {
                _registeredReceiver[hash].Remove(a_receiver);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.Receiver, "Trying to remove a receiver from " + a_channel.ToString() + " while no receiver are register for this channel.");
            }
        }
        #endregion

        internal List<BaseReceiver> ReceiversForChannel(string a_channel)
        {
            int hash = a_channel.GetHashCode();
            List<BaseReceiver> results = null;
            
            if (_registeredReceiver.TryGetValue(hash, out results))
            {
                return results;
            }

            return null;
        }

        internal List<BaseReceiver> ReceiversForChannel(int a_channelHash)
        {
            List<BaseReceiver> results = null;

            if (_registeredReceiver.TryGetValue(a_channelHash, out results))
            {
                return results;
            }

            return null;
        }
    }
}