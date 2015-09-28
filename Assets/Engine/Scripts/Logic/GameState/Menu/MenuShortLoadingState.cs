using UnityEngine;
using System.Collections;

namespace FF
{
	internal class MenuShortLoadingState : ANavigationMenuState
	{
		internal int outId = -1;
		internal string message = "Loading";
		
		internal override int ID
		{
			get
			{
				return (int) EMenuStateID.ShortLoading;
			}
		}
		
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
			FFEngine.Events.RegisterForEvent(EEventType.Next, OnNext);
		}
		
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
			FFEngine.Events.UnregisterForEvent(EEventType.Next, OnNext);
		}
		
		internal void OnNext(FFEventParameter a_args)
		{
			RequestState(outId);
		}
	}
}