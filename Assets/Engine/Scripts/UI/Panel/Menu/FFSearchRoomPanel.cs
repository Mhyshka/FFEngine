using UnityEngine;
using System.Net;
using System.Collections.Generic;

using FF.Multiplayer;

namespace FF.UI
{
	internal class FFSearchRoomPanel : FFPanel
	{
		#region Properties
		private Dictionary<Room, FFRoomCellWidget> roomsCells = new Dictionary<Room, FFRoomCellWidget>();
		public GameObject hostCellPrefab = null;
		public GameObject list = null;
        public IPEndpointInput directConnectInputField = null;
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
                    lHostCell.transform.SetParent(list.transform);
                    lHostCell.transform.localPosition = Vector3.zero;
                    lHostCell.transform.localScale = Vector3.one;
                    roomsCells.Add (a_room, lHostCell);
				}

                if (roomsCells.Count == 1 && Engine.Inputs.ShouldUseNavigation)
                {
                    UICamera.selectedObject = lObject;
                }

             
            }
		}

        internal FFRoomCellWidget RoomWidgetForEP(IPEndPoint a_endpoint)
        {
            foreach (KeyValuePair<Room, FFRoomCellWidget> pair in roomsCells)
            {
                if (pair.Key.serverEndPoint == a_endpoint)
                {
                    return pair.Value;
                }
            }

            return null;
        }

		internal void RemoveRoom (Room a_room)
		{
			FFRoomCellWidget lCell = null;
            if (roomsCells.TryGetValue(a_room, out lCell))
            {
                a_room.onRoomUpdated -= lCell.UpdateWithRoom;
                lCell.Destroy();
                roomsCells.Remove(a_room);
            }
		}

        

      
	}
}
