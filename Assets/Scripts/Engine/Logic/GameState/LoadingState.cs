using UnityEngine;
using System.Collections;

//Custom inspector
public class LoadingState : AGameState
{
#region Inspector Properties
	public string[] additionalRequiredScenes = null;
	
	[BitMaskUIScenesAttribute]
	public int panelsToLoad = 0;
#endregion

#region Properties
	internal string[] loadedPanelsScenes;

	protected float POST_LOADING_DURATION = 2.0f;
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
		
		loadedPanelsScenes = FFUtils.BitMaskToUiScenes(panelsToLoad);
		FFEngine.UI.LoadPanelsSet(loadedPanelsScenes);
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
