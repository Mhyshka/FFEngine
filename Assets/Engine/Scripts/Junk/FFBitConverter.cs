using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace FF
{
	internal static class FFBitConverter
	{
		#region Int
		private static byte[] GetBytes(int val)
		{
			Int32 valCast = val;
			return BitConverter.GetBytes(valCast);
		}
		
		private static int ToInt(byte[] data)
		{
			return (int)BitConverter.ToInt32(data,0);
		}
		#endregion
		
		#region long
		private static byte[] GetBytes(long val)
		{
			Int64 valCast = val;
			return BitConverter.GetBytes(valCast);
		}
		
		private static long ToLong(byte[] data)
		{
			return BitConverter.ToInt64(data,0);
		}
		#endregion
		
		#region float
		private static byte[] GetBytes(float val)
		{
			return BitConverter.GetBytes(val);
		}
		
		private static float ToFloat(byte[] data)
		{
			return BitConverter.ToSingle(data,0);
		}
		#endregion
		
		#region double
		private static byte[] GetBytes(double val)
		{
			return BitConverter.GetBytes(val);
		}
		
		private static double ToDouble(byte[] data)
		{
			return BitConverter.ToDouble(data,0);
		}
		#endregion
		
		#region bool
		private static byte[] GetBytes(bool val)
		{
			return BitConverter.GetBytes(val);
		}
		
		private static bool ToBool(byte[] data)
		{
			return BitConverter.ToBoolean(data,0);
		}
		#endregion
		
		#region char
		private static byte[] GetBytes(char val)
		{
			return BitConverter.GetBytes(val);
		}
		
		private static char ToChar(byte[] data)
		{
			return BitConverter.ToChar(data,0);
		}
		#endregion
		
		#region string
		private static byte[] GetBytes(string val)
		{
			return UnicodeEncoding.Unicode.GetBytes(val);
		}
		
		private static string ToString(byte[] data)
		{
			return UnicodeEncoding.Unicode.GetString(data);
		}
		#endregion
	}
}