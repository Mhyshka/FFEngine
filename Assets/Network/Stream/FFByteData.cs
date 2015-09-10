using UnityEngine;
using System.Collections;
using System;

namespace FF.Networking
{
	internal class FFByteData
	{
		#region Properties
		protected byte[] _data;
		internal byte[] Data
		{
			get
			{
				return _data;
			}
		}
		
		internal int Length
		{
			get
			{
				return _data.Length;
			}
		}
		#endregion
		
		#region Methods
		internal FFByteData()
		{
			_data = new byte[0];
		}
		
		internal byte[] Insert(byte[] a_newData, int a_startIndex = 0)
		{
			if(a_startIndex >= _data.Length)
			{
				FFLog.LogError("Cant insert byte[] after data.Length. Did you want to append it?");
				_data = Append (a_newData);
			}
			else
			{
				int size = _data.Length + a_newData.Length;
				byte[] result = new byte[size];
				
				if(a_startIndex > 0)
					Read(a_startIndex).CopyTo(result,0);
				a_newData.CopyTo(result,a_startIndex);
				int lastPartIndex = a_startIndex + a_newData.Length;
				_data.CopyTo(result,lastPartIndex);
				_data = result;
			}
			
			return _data;
		}
		
		internal byte[] Append(byte[] a_newData)
		{
			int size = _data.Length + a_newData.Length;
			byte[] result = new byte[size];
			_data.CopyTo(result,0);
			a_newData.CopyTo(result,_data.Length);
			
			_data = result;
			
			return _data;
		}
		
		internal byte[] Extract(int length)
		{
			byte[] result = new byte[length];
			if(length > 0)
			{
				int size = _data.Length - length;
				byte[] dataLeft = new byte[size];
				for(int i = 0 ; i < _data.Length ; i++)
				{
					if(i < length)
					{
						result[i] = _data[i];
					}
					else
					{
						dataLeft[i - length] = _data[i];
					}
				}
				
				_data = dataLeft;
			}
			
			return result;
		}
		
		internal byte[] Read(int length)
		{
			byte[] result = new byte[length];
			if(length > 0)
			{
				for(int i = 0 ; i < length; i++)
				{
					result[i] = _data[i];
				}
			}
			
			return result;
		}
		#endregion
	}
}