using UnityEngine;
using System.Collections;

namespace FFEngine
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
			
			if (!hideOnLoad)
			{
				_animator.SetInteger("StartingState", 1);
			}
			else
			{
				_animator.SetInteger("StartingState", -1);
			}
			
			if(!debug)
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
			if(state == EState.Shown || state == EState.Showing)
			{
				_animator.SetTrigger("Show");
				state = EState.Showing;
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
			if(state == EState.Shown || state == EState.Showing)
			{
				_animator.SetTrigger("Hide");
				state = EState.Hidding;
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
			state = EState.Shown;
		}
	
		/// <summary>
		/// Callback from animator
		/// </summary>
		public virtual void OnHidden()
		{
			FFLog.Log(EDbgCat.UI, "On Hidden : " + gameObject.name);
			state = EState.Hidden;
		}
		#endregion
	}
}