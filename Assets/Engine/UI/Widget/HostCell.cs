using UnityEngine;
using System.Collections;
using Zeroconf;
using UnityEngine.UI;

namespace FF
{
	public class HostCell : MonoBehaviour 
	{
		#region Inspector Properties
		public Text hostName = null;
		#endregion


		#region Properties
		private ZeroconfRoom _room;
		#endregion


		internal void UpdateWithZeroconfRoom (ZeroconfRoom aRoom)
		{
			_room = aRoom;
			hostName.text = aRoom.roomName;
		}
	}
}