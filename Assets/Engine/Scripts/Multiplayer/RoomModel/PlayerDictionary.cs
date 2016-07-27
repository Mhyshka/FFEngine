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
            foreach (int id in Engine.Network.CurrentRoom.Players.Keys)
            {
                Add(id, new T());
            }

            if (Engine.Network.IsServer)
            {
                Engine.Network.CurrentRoom.onPlayerAdded += OnPlayerAdded;
                Engine.Network.CurrentRoom.onPlayerRemoved += OnPlayerRemoved;
            }
        }

        /// <summary>
        /// Constructor for deserialization only
        /// </summary>
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
                Engine.Network.CurrentRoom.onPlayerAdded = null;
                Engine.Network.CurrentRoom.onPlayerRemoved = null;
            }
        }
        #endregion

        #region Events
        protected void OnPlayerAdded(Room a_room, FFNetworkPlayer a_player)
        {
            Add(a_player.ID, new T());
        }

        protected void OnPlayerRemoved(Room a_room, FFNetworkPlayer a_player)
        {
            Remove(a_player.ID);
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