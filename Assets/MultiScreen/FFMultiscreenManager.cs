using UnityEngine;
using System.Collections;

using FF.Utils;

namespace FF.Multiscreen
{
	internal class FFMultiscreenManager
	{
		#region Properties
		protected bool _isTV;
		#endregion

		internal FFMultiscreenManager()
		{
			_isTV = RefreshIsTV();
		}
		
		#region TV
		internal bool IsTV
		{
			get
			{
				return _isTV;
			}
		}
		
		protected bool RefreshIsTV()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			return FFAndroidUtils.IsAndroidTV();
			#else
			return false;
			#endif
		}
		#endregion
	}
}