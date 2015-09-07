using UnityEngine;
using System.Collections;

namespace FF
{
	//Custom inspector
	public class LoadingState : AGameState
	{
	#region Inspector Properties
		public string[] additionalRequiredScenes = null;
		
		[BitMaskUIScenesAttribute]
		public int panelsToLoad = 0;
		
		[HideInInspector]
		public string[] panelNamesToLoad = null;
	#endregion
	
	#region Properties
		internal string[] loadedPanelsScenes;
	
		protected float POST_LOADING_DURATION = 0.1f;
		protected float _postLoadTimeElapsed = 0f;
	#endregion
	
	#region States Methods
		internal override int ID
		{
			get
			{
				return (int)EStateID.Loading;
			}
		}
	
		internal override void Enter ()
		{
			base.Enter ();
			_postLoadTimeElapsed = 0f;
			System.GC.Collect();
			
			foreach(string each in additionalRequiredScenes)
			{
				_gameMode.LoadAsyncScene(each);
			}
			
			FFEngine.UI.LoadPanelsSet(panelNamesToLoad);
		}
	
		internal override int Manage ()
		{
			if(_gameMode.IsLoadingComplete)
			{
				if(_postLoadTimeElapsed < POST_LOADING_DURATION)
				{
					_postLoadTimeElapsed += Time.deltaTime;
				}
				else
				{
					return (int)outState.ID;
				}
			}
			return base.Manage ();
		}
	
		internal override void Exit ()
		{
			if(FFEngine.UI.HasLoadingScreen)
				FFEngine.UI.HideLoading();
			base.Exit ();
		}
	#endregion
	
	#region Event Management
		protected override void RegisterForEvent ()
		{
			base.RegisterForEvent ();
		}
	
		protected override void UnregisterForEvent ()
		{
			base.UnregisterForEvent ();
		}
		
	/*	internal void OnUILoadingComplete(FFEventParameter a_args)
		{
		}*/
	#endregion
	}
}