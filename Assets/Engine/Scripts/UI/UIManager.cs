using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FF.UI
{
	internal class UIManager
	{
		#region Properties
		internal UIRoot _root;
		private LoadingScreen _loadingScreen;
		
		private Dictionary<string, FFPanel> _panelsByName;
        private Dictionary<string, FFPopup> _popupsByName;
        private Stack<FFPopupData> _pendingPopups;
		
		private bool _isLoading = false;
		private int _panelsToLoadCount = 0;
		#endregion
		
		internal UIManager()
		{
			_panelsByName = new Dictionary<string, FFPanel> ();
            _popupsByName = new Dictionary<string, FFPopup>();
            _pendingPopups = new Stack<FFPopupData>();
        }

        internal void DoUpdate()
        {
            if (_currentPopup == null && _pendingPopups.Count > 0)
            {
                DisplayNextPopup(_pendingPopups.Pop());
            }
        }
		
		#region Loading
		internal void LoadPanelsSet(string[] a_panelList)
		{
			
			if(a_panelList != null && a_panelList.Length > 0)
			{
				_isLoading = true;
				FFLog.Log(EDbgCat.UI,"Starting to load " + a_panelList.Length + " panels.");
				foreach(string each in a_panelList)
				{
					if(string.IsNullOrEmpty(each))
						FFLog.LogWarning(EDbgCat.UI,"Null of empty panel given.");
					else
						LoadAsyncScene(each);
				}
			}
			else
			{
				_isLoading = false;
				FFLog.Log(EDbgCat.UI,"No panel to load, UIManager is done.");
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
			
			Transform parent = a_loading.transform;
			while(parent.parent != null)
			{
				parent = parent.parent;
			}
			a_loading.transform.SetParent(_root.transform);
			
			GameObject.Destroy(parent.gameObject);
		}
		
		internal bool HasLoadingScreen
		{
			get
			{
				return _loadingScreen != null;
			}
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
				return _loadingScreen.State;
			}
		}
		#endregion
		
		#region Register
		internal void RegisterRoot(UIRoot a_rootUi)
		{
			_root = a_rootUi;
		}
		
		internal void Register(string a_eventKey, FFPanel a_panel)
		{
			_panelsToLoadCount--;
            //POPUPS
            if (a_panel is FFPopup && !_popupsByName.ContainsKey(a_eventKey))
            {
                _popupsByName.Add(a_eventKey, (FFPopup)a_panel);
                if (a_panel.ShouldMoveToRoot)
                {
                    RectTransform parent = a_panel.transform as RectTransform;
                    while (parent.parent != null)
                    {
                        parent = parent.parent as RectTransform;
                    }
                    parent.SetParent(_root.transform, false);
                }
            }
            //PANELS
			else if (!_panelsByName.ContainsKey (a_eventKey))
			{
				_panelsByName.Add (a_eventKey, a_panel);
				if(a_panel.ShouldMoveToRoot)
				{
					RectTransform parent = a_panel.transform as RectTransform;
					while(parent.parent != null)
					{
						parent = parent.parent as RectTransform;
					}
                    parent.SetParent(_root.transform, false);
				}
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI,"Same panel registered twice. : " + a_panel.gameObject.name);
			}
			
			if(_panelsToLoadCount == 0 && _isLoading)
			{
				_isLoading = false;
				FFEngine.Events.FireEvent(EEventType.UILoadingComplete);
				FFLog.Log(EDbgCat.UI,"UI LOADING COMPLETE");
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
				FFLog.LogWarning(EDbgCat.UI,"No panel registered with this key : " + a_eventKey);
			}
		}
		#endregion
	
		#region Access
		internal FFPanel GetPanel(string a_sceneName)
		{
			if (_panelsByName.ContainsKey (a_sceneName))
			{
				return _panelsByName[a_sceneName];
			}
	
			FFLog.LogWarning(EDbgCat.UI,"No panel registered with this key : " + a_sceneName);
			return null;
		}
		#endregion
	
	
		#region Display
		internal void SwitchToPanels(string[] a_panels, bool a_isForward = true)
		{
			List<string> panelsToShow = new List<string>();
			foreach(string each in a_panels)
			{
				panelsToShow.Add(each);
			}
			
			foreach(string each in _panelsByName.Keys)
			{
				if(!panelsToShow.Contains(each) && 
				   (_panelsByName[each].State == FFPanel.EState.Shown || _panelsByName[each].State == FFPanel.EState.Showing))
				{
					HideSpecificPanel(each, a_isForward);
				}
				else if(panelsToShow.Contains(each) && 
				        (_panelsByName[each].State == FFPanel.EState.Hidden || _panelsByName[each].State == FFPanel.EState.Hidding))
				{
					RequestDisplay(each, a_isForward);
				}
			}
		}
		
		internal void RequestDisplay(string[] a_panels, bool a_isForward = true)
		{
			foreach(string each in a_panels)
				RequestDisplay(each, a_isForward);
		}
		
		internal void RequestDisplay(string a_panel, bool a_isForward = true)
		{
			if(!_panelsByName.ContainsKey(a_panel))
			{
				FFLog.LogWarning(EDbgCat.UI,"No panel found using this name : " + a_panel);
				return;
			}
			
			if(_panelsByName[a_panel].State == FFPanel.EState.Hidden || _panelsByName[a_panel].State == FFPanel.EState.Hidding)
			{
				_panelsByName[a_panel].Show(a_isForward);
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI,"Panel is already shown or showing : " + a_panel);
			}
		}
		
		internal void HideSpecificPanel(string a_panel, bool a_isForward = true)
		{
			if(!_panelsByName.ContainsKey(a_panel))
			{
				FFLog.LogWarning(EDbgCat.UI,"No panel found using this name : " + a_panel);
				return;
			}
			
			if(_panelsByName[a_panel].State == FFPanel.EState.Showing || _panelsByName[a_panel].State == FFPanel.EState.Shown)
			{
				_panelsByName[a_panel].Hide(a_isForward);
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI,"Panel is already hidden or hidding : " + a_panel);
			}
		}
		
		internal void HideAllPanels(bool a_isForward = true)
		{
			foreach (FFPanel each in _panelsByName.Values)
			{
				if(each.State != FFPanel.EState.Hidden && each.State != FFPanel.EState.Hidding)
					each.Hide(a_isForward);
			}
		}
		
		internal bool IsTransitionning
		{
			get
			{
				if(_isLoading)
					return true;
					
				foreach(FFPanel each in _panelsByName.Values)
				{
					if(each.IsTransitionning)
						return true;
				}
				return false;
			}
		}
        #endregion

        #region Popups
        protected FFPopup _currentPopup;
        internal FFPopup CurrentPopup
        {
            get
            {
                return _currentPopup;
            }
        }

        internal void PushPopup(FFPopupData a_data)
        {
            if (_currentPopup != null)
            {
                _pendingPopups.Push(_currentPopup.currentData);
            }
            DismissCurrentPopup();

            _pendingPopups.Push(a_data);
        }

        protected void DisplayNextPopup(FFPopupData a_data)
        {
            //FFPopupData data = _pendingPopups.Dequeue();
            FFPopup popup = _popupsByName[a_data.popupName] as FFPopup;
            popup.SetContent(a_data);
            popup.Show();
            _currentPopup = popup;

            if (FFEngine.Game.CurrentGameMode != null)
            {
                FFEngine.Game.CurrentGameMode.OnLostFocus();
            }
        }

        internal void DismissCurrentPopup()
        {
            if (_currentPopup != null)
            {
                _currentPopup.Hide();
                _currentPopup.onHidden += OnPopupHidden;
            }
        }

        internal void OnPopupHidden()
        {
            _currentPopup.onHidden -= OnPopupHidden;
            _currentPopup = null;

            if (FFEngine.Game.CurrentGameMode != null)
            {
                FFEngine.Game.CurrentGameMode.OnGetFocus();
            }
        }
        #endregion
    }
}