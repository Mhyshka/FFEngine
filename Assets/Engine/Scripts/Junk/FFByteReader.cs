using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;

using FF.Networking;

namespace FF
{
	internal interface IByteStreamSerialized
	{
		void SerializeData(FFByteWriter stream);
		void LoadFromData(FFByteReader stream);
	}
	
	internal class FFByteReader
	{
		protected MemoryStream _data;
		protected BinaryReader _reader;
		
		internal FFByteReader(byte[] a_data)
		{
			_data = new MemoryStream(a_data, false);
			_reader = new BinaryReader(_data);
		}
		
		internal byte[] Data
		{
			get
			{
				return _data.ToArray();
			}
		}
		
		internal long Length
		{
			get
			{
				return _data.Length;
			}
		}
		
		internal void Close()
		{
			_reader.Close();
		}
		
		#region primitive types
		internal byte[] TryReadBytes(int a_length)
		{
			byte[] result = new Byte[0];
			if(TryReadBool())
			{
				try
				{
					result = _reader.ReadBytes(a_length);
				}
				catch
				{
					FFLog.LogError("Couldn't read bytes from stream");
				}
			}
			return result;
			
		}
		
		internal bool TryReadBool()
		{
			bool result = false;
			try
			{
				result = _reader.ReadBoolean();
			}
			catch
			{
				FFLog.LogError("Couldn't read bool from stream");
			}
			return result;
		}
		
		internal short TryReadShort()
		{
			short result = 0;
			try
			{
				result = _reader.ReadInt16();
			}
			catch
			{
				FFLog.LogError("Couldn't read short from stream");
			}
			return result;
		}
		
		internal int TryReadInt()
		{
			int result = 0;
			try
			{
				result = _reader.ReadInt32();
			}
			catch
			{
				FFLog.LogError("Couldn't read int from stream");
			}
			return result;
		}
		
		internal long TryReadLong()
		{
			long result = 0;
			try
			{
				result = _reader.ReadInt64();
			}
			catch
			{
				FFLog.LogError("Couldn't read long from stream");
			}
			return result;
		}
		
		internal float TryReadFloat()
		{
			float result = 0f;
			try
			{
				result = _reader.ReadSingle();
			}
			catch
			{
				FFLog.LogError("Couldn't read float from stream");
			}
			return result;
		}
		
		internal double TryReadDouble()
		{
			double result = 0d;
			try
			{
				result = _reader.ReadDouble();
			}
			catch
			{
				FFLog.LogError("Couldn't read double from stream");
			}
			return result;
		}
		
		internal char TryReadChar()
		{
			char result = '0';
			try
			{
				result = _reader.ReadChar();
			}
			catch
			{
				FFLog.LogError("Couldn't read char from stream");
			}
			return result;
		}

		internal string TryReadString()
		{
			string result = "";
			if(TryReadBool())
			{
				try
				{
					result = _reader.ReadString();
				}
				catch
				{
					FFLog.LogError("Couldn't read string from stream");
				}
			}
			return result;
		}
		
		internal List<T> TryReadObjectList<T>() where T : IByteStreamSerialized, new()
		{
			int length = TryReadInt();
			List<T> list = new List<T>(length);
		/*	try
			{*/
				for(int i = 0 ; i < length ; i++)
				{
					T obj = new T();
					if(TryReadBool())
						obj.LoadFromData(this);
					list.Add(obj);
				}
			/*}
			catch
			{
				FFLog.LogError("Couldn't read List<Object> from stream");
			}*/
			
			return list;
		}
		
		internal T[] TryReadObjectArray<T>() where T : IByteStreamSerialized, new()
		{
			int length = TryReadInt();
			T[] array = new T[length];
			try
			{
				for(int i = 0 ; i < length ; i++)
				{
					T obj = new T();
					if(TryReadBool())
						obj.LoadFromData(this);
					array[i] = obj;
				}
			}
			catch
			{
				FFLog.LogError("Couldn't read Object[] from stream");
			}
			return array;
		}
		
		internal T TryReadObject<T>() where T : IByteStreamSerialized, new()
		{
			T obj = default(T);
			if(TryReadBool())
			{
				obj = new T();
				try
				{
					obj.LoadFromData(this);
				}
				catch
				{
					FFLog.LogError("Couldn't read Object from stream");
				}
			}
			return obj;
		}
		#endregion
	}
}