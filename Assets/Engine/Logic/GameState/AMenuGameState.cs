using UnityEngine;
using System.Collections;

namespace FFEngine
{
	internal abstract class AMenuGameState : AGameState
	{
		#region Inspector Properties
		public bool shouldStayInStack = true;
		#endregion
	
		#region Properties
		private bool isGoingBack = false;
		#endregion
	
		#region Methods
		internal override void Enter ()
		{
			base.Enter ();
			isGoingBack = false;
		}
		
		internal override void Exit ()
		{
			base.Exit ();
			if(!isGoingBack)
			{
				MenuGameMode mgm = _gameMode as MenuGameMode;
				if(mgm != null)
				{
					mgm.OnMenuStateExit(this);
				}
			}	
		}
		
		internal void GoBack(int a_id)
		{
			isGoingBack = true;
			RequestState(a_id);
		}
		#endregion
	
		
		
		
	}
}