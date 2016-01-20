using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FF.UI
{
    

	internal class UIManager : BaseManager
	{
        internal static int POPUP_PRIO_HIGH = 2;
        internal static int POPUP_PRIO_MEDIUM = 1;
        internal static int POPUP_PRIO_LOW = 0;

        #region Properties
        internal UIRoot _root;
        internal Camera UiCamera
        {
            get
            {
                return _root.uiCamera; ;
            }
        }
		private LoadingScreen _loadingScreen;
		
		private Dictionary<string, FFPanel> _panelsByName;
        #endregion

        #region Popups
        private Dictionary<string, FFPopup> _popupsByName;
        private LinkedList<FFPopupData>[] _pendingPopups;
        protected FFPopup _currentPopup;
        internal FFPopup CurrentPopup
        {
            get
            {
                return _currentPopup;
            }
        }

        protected int _latestPopupId = 0;
        internal int NextPopupId
        {
            get
            {
                _latestPopupId++;
                return _latestPopupId;
            }
        }
        #endregion

        #region Toasts
        private Dictionary<string, FFToast> _toastsByName;
        private Queue<FFToastData> _pendingToasts;
        protected FFToast _currentToast;
        #endregion

        #region Manager
        internal UIManager()
		{
			_panelsByName = new Dictionary<string, FFPanel> ();

            _popupsByName = new Dictionary<string, FFPopup>();
            _pendingPopups = new LinkedList<FFPopupData>[3];
            _pendingPopups[POPUP_PRIO_LOW] = new LinkedList<FFPopupData>();
            _pendingPopups[POPUP_PRIO_MEDIUM] = new LinkedList<FFPopupData>();
            _pendingPopups[POPUP_PRIO_HIGH] = new LinkedList<FFPopupData>();

            _toastsByName = new Dictionary<string, FFToast>();
            _pendingToasts = new Queue< FFToastData>();

            _loadingOperations = new List<AsyncOperation>();
        }

        internal override void DoUpdate()
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (!Engine.Inputs.ShouldUseNavigation && selected != null)
            {
                if(selected.GetComponent<FFWidgetFocusKeeper>() == null)
                    EventSystem.current.SetSelectedGameObject(null);
            }

            if (_currentPopup == null && PendingPopupCount > 0)
            {
                FFPopupData data = NextPopupToDisplay;
                DisplayPopup(data);
            }

            if (_currentToast == null && _pendingToasts.Count > 0)
            {
                DisplayNextToast(_pendingToasts.Dequeue());
            }
        }

        internal override void TearDown()
        {
            
        }
        #endregion

        #region Loading
        protected List<AsyncOperation> _loadingOperations;

        internal void LoadPanelsSet(string[] a_panelList)
		{
			if(a_panelList != null && a_panelList.Length > 0)
			{
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
				FFLog.Log(EDbgCat.UI,"No panel to load, UIManager is done.");
				return;
			}
		}
		
		internal void LoadAsyncScene(string a_sceneName)
		{
            _loadingOperations.Add(SceneManager.LoadSceneAsync(a_sceneName, LoadSceneMode.Additive));
		}
		
		internal void ClearPanels()
		{
			foreach(FFPanel each in _panelsByName.Values)
			{
				GameObject.Destroy (each.gameObject);
			}
            foreach (FFPopup each in _popupsByName.Values)
            {
                GameObject.Destroy(each.gameObject);
            }
            foreach (FFToast each in _toastsByName.Values)
            {
                GameObject.Destroy(each.gameObject);
            }
        }

        internal float UILoadingFactor
        {
            get
            {
                return 5f * _loadingOperations.Count;
            }
        }

        internal float UILoadingProgress
        {
            get
            {
                float progress = 0f;
                foreach(AsyncOperation each in _loadingOperations)
                {
                    progress += each.progress;
                }

                if (_loadingOperations.Count > 0)
                    progress /= _loadingOperations.Count;

                return progress;
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

        internal LoadingScreen LoadingScreen
        {
            get
            {
                return _loadingScreen;
            }
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
		
		internal void Register(string a_panelName, FFPanel a_panel)
		{
            //POPUPS
            if (a_panel is FFPopup && !_popupsByName.ContainsKey(a_panelName))
            {
                _popupsByName.Add(a_panelName, (FFPopup)a_panel);
            }
            //TOASTS
            else if (a_panel is FFToast && !_toastsByName.ContainsKey(a_panelName))
            {
                _toastsByName.Add(a_panelName, (FFToast)a_panel);
            }
            //PANELS
            else if (!_panelsByName.ContainsKey(a_panelName))
            {
                _panelsByName.Add(a_panelName, a_panel);
            }
            else
            {
                FFLog.LogWarning(EDbgCat.UI, "Same panel registered twice. : " + a_panel.gameObject.name);
            }

            if (a_panel.ShouldMoveToRoot)
            {
                a_panel.Canvas.worldCamera = UiCamera;
                RectTransform parent = a_panel.transform as RectTransform;
                while (parent.parent != null)
                {
                    parent = parent.parent as RectTransform;
                }
                parent.SetParent(_root.transform, false);
            }
		}
		
		internal void Unregister(string a_eventKey)
		{
			if (_panelsByName.ContainsKey (a_eventKey))
			{
				_panelsByName.Remove(a_eventKey);
			}
            else if (_popupsByName.ContainsKey(a_eventKey))
            {
                _popupsByName.Remove(a_eventKey);
            }
            else if (_toastsByName.ContainsKey(a_eventKey))
            {
                _toastsByName.Remove(a_eventKey);
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
				if(UILoadingProgress != 1f)
					return true;
					
				foreach(FFPanel each in _panelsByName.Values)
				{
					if(each.IsTransitionning)
						return true;
				}

                foreach (FFPopup each in _popupsByName.Values)
                {
                    if (each.IsTransitionning)
                        return true;
                }
                return false;
			}
		}
        #endregion

        #region Popups
        internal bool HasCurrentActivePopup
        {
            get
            {
                return _currentPopup != null && _currentPopup.State != FFPanel.EState.Hidding;
            }
        }

        internal void PushPopup(FFPopupData a_data)
        {
            if (HasCurrentActivePopup)
            {
                if (CurrentPopup.currentData.priority > a_data.priority)
                {
                    _pendingPopups[CurrentPopup.currentData.priority].AddLast(_currentPopup.currentData);
                    DismissCurrentPopup();
                }
            }
            _pendingPopups[a_data.priority].AddLast(a_data);
        }

        protected void DisplayPopup(FFPopupData a_data)
        {
            FFPopup popup = _popupsByName[a_data.popupName] as FFPopup;
            popup.SetContent(a_data);
            popup.Show();
            _currentPopup = popup;

            if (Engine.Game.CurrentGameMode != null && Engine.Game.CurrentGameMode.HasFocus)
            {
                Engine.Game.CurrentGameMode.OnLostFocus();
            }
        }

        internal void DismissPopup(int a_id)
        {
            //FFLog.LogError("Try Dismissing popup : " + a_id);
            if (HasCurrentActivePopup && _currentPopup.currentData.id == a_id)
            {
                DismissCurrentPopup();
            }

            for (int i = 0; i < 3; i++)
            {
                LinkedListNode<FFPopupData> _current = _pendingPopups[i].Last;
                while (_current != null)
                {
                    if (_current.Value.id == a_id)
                    {
                        _pendingPopups[i].Remove(_current);
                        return;
                    }
                    _current = _current.Previous;
                }
            }
        }

        internal void DismissCurrentPopup()
        {
            //FFLog.LogError("Try Dismissing current popup");
            if (HasCurrentActivePopup)
            {
                //FFLog.LogError("Dismissing current popup");
                _currentPopup.onHidden += OnPopupHidden;
                _currentPopup.Hide(false);
            }
        }

        protected void OnPopupHidden(FFPanel a_panel)
        {
            //FFLog.LogError("On popup hidden.");
            _currentPopup.onHidden -= OnPopupHidden;
            _currentPopup = null;

            if (PendingPopupCount == 0 && Engine.Game.CurrentGameMode != null)
            {
                Engine.Game.CurrentGameMode.OnGetFocus();
            }
        }

        protected FFPopupData NextPopupToDisplay
        {
            get
            {
                FFPopupData popup = null;

                for (int i = 2; i >= 0; i--)
                {
                    if (_pendingPopups[i].Count > 0)
                    {
                        popup = _pendingPopups[i].Last.Value;
                        _pendingPopups[i].RemoveLast();
                        break;
                    }
                }

                return popup;
            }
        }

        protected int PendingPopupCount
        {
            get
            {
                int total = 0;

                for (int i = 0; i < 3; i++)
                {
                    total += _pendingPopups[i].Count;
                }
                return total;
            }
        }
        #endregion

        #region Toasts
        internal void PushToast(FFToastData a_data)
        {
            _pendingToasts.Enqueue(a_data);
        }

        protected void DisplayNextToast(FFToastData a_data)
        {
            //FFLog.LogError("Displaying popup : " + a_data.id);
            FFToast toast = _toastsByName[a_data.toastName] as FFToast;
            toast.SetContent(a_data);
            toast.Show();
            toast.onHidden += OnToastHidden;
            _currentToast = toast;
        }

        protected void OnToastHidden(FFPanel a_panel)
        {
            _currentToast.onHidden -= OnToastHidden;
            _currentToast = null;
        }
        #endregion
    }
}