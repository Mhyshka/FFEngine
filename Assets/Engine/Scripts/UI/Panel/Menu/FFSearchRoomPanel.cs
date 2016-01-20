using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeroconf;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using FF.Multiplayer;

namespace FF.UI
{
	internal class FFSearchRoomPanel : FFPanel
	{
		#region Properties
		private Dictionary<Room, FFRoomCellWidget> roomsCells = new Dictionary<Room, FFRoomCellWidget>();
		public GameObject hostCellPrefab = null;
		public VerticalLayoutGroup verticalLayout = null;
		
		public GameObject list = null;
		#endregion


		internal void ClearRoomsCells ()
		{
			foreach (Room aRoom in roomsCells.Keys) 
			{
                aRoom.onRoomUpdated -= roomsCells[aRoom].UpdateWithRoom;
                Destroy (roomsCells[aRoom].gameObject);
			}
			roomsCells.Clear ();
		}


		internal void AddRoom (Room a_room)
		{
			if (!roomsCells.ContainsKey (a_room)) 
			{
				// Load a HostCell
				GameObject lObject = GameObject.Instantiate (hostCellPrefab);
				FFRoomCellWidget lHostCell = lObject.GetComponent <FFRoomCellWidget>();
				if (lHostCell != null)
				{
					lHostCell.UpdateWithRoom (a_room);
                    a_room.onRoomUpdated += lHostCell.UpdateWithRoom;
                    lHostCell.transform.SetParent(verticalLayout.transform);
                    lHostCell.transform.localPosition = Vector3.zero;
                    lHostCell.transform.localScale = Vector3.one;
                    roomsCells.Add (a_room, lHostCell);
				}

                if (roomsCells.Count == 1 && Engine.Inputs.ShouldUseNavigation)
                    EventSystem.current.SetSelectedGameObject(lObject);
            }

            FFLog.LogError("Chiled count : " + verticalLayout.transform.childCount.ToString());
		}


		internal void RemoveRoom (Room a_room)
		{
			FFRoomCellWidget lCell = roomsCells [a_room];
            if (lCell != null)
            {
                a_room.onRoomUpdated -= lCell.UpdateWithRoom;
                lCell.Destroy();
                roomsCells.Remove(a_room);
            }
		}
	}
}
