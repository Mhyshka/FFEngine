using UnityEngine;
using System.Collections;
using System;

namespace FF
{
	internal static class FFByteArrayExtension
	{
		#region Methods
		internal static byte[] Insert(this byte[] a_data, byte[] a_newData, int a_startIndex = 0)
		{
			byte[] result = null;
			if(a_startIndex >= a_data.Length)
			{
				FFLog.LogError("Cant insert byte[] after data.Length. Did you want to append it?");
				result = a_data.Append (a_newData);
			}
			else
			{
				int size = a_data.Length + a_newData.Length;
				result = new byte[size];
				
				if(a_startIndex > 0)
					a_data.Read(a_startIndex).CopyTo(result,0);
				a_newData.CopyTo(result,a_startIndex);
				int lastPartIndex = a_startIndex + a_newData.Length;
				a_data.CopyTo(result,lastPartIndex);
			}
			
			return result;
		}
		
		internal static byte[] Append(this byte[] a_data, byte[] a_newData)
		{
			int size = a_data.Length + a_newData.Length;
			byte[] result = new byte[size];
			a_data.CopyTo(result,0);
			a_newData.CopyTo(result,a_data.Length);
			
			return result;
		}
		
		internal static byte[] Extract(ref byte[] a_data, int length)
		{
			byte[] result = new byte[length];
			if(length > 0)
			{
				int size = a_data.Length - length;
				byte[] dataLeft = new byte[size];
				for(int i = 0 ; i < a_data.Length ; i++)
				{
					
					if(i < length)
					{
						result[i] = a_data[i];
					}
					else
					{
						dataLeft[i - length] = a_data[i];
					}
					
				}
				a_data = dataLeft;
			}
			return result;
		}
		
		internal static byte[] Read(this byte[] a_data, int length)
		{
			byte[] result = new byte[length];
			if(length > 0)
			{
				for(int i = 0 ; i < length; i++)
				{
					result[i] = a_data[i];
				}
			}
			
			return result;
		}
		#endregion
	}
}