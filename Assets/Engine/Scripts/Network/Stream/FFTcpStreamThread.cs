﻿using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FF.Network
{
	internal abstract class FFTcpStreamThread
	{
		#region Properties
		protected FFNetworkClient _ffClient = null;
		protected TcpClient Client
		{
			get
			{
				return _ffClient.TcpClient;
			}
		}

        protected NetworkStream _stream;

        protected Thread _thread = null;
		
		protected bool _shouldRun = false;

        protected bool _crashed = false;
        #endregion

        internal FFTcpStreamThread(FFNetworkClient a_ffClient)
		{
			_ffClient = a_ffClient;
		}

        #region Start & Stop
        internal virtual void Start()
		{
			if(_thread == null || !_thread.IsAlive)
			{
				_thread = new Thread(new ThreadStart(Task));
				_thread.IsBackground = true;
				_shouldRun = true;
                _stream = Client.GetStream();
                _thread.Start();
                _crashed = false;
            }
			else
			{
				FFLog.LogError(EDbgCat.Networking, "Reader thread is already running.");
			}
		}
		
		internal virtual void Stop()
		{
			_shouldRun = false;
		}

        internal virtual void Close()
        {
            
        }
		#endregion
		
		#region Task
		protected abstract void Task();
		#endregion
	}
}
