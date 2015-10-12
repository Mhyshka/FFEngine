using UnityEngine;
using System.Collections;

namespace FF
{
	internal abstract class AMenuGameState : AGameState
	{
		#region Inspector Properties
		public bool shouldStayInStack = true;
		#endregion
	
		#region Properties
		private bool isGoingBack = false;
		private MenuGameMode mgm = null;
		#endregion
	
		#region Methods
		internal override void Enter ()
		{
			base.Enter ();
			isGoingBack = false;
			mgm = _gameMode as MenuGameMode;
		}
		
		internal override void Exit ()
		{
			base.Exit ();
			if(!isGoingBack)
			{
				if(mgm != null)
				{
					mgm.OnMenuStateExit(this);
				}
			}	
		}
		
        /// <summary>
        /// Called by the game mode. Do not call it yourself.
        /// </summary>
		internal void GoBack(int a_id)
		{
            isGoingBack = true;
            RequestState(a_id);
		}

        internal virtual void GoBack()
        {
            mgm.GoBack();
        }
		#endregion
		
	}
}