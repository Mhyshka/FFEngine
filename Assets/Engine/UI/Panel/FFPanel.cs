using UnityEngine;
using System.Collections;

namespace FFEngine
{
	[RequireComponent(typeof(CanvasGroup))]
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
		public bool isUsingTransition = true;
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
		
		internal EState state = EState.Hidden;
		#endregion
	
		protected virtual void Awake()
		{
			_canvasGroup = GetComponent<CanvasGroup>();
			_animator = GetComponent<Animator>();
			if(!debug)
			{
				FFEngine.UI.Register (gameObject.name, this);
				if (hideOnLoad)
					OnHidden ();
				else
					OnShown ();
			}
		}
	
		protected virtual void OnDestroy()
		{
			FFEngine.UI.Unregister (gameObject.name);
		}

		#region Show
		internal virtual void Show()
		{
			if(!hasTweenAlpha && _canvasGroup != null)
				_canvasGroup.alpha = 1f;
			if(isUsingTransition)
				PlayShowTransition ();
			else
				OnShown();
		}
		
		protected virtual void PlayShowTransition ()
		{
			//Debug.Log("Showing : " + gameObject.name);
			if (state == EState.Hidden)
			{
				state = EState.Showing;
				tweens.PlayForward ();
			}
			else if (state == EState.Hidding)
			{
				state = EState.Showing;
				tweens.Toggle ();
			}
		}
		#endregion
	
		#region Hide
		internal virtual void Hide()
		{
			if(isUsingTransition)
				PlayHideTransition ();
			else
				OnHidden();
		}
	
		protected virtual void PlayHideTransition ()
		{
			if (state == EState.Shown)
			{
				state = EState.Hidding;
				tweens.PlayReverse ();
			}
			else if (state == EState.Showing)
			{
				state = EState.Hidding;
				tweens.Toggle ();
			}
		}
		#endregion
		
		#region Transition Events
		public void OnTransitionComplete()
		{
			if (state == EState.Showing || state == EState.Hidden)
			{
				OnShown();
			}
			else if (state == EState.Hidding || state == EState.Shown)
			{
				OnHidden();
			}
		}
	
		internal virtual void OnShown()
		{
			//Debug.Log("On Shown : " + gameObject.name);
			state = EState.Shown;
		}
	
		internal virtual void OnHidden()
		{
			//Debug.Log("On Hidden : " + gameObject.name);
			state = EState.Hidden;
			if(_canvasGroup != null)
				_canvasGroup.alpha = 0f;
		}
		#endregion
	}
}