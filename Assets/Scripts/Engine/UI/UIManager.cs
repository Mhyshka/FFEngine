using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class UIManager
{
	#region Properties
	internal FF_UI_Root root;
	private LoadingScreen _loadingScreen;
	
	private Camera _uiCamera;
	internal Camera UICamera{get{return _uiCamera;}}
	
	private Dictionary<string, FFPanel> _panelsByName;
	
	private bool _isLoading = false;
	private int _panelsToLoadCount = 0;
	#endregion
	
	internal UIManager()
	{
		_panelsByName = new Dictionary<string, FFPanel> ();
	}
	
	internal void RegisterUiCamera(Camera a_cam)
	{
		_uiCamera = a_cam;
	}
	
	#region Loading
	internal void LoadPanelsSet(string[] a_panelList)
	{
		
		if(a_panelList != null && a_panelList.Length > 0)
		{
			_isLoading = true;
			Debug.Log ("Starting to load " + a_panelList.Length + " panels.");
			foreach(string each in a_panelList)
			{
				if(string.IsNullOrEmpty(each))
					Debug.LogWarning("Null of empty panel given.");
				else
					LoadAsyncScene(each);
			}
		}
		else
		{
			_isLoading = false;
			Debug.Log ("No panel to load, UIManager is done.");
			FFEngine.Events.FireEvent(EEventType.UILoadingComplete);
			return;
		}
	}
	
	internal void LoadAsyncScene(string a_sceneName)
	{
		Application.LoadLevelAdditiveAsync(a_sceneName);
		_panelsToLoadCount++;
	}
	
	internal void ClearPanels()
	{
		foreach(FFPanel each in _panelsByName.Values)
		{
			GameObject.Destroy (each.gameObject);
		}
	}
	#endregion
	
	#region LoadingScreen
	internal void RegisterLoadingScreen(LoadingScreen a_loading)
	{
		_loadingScreen = a_loading;
	}
	
	internal void DisplayLoading()
	{
		_loadingScreen.Show();
	}
	
	internal void HideLoading()
	{
		_loadingScreen.Hide();
	}
	
	internal FFPanel.EState LoadingScreenState
	{
		get
		{
			return _loadingScreen.state;
		}
	}
	#endregion
	
	#region Register
	internal void Register(string a_eventKey, FFPanel a_panel)
	{
		_panelsToLoadCount--;
		if (!_panelsByName.ContainsKey (a_eventKey))
		{
			_panelsByName.Add (a_eventKey, a_panel);
			if(a_panel.ShouldMoveToRoot)
			{
				Transform parent = a_panel.transform;
				while(parent.parent != null)
				{
					parent = parent.parent;
				}
				a_panel.transform.SetParent(root.transform);
				
				GameObject.Destroy(parent.gameObject);
			}
		}
		else
		{
			Debug.LogWarning("Same panel registered twice.");
		}
		
		if(_panelsToLoadCount == 0 && _isLoading)
		{
			FFEngine.Events.FireEvent(EEventType.UILoadingComplete);
			Debug.Log ("UI LOADING COMPLETE");
		}
	}
	
	internal void Unregister(string a_eventKey)
	{
		if (_panelsByName.ContainsKey (a_eventKey))
		{
			_panelsByName.Remove(a_eventKey);
		}
		else
		{
			Debug.LogWarning("No panel registered with this key : " + a_eventKey);
		}
	}
	#endregion

	#region Access
	internal FFPanel GetPanel(string a_eventKey)
	{
		if (_panelsByName.ContainsKey (a_eventKey))
		{
			return _panelsByName[a_eventKey];
		}

		Debug.LogWarning("No panel registered with this key : " + a_eventKey);
		return null;
	}
	#endregion


	#region Display
	internal void SwitchToPanels(string[] a_panels)
	{
		List<string> panelsToShow = new List<string>();
		foreach(string each in a_panels)
		{
			panelsToShow.Add(each);
		}
		
		foreach(string each in _panelsByName.Keys)
		{
			if(!panelsToShow.Contains(each) && 
			   (_panelsByName[each].state == FFPanel.EState.Shown || _panelsByName[each].state == FFPanel.EState.Showing))
			{
				HideSpecificPanel(each);
			}
			else if(panelsToShow.Contains(each) && 
			       (_panelsByName[each].state == FFPanel.EState.Hidden || _panelsByName[each].state == FFPanel.EState.Hidding))
			{
				RequestDisplay(each);
			}
		}
	}
	
	internal void RequestDisplay(string[] a_panels)
	{
		foreach(string each in a_panels)
			RequestDisplay(each);
	}
	
	internal void RequestDisplay(string a_panel)
	{
		if(!_panelsByName.ContainsKey(a_panel))
		{
			Debug.LogWarning("No panel found using this name : " + a_panel);
			return;
		}
		
		if(_panelsByName[a_panel].state == FFPanel.EState.Hidden || _panelsByName[a_panel].state == FFPanel.EState.Hidding)
		{
			_panelsByName[a_panel].Show();
		}
		else
		{
			Debug.LogWarning("Panel is already shown or showing : " + a_panel);
		}
	}
	
	internal void HideSpecificPanel(string a_panel)
	{
		if(!_panelsByName.ContainsKey(a_panel))
		{
			Debug.LogWarning("No panel found using this name : " + a_panel);
			return;
		}
		
		if(_panelsByName[a_panel].state == FFPanel.EState.Showing || _panelsByName[a_panel].state == FFPanel.EState.Shown)
		{
			_panelsByName[a_panel].Hide();
		}
		else
		{
			Debug.LogWarning("Panel is already hidden or hidding : " + a_panel);
		}
	}
	
	internal void HideAllPanels()
	{
		foreach (FFPanel each in _panelsByName.Values)
		{
			if(each.state != FFPanel.EState.Hidden && each.state != FFPanel.EState.Hidding)
				each.Hide();
		}
	}
	#endregion
}
