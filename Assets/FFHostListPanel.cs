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
			if (roomsCells [aRoom] == null) 
			{
				// Load a HostCell
				GameObject lObject = GameObject.Instantiate (hostCellPrefab);
				HostCell lHostCell = lObject.GetComponent <HostCell>();

				if (lHostCell != null)
				{
					lHostCell.UpdateWithZeroconfRoom (aRoom);

					roomsCells.Add (aRoom, lHostCell);

					lHostCell.transform.parent = verticalLayout.transform;
				}
			}
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
