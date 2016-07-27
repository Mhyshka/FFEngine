using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace FF.Network
{
    internal class FFTcpConnectionTask
    {
        #region Properties
        protected Thread _thread = null;
        protected bool _shouldRun;

        FFNetworkClient _client;
        SimpleCallback _onSuccess = null;
        SimpleCallback _onFail = null;
        #endregion

        internal FFTcpConnectionTask(FFNetworkClient a_client, SimpleCallback a_onSuccess, SimpleCallback a_onFail)
        {
            _client = a_client;
            _shouldRun = false;
            _onSuccess = a_onSuccess;
            _onFail = a_onFail;
        }

        internal void TearDown()
        {
            _onSuccess = null;
            _onFail = null;
        }

        #region Start & Stop
        internal virtual void Start()
        {
            if (_thread == null || !_shouldRun)
            {
                _thread = new Thread(new ThreadStart(Task));
                _thread.IsBackground = true;
                _shouldRun = true;
                _thread.Start();
            }
            else
            {
                FFLog.LogError(EDbgCat.Networking, "Connection thread is already running.");
            }
        }

        internal void Stop()
        {
            if (_shouldRun)
            {
                _shouldRun = false;
            }
        }
        #endregion

        #region Task
        AutoResetEvent _waitHandle = new AutoResetEvent(false);
        protected void Task()
        {
            bool success = false;

            while (_client.TcpClient == null)
            {
                try
                {
                    _client.TcpClient = new TcpClient(_client.Local);
                    _client.TcpClient.SendTimeout = 1000;
                    _client.TcpClient.ReceiveTimeout = 3000;
                }
                catch (Exception e)
                {
                    FFLog.LogWarning(EDbgCat.ClientConnection, "Couldn't create tcp client : " + e.Message);
                }
                Thread.Sleep(250);
            }

            //Timeout Thread
            Thread timeoutThread = new Thread(new ThreadStart(TimeoutTask));
            timeoutThread.Start();

            try
            {
                FFLog.Log(EDbgCat.ClientConnection, "Connecting.");
                _client.TcpClient.Connect(_client.Remote);
                success = true;
            }
            catch (Exception e)
            {
                success = false;
                FFLog.LogWarning(EDbgCat.ClientConnection, "Couldn't connect to server." + e.Message);
            }

            if (_shouldRun)
            {
                _waitHandle.Set();
                if (success)
                {
                    _onSuccess();
                }
                else
                {
                    Thread.Sleep(1000);
                    _onFail();
                }
                _shouldRun = false;
            }

            _thread = null;
        }

        protected void TimeoutTask()
        {
            if (!_waitHandle.WaitOne(1000))
            {
                _client.TcpClient.Close();
                _client.TcpClient = null;
                _shouldRun = false;
                _onFail();
            }
        }
        #endregion

        /*IAsyncResult ar = _client.TcpClient.BeginConnect(_client.Remote.Address, _client.Remote.Port, null, null);
        WaitHandle wh = ar.AsyncWaitHandle;
        try
        {
            FFLog.Log(EDbgCat.ClientConnection, "Connecting.");
            if (ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(3d)))
            {
                success = true;
                FFLog.Log(EDbgCat.ClientConnection, "Connection success");
            }
            else
            {
                success = false;
                FFLog.LogWarning(EDbgCat.ClientConnection, "Couldn't connect to server.");
            }

            _client.TcpClient.EndConnect(ar);
        }
        finally
        {
            wh.Close();
        }*/
    }
}