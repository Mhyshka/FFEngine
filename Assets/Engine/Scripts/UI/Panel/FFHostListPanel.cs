using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeroconf;
using UnityEngine.UI;

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
		public GameObject loader = null;
		#endregion


		internal void ClearRoomsCells ()
		{
			foreach (FFRoom aRoom in roomsCells.Keys) 
			{
				Destroy (roomsCells[aRoom].gameObject);
			}
			roomsCells.Clear ();
			
			list.gameObject.SetActive(false);
			loader.SetActive(true);
		}


		internal void AddRoom (FFRoom a_room)
		{
			if(roomsCells.Count <= 0)
			{
				list.gameObject.SetActive(true);
				loader.SetActive(false);
			}
			
			FFRoomCellWidget lCell;
			if (!roomsCells.TryGetValue (a_room, out lCell)) 
			{
				// Load a HostCell
				GameObject lObject = GameObject.Instantiate (hostCellPrefab);
				FFRoomCellWidget lHostCell = lObject.GetComponent <FFRoomCellWidget>();
				if (lHostCell != null)
				{
					lHostCell.UpdateWithRoom (a_room);
					roomsCells.Add (a_room, lHostCell);
					lHostCell.transform.SetParent (verticalLayout.transform);
					lHostCell.transform.localScale = Vector3.one;
				}
			}
		}


		internal void RemoveRoom (FFRoom aRoom)
		{
			FFRoomCellWidget lCell = roomsCells [aRoom];
            if (lCell != null)
            {
                Destroy(lCell.gameObject);
                roomsCells.Remove(aRoom);
            }
		}
	}
}
