using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Zeroconf;
using UnityEngine.UI;



namespace FF 
{
	internal class FFHostListPanel : FFPanel 
	{
		#region Properties
		private Dictionary<ZeroconfRoom, HostCell> roomsCells = new Dictionary<ZeroconfRoom, HostCell>();
		public GameObject hostCellPrefab = null;
		public VerticalLayoutGroup verticalLayout;
		#endregion


		internal void ClearRoomsCells ()
		{
			foreach (ZeroconfRoom aRoom in roomsCells.Keys) 
			{
				Destroy (roomsCells[aRoom].gameObject);
			}

			roomsCells.Clear ();
		}


		internal void AddRoom (ZeroconfRoom aRoom)
		{
			HostCell lCell;
			Debug.Log ("host list panel AddRoom 123");
			if (!roomsCells.TryGetValue (aRoom, out lCell)) 
			{
				Debug.Log ("roomsCells [aRoom] == null");
				// Load a HostCell
				GameObject lObject = GameObject.Instantiate (hostCellPrefab);
				Debug.Log ("prefab instantiate");
				HostCell lHostCell = lObject.GetComponent <HostCell>();
				Debug.Log ("host cell created");

				if (lHostCell != null)
				{
					Debug.Log ("lHostCell exist");
					lHostCell.UpdateWithZeroconfRoom (aRoom);
					Debug.Log ("UpdateWithZeroconfRoom");

					roomsCells.Add (aRoom, lHostCell);
					Debug.Log ("roomsCells");

					lHostCell.transform.SetParent (verticalLayout.transform);
					Debug.Log ("verticalLayout.transform");
				}
			}
			Debug.Log ("fin");
		}


		internal void RemoveRoom (ZeroconfRoom aRoom)
		{
			HostCell lCell = roomsCells [aRoom];
			if (lCell != null)
			{
				Destroy (lCell);
				roomsCells.Remove(aRoom);
			}
		}
	}
}
