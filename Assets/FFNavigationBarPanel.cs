using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace FF
{
	internal class FFNavigationBarPanel : FFPanel
	{
		#region Inspector Properties
		public Text title = null;
		#endregion

		internal void setTitle (string newTitle)
		{
			title.text = newTitle;
		}


		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
