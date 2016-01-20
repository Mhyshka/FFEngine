using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using FF.Network;
using System;

namespace FF.Multiplayer
{
    internal class PlayerDictionary<T> : Dictionary<int, T> where T : new()
    {
        #region Properties
        #endregion

        #region Construction
        internal PlayerDictionary() : base()
        {
            foreach (int id in Engine.Game.CurrentRoom.players.Keys)
            {
                Add(id, new T());
            }

            if (Engine.Network.IsServer)
            {
                Engine.Network.Server.onClientAdded += OnClientConnected;
                Engine.Network.Server.onClientRemoved += OnClientDisconnected;
            }
        }

        internal PlayerDictionary(bool a_empty) : base()
        {
            if (!a_empty)
            {
                FFLog.LogError("Player dictionary shoudln't be called with empty set to false.");
            }
        }

        internal void TearDown()
        {
            Clear();
            if (Engine.Network.IsServer)
            {
                Engine.Network.Server.onClientAdded -= OnClientConnected;
                Engine.Network.Server.onClientRemoved -= OnClientDisconnected;
            }
        }
        #endregion

        #region Events
        internal void OnClientConnected(FFTcpClient a_client)
        {
            Add(a_client.NetworkID, new T());
        }

        internal void OnClientDisconnected(FFTcpClient a_client)
        {
            Remove(a_client.NetworkID);
        }
        #endregion

        #region GetList
        internal List<int> GetIdsForValue(T a_value)
        {
            List<int> matchs = new List<int>();
            foreach (KeyValuePair<int, T> each in this)
            {
                if (each.Value.Equals(a_value))
                {
                    matchs.Add(each.Key);
                }
            }
            return matchs;
        }
        #endregion
    }
}