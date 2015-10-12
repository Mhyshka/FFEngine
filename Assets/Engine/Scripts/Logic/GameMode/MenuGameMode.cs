using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF
{
	internal class MenuGameMode : AGameMode
	{
		#region Inspector Properties
		#endregion
		
		#region Properties
		internal override int ID
		{
			get
			{
				return (int)EGameModeID.Menu;
			}
		}
		Stack<int> navigationHistory = new Stack<int>();
        #endregion

        #region Methods
        protected virtual void OnBackPressed()
        {
            AMenuGameState state = CurrentState as AMenuGameState;
            state.GoBack();
        }

		internal void GoBack()
		{
			if(navigationHistory.Count > 0 && _states[CurrentStateID] is AMenuGameState)
			{
				AMenuGameState menuState = _states[CurrentStateID] as AMenuGameState;
				_isGoingBack = true;
				menuState.GoBack(navigationHistory.Pop());
			}
		}
		
		internal void OnMenuStateExit(AMenuGameState a_menuState)
		{
			if(a_menuState.shouldStayInStack)
			{
				navigationHistory.Push(a_menuState.ID);
			}
		}
		#endregion
		
		#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Inputs.PushOnBackCallback(OnBackPressed);
		}
	
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
            FFEngine.Inputs.PopOnBackCallback();
        }
		#endregion
	}
}