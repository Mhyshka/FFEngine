using UnityEngine;
using System.Collections;

using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FF.Networking
{
	internal abstract class FFTcpStreamThread
	{
		#region Properties
		protected FFTcpClient _ffClient = null;
		protected TcpClient _client = null;
		protected MemoryStream _memoryStream = null;
		protected BinaryFormatter _binaryFormatter = null;
		protected Thread _thread = null;
		
		protected FFByteData _data = null;
		protected bool _shouldRun = false;
		#endregion
		
		internal FFTcpStreamThread(FFTcpClient a_ffClient)
		{
			_ffClient = a_ffClient;
			_client = a_ffClient.TcpClient;
		}
		
		#region Start & Stop
		internal void Start()
		{
			if(_thread == null || !_thread.IsAlive)
			{
				_thread = new Thread(new ThreadStart(Task));
				_thread.IsBackground = true;
				_binaryFormatter = new BinaryFormatter();
				_shouldRun = true;
				_thread.Start();
			}
			else
			{
				FFLog.LogError(EDbgCat.Networking, "Reader thread is already running.");
			}
		}
		
		internal void Stop()
		{
			_shouldRun = false;
		}
		#endregion
		
		#region Task
		protected abstract void Task();
		#endregion
	}
}
