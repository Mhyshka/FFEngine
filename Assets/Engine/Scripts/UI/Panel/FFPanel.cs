using UnityEngine;

namespace FF.UI
{
    internal delegate void PanelCallback(FFPanel a_panel);
	internal class FFPanel : MonoBehaviour
	{
		internal enum EState
		{
			Hidden,
			Showing,
			Shown,
			Hidding
		}
	
		#region Inspector Properties
		public bool debug = false;
		public bool hideOnLoad = true;
        public Animator animator = null;
        /*public TweenerGroup forwardTweeners = null;
        public TweenerGroup backwardTweeners = null;*/

        public UIKeyNavigation defaultSelectedWidget = null;
		#endregion
	
		#region Properties
		//protected Dictionary<Selectable, Navigation.Mode> _selectables = null;
		internal virtual bool ShouldMoveToRoot
		{
			get
			{
				return true;
			}
		}
		
		protected EState _state = EState.Hidden;
		internal EState State
		{
			get
			{
				return _state;
			}
		}
		#endregion

        protected virtual bool NeedsTobeRegister
        {
            get
            {
                return true;
            }
        }
	
		protected virtual void Awake()
		{
#if RELEASE
			debug = false;	
#endif
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            if (!hideOnLoad)
            {
                if (animator != null)
                {
                    animator.SetTrigger("Show");
                }
            }

            if (!debug && NeedsTobeRegister)
            {
                Engine.UI.Register(gameObject.name, this);
            }
        }
	
		protected virtual void OnDestroy()
		{
            if(NeedsTobeRegister)
			    Engine.UI.Unregister (gameObject.name);
		}

		#region Show
		internal virtual void Show(bool a_isForward = true)
		{
			if(_state == EState.Hidden || _state == EState.Hidding)
			{
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }

                if (animator != null)
                {
                    animator.SetBool("Forward", a_isForward);
                    animator.SetTrigger("Show");
                }
					
				_state = EState.Showing;
				FFLog.Log(EDbgCat.UI, "Showing : " + gameObject.ToString());
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI, "Already shown / showing");
			}
		}
		#endregion
	
		#region Hide
		internal virtual void Hide(bool a_isForward = true)
		{
            if (_state == EState.Shown || _state == EState.Showing)
			{
                if (animator != null)
                {             
                    animator.SetBool("Forward", a_isForward);
                    animator.SetTrigger("Hide");
                }

                _state = EState.Hidding;
				FFLog.Log(EDbgCat.UI, "Hiding : " + gameObject.ToString());
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI, "Already hidden / hiding");
			}
		}
        #endregion

        internal void TrySelectWidget()
        {
            if (defaultSelectedWidget != null && (debug || Engine.Inputs.ShouldUseNavigation))
                defaultSelectedWidget.OnNavigate(KeyCode.None);
        }

        #region Transition Events
        internal PanelCallback onShown = null;
        /// <summary>
        /// Callback from animator or tweeners
        /// </summary>
        public virtual void OnShown()
		{
            FFLog.Log(EDbgCat.UI, "On Shown : " + gameObject.name);
			_state = EState.Shown;
            TrySelectWidget();

            if (onShown != null)
                onShown(this);
        }

        internal PanelCallback onHidden = null;
        /// <summary>
        /// Callback from animator or tweeners
        /// </summary>
        public virtual void OnHidden()
		{
            if (animator != null)
            {
                if (animator.GetBool("Show"))
                {
                    return;
                }
            }

            FFLog.Log(EDbgCat.UI, "On Hidden : " + gameObject.name);
            _state = EState.Hidden;

            if(gameObject.activeSelf)
                gameObject.SetActive(false);

            if (onHidden != null)
                onHidden(this);
        }
		
		internal bool IsTransitionning
		{
			get
			{
				return _state == EState.Showing || _state == EState.Hidding;
			}
		}
		#endregion
	}
}