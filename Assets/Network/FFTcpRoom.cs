using UnityEngine;
using System.Collections;

using Zeroconf;

namespace FFNetworking
{
	internal class FFTcpRoom : ZeroconfRoom
	{
		#region Properties
		internal int maxPlayerCount = 0;
		internal int currentPlayerCount = 0;
		#endregion
	}
}