using System.Collections.Generic;

using System.Net;
using FF.Network.Message;

namespace FF.Network
{
    /// <summary>
    /// Manage clients as soon as they enter the Connected state.
    /// </summary>
    internal class FFGameServer
    {
        #region Properties
        protected FFTcpServer _tcpServer;

        internal List<int> GetClientsIds()
        {
            List<int> ids = new List<int>(Clients.Count);
            foreach (int each in _clients.Keys)
            {
                ids.Add(each);
            }
            return ids;
        }

        protected Dictionary<int, FFNetworkClient> _clients;
        internal Dictionary<int, FFNetworkClient> Clients
        {
            get
            {
                return _clients;
            }
        }

        internal FFNetworkClient ClientForId(int a_id)
        {
            FFNetworkClient toReturn = null;
            if (!_clients.TryGetValue(a_id, out toReturn))
            {
                FFLog.LogWarning(EDbgCat.ServerGame, "Couldn't find Client for id : " + a_id);
            }
            return toReturn;
        }
        #endregion

        internal FFGameServer(FFTcpServer a_tcpServer)
        {
            _tcpServer = a_tcpServer;

            _tcpServer.onClientReady += OnClientReady;
            _tcpServer.onClientRemoved += OnClientRemoved;

            _clients = new Dictionary<int, FFNetworkClient>();
            _clients.Add(_tcpServer.LoopbackClient.NetworkID, _tcpServer.LoopbackClient);
        }

        internal void Close()
        {
            _tcpServer.onClientReady -= OnClientReady;
            _tcpServer.onClientRemoved -= OnClientRemoved;

            _tcpServer = null;

            _clients.Clear();
        }

        #region Reconnection Only
        protected bool _isReconnectionOnly = false;
        internal bool IsReconnectionOnlyMode
        {
            get
            {
                return _isReconnectionOnly;
            }
        }

        internal void EnableReconnectionOnlyMode()
        {
            _isReconnectionOnly = true;
        }

        internal void DisableReconnectionOnlyMode()
        {
            _isReconnectionOnly = false;
        }

        internal void KeepPlayersOnly(List<int> a_clientsToKeep)
        {
            foreach (KeyValuePair<int, FFNetworkClient> keypair in _clients)
            {
                if (!a_clientsToKeep.Contains(keypair.Key))
                {
                    keypair.Value.Disconnect();
                }
            }
        }
        #endregion

        #region Client Ready
        internal IntCallback onClientConnected = null;
        internal IntCallback onClientReconnected = null;
        protected void OnClientReady(FFNetworkClient a_client, int a_serverId, int a_playerId)
        {
            FFNetworkClient toReplace = null;
            if (_clients.TryGetValue(a_playerId, out toReplace))
            {
                toReplace.onConnectionEnded = null;
                toReplace.Disconnect();
                _clients[a_playerId] = a_client;

                _tcpServer.ReconnectClient(a_client, a_serverId, a_playerId);

                FFLog.Log(EDbgCat.ServerGame, "Reconnecting client : " + a_client.NetworkID);
                if (onClientReconnected != null)
                    onClientReconnected(a_playerId);

                SendRoomInfo(a_client);
            }
            else if (!_isReconnectionOnly)
            {
                _clients.Add(a_client.NetworkID, a_client);
                FFLog.Log(EDbgCat.ServerGame, "New game client : " + a_client.NetworkID);
                if (onClientConnected != null)
                    onClientConnected(a_serverId);

                SendRoomInfo(a_client);
            }
            else
            {
                a_client.Disconnect();
            }

        }

        protected void SendRoomInfo(FFNetworkClient a_client)
        {
            MessageRoomData roomInfo = new MessageRoomData(Engine.Network.CurrentRoom);
            SentMessage message = new SentMessage(roomInfo,
                                                  EMessageChannel.RoomInfos.ToString());
            a_client.QueueMessage(message);
        }
        #endregion

        #region Removing
        internal IntCallback onClientRemoved = null;
        /// <summary>
        /// Remove a client from the ready clients dictionnary.
        /// </summary>
        internal void RemoveAndDisconnectClient(int a_id)
        {
            FFNetworkClient client = null;
            if (_clients.TryGetValue(a_id, out client))
            {
                client.Disconnect();
                
                FFLog.Log(EDbgCat.ServerGame, "Remove and disconnect client : " + client.NetworkID);
            }
        }


        internal IntCallback onClientConnectionLost = null;
        protected void OnClientRemoved(FFNetworkClient a_client)
        {
            FFLog.Log(EDbgCat.ServerGame, "On client removed from TcpServer : " + a_client.NetworkID);
            if (ShouldClientBeDisposed(a_client.NetworkID))
            {
                RemoveClient(a_client.NetworkID);
            }
            else
            {
                if (onClientConnectionLost != null)
                    onClientConnectionLost(a_client.NetworkID);
            }
        }

        protected void RemoveClient(int a_id)
        {
            FFNetworkClient client = null;
            if (_clients.TryGetValue(a_id, out client))
            {
                _clients.Remove(a_id);

                if (onClientRemoved != null)
                    onClientRemoved(a_id);

                FFLog.Log(EDbgCat.ServerGame, "Remove client : " + client.NetworkID);
            }
        }
        #endregion

        /// <summary>
        /// For now, if not in the room while connection lost -> Remove player.
        /// </summary>
        protected bool ShouldClientBeDisposed(int a_id)
        {
            return !Engine.Network.CurrentRoom.Players.ContainsKey(a_id);
        }
    }
}