using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

namespace FF
{	
	internal class FFByteWriter
	{
		protected MemoryStream _data;
		protected BinaryWriter _writer;
		protected byte[] _dataToReturn;
		
		internal FFByteWriter()
		{
			_data = new MemoryStream();
			_writer = new BinaryWriter(_data);
		}
		
		internal byte[] Data
		{
			get
			{
				if(_dataToReturn == null)
					_dataToReturn = _data.ToArray();
				return _dataToReturn;
			}
		}
		
		internal long Length
		{
			get
			{
				if(_dataToReturn == null)
					return _data.Length;
				return _dataToReturn.Length;
			}
		}
		
		internal void Close()
		{
			_writer.Close();
			_dataToReturn = _data.ToArray();
		}
		
		#region Writing Type
		internal void Write(byte[] bytes)
		{
			if(bytes != null)
			{
				Write (true);
				_writer.Write(bytes);
			}
			else
			{
				Write (false);
			}
		}
		
		internal void Write(bool a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(short a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(int a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(long a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(float a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(double a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(char a_val)
		{
			_writer.Write(a_val);
		}
		
		internal void Write(string a_val)
		{
			if(a_val != null)
			{
				Write (true);
				_writer.Write(a_val);
			}
			else
			{
				Write (false);
			}
		}
		
		internal void Write<T>(List<T> a_list) where T : IByteStreamSerialized
		{
			if(a_list != null)
			{
				Write (a_list.Count);
				foreach(IByteStreamSerialized each in a_list)
				{
					Write (each);
				}
			}
			else
			{
				Write (0);
			}
		}
		
		internal void Write<T>(T[] a_array) where T : IByteStreamSerialized
		{
			if(a_array != null)
			{
				Write (a_array.Length);
				foreach(IByteStreamSerialized each in a_array)
				{
					Write (each);
				}
			}
			else
			{
				Write (0);
			}
		}
		
		internal void Write(IByteStreamSerialized a_object)
		{
			
			if(a_object != null)
			{
				Write (true);
				a_object.SerializeData(this);
			}
			else
			{
				Write (false);
			}
		}
		#endregion
	}
}