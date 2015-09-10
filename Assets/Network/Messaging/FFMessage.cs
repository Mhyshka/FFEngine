using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{	
	[System.Serializable]
	internal abstract class FFMessage
	{
		#region properties
		#endregion
		
		#region Methods
		internal abstract void Read(FFTcpClient a_tcpClient);
		
		/// <summary>
		/// Should this message be send over & over until it's succesfully delivered.
		/// </summary>
		internal virtual bool IsMandatory
		{
			get
			{
				return true;
			}
		}
		#endregion
	}
}