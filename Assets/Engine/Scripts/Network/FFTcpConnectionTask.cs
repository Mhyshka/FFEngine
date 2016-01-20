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
        protected FFTcpClient _ffClient;
        #endregion

        internal FFTcpConnectionTask(FFTcpClient a_client)
        {
            _ffClient = a_client;
            _shouldRun = false;
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
        protected void Task()
        {
            bool success = false;
            try
            {
                FFLog.Log(EDbgCat.Networking,"Connecting.");
                _ffClient.TcpClient.Connect(_ffClient.Remote);
                success = true;
                
            }
            catch (SocketException e)
            {
                success = false;
                FFLog.LogWarning(EDbgCat.Networking, "Couldn't connect to server." + e.Message);
            }
            
            Thread.Sleep(100);
            if (_shouldRun)
            {
                if(success)
                    _ffClient.ConnectionSuccess();
                else
                     _ffClient.ConnectionFailed();
                _shouldRun = false;
            }
        }
        #endregion
    }
}