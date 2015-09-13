using UnityEngine;
using System.Collections;
using Zeroconf;
using UnityEngine.UI;

using FF.Networking;
using FF.UI;

namespace FF
{
	public class FFRoomCellWidget : MonoBehaviour 
	{
		#region Inspector Properties
		public Text hostName = null;
		public FFRoomSelectionButton button = null;
		#endregion


		#region Properties
		private FFRoom _room;
		#endregion

		internal void UpdateWithRoom (FFRoom aRoom)
		{
			_room = aRoom;
			button.room = _room;
			hostName.text = aRoom.roomName;
		}
	}
}