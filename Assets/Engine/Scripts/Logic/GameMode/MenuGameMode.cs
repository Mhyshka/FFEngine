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
		internal void GoBack(FFEventParameter a_args)
		{
			if(navigationHistory.Count > 0 && states[CurrentStateID] is AMenuGameState)
			{
				AMenuGameState menuState = states[CurrentStateID] as AMenuGameState;
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
			FFEngine.Events.RegisterForEvent(EEventType.Back, GoBack);
		}
	
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent(EEventType.Back, GoBack);
		}
		#endregion
	}
}