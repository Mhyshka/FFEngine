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
		/// <summary>
		/// Returns yes if the device is a TV OS.
		/// </summary>
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
		
		#region Cast
		/// <summary>
		/// Returns yes if the device is currently casting on a screen. ( Chromecast / Airplay )
		/// </summary>
		internal bool IsCasting
		{
			get
			{
				return false;
			}
		}
		#endregion
		
		/// <summary>
		/// Returns yes if the device is either a TV or casting on a screen.
		/// </summary>
		internal bool UseTV
		{
			get
			{
				return IsTV || IsCasting;
			}
		}
	}
}