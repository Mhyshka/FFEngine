using UnityEngine;
using System.Collections;

namespace FF.UI
{
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(Animator))]
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
		#endregion
	
		#region Properties
		protected CanvasGroup _canvasGroup = null;
		protected Animator _animator = null;
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
	
		protected virtual void Awake()
		{
			#if RELEASE
			debug = false;	
			#endif
			_canvasGroup = GetComponent<CanvasGroup>();
			_animator = GetComponent<Animator>();
			
			if (!hideOnLoad)
			{
				_animator.SetTrigger("Show");
			}

			if(!debug && !(this is LoadingScreen))
			{
				FFEngine.UI.Register (gameObject.name, this);
			}
		}
	
		protected virtual void OnDestroy()
		{
			FFEngine.UI.Unregister (gameObject.name);
		}

		#region Show
		internal virtual void Show()
		{
			if(_state == EState.Hidden || _state == EState.Hidding)
			{
				gameObject.SetActive(true);
				_animator.SetTrigger("Show");
					
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
		internal virtual void Hide()
		{
			if(_state == EState.Shown || _state == EState.Showing)
			{
				_animator.SetTrigger("Hide");
				_state = EState.Hidding;
				FFLog.Log(EDbgCat.UI, "Hiding : " + gameObject.ToString());
			}
			else
			{
				FFLog.LogWarning(EDbgCat.UI, "Already hidden / hiding");
			}
		}
		#endregion
		
		#region Transition Events
		/// <summary>
		/// Callback from animator
		/// </summary>
		public virtual void OnShown()
		{
			FFLog.Log(EDbgCat.UI, "On Shown : " + gameObject.name);
			_state = EState.Shown;
		}
	
		/// <summary>
		/// Callback from animator
		/// </summary>
		public virtual void OnHidden()
		{
			FFLog.Log(EDbgCat.UI, "On Hidden : " + gameObject.name);
			_state = EState.Hidden;
			
			if(gameObject.activeSelf)
				gameObject.SetActive(false);
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