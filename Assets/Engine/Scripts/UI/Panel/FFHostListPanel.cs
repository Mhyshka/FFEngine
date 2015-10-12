using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeroconf;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using FF.Networking;

namespace FF.UI
{
	internal class FFHostListPanel : FFPanel
	{
		#region Properties
		private Dictionary<FFRoom, FFRoomCellWidget> roomsCells = new Dictionary<FFRoom, FFRoomCellWidget>();
		public GameObject hostCellPrefab = null;
		public VerticalLayoutGroup verticalLayout = null;
		
		public GameObject list = null;
		#endregion


		internal void ClearRoomsCells ()
		{
			foreach (FFRoom aRoom in roomsCells.Keys) 
			{
                aRoom.onRoomUpdated -= roomsCells[aRoom].UpdateWithRoom;
                Destroy (roomsCells[aRoom].gameObject);
			}
			roomsCells.Clear ();
		}


		internal void AddRoom (FFRoom a_room)
		{
			FFRoomCellWidget lCell;
			if (!roomsCells.TryGetValue (a_room, out lCell)) 
			{
				// Load a HostCell
				GameObject lObject = GameObject.Instantiate (hostCellPrefab);
				FFRoomCellWidget lHostCell = lObject.GetComponent <FFRoomCellWidget>();
				if (lHostCell != null)
				{
					lHostCell.UpdateWithRoom (a_room);
                    a_room.onRoomUpdated += lHostCell.UpdateWithRoom;
                    roomsCells.Add (a_room, lHostCell);
					lHostCell.transform.SetParent (verticalLayout.transform);
					lHostCell.transform.localScale = Vector3.one;
				}
			}
		}


		internal void RemoveRoom (FFRoom a_room)
		{
			FFRoomCellWidget lCell = roomsCells [a_room];
            if (lCell != null)
            {
                if (EventSystem.current.currentSelectedGameObject == lCell.button.gameObject)
                    TrySelectWidget();

                a_room.onRoomUpdated -= lCell.UpdateWithRoom;
                lCell.Destroy();
                roomsCells.Remove(a_room);
            }
		}
	}
}
