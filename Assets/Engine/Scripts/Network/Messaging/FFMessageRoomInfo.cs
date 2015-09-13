using UnityEngine;
using System.Collections;
using System.Net.Sockets;

namespace FF.Networking
{
	[System.Serializable]
	internal class FFMessageRoomInfo : FFMessage
	{
		#region Properties
		public FFRoom room;
		#endregion
		
		internal FFMessageRoomInfo(FFRoom a_room)
		{
		 	room = a_room;
		}
		
		#region Methods
		internal override void Read(FFTcpClient a_tcpClient)
		{
			GameRoomSearchState state = FFEngine.Game.CurrentGameMode().CurrentState as GameRoomSearchState;
			if(state != null)
				state.OnRoomAdded(room);
		}	
		#endregion
	}
}