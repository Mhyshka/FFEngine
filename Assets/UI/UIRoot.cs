using UnityEngine;
using System.Collections;

internal class UIRoot : MonoBehaviour
{
	#region Properties
	public bool adaptToScreenRatio = true;
	
	protected RectTransform _rectTransform;
	protected float _baseWidth;
	#endregion
	
	internal void Awake()
	{
		_rectTransform = GetComponent<RectTransform>();
		if(_rectTransform != null)
		{
			_baseWidth = _rectTransform.sizeDelta.x;
			if(adaptToScreenRatio)
			{
				float aspectRatio = Screen.width / (float)Screen.height;
				FFLog.Log("Aspect ratio : " + aspectRatio.ToString());
				SetWidthMultiplier(aspectRatio * 3f / 4f);
			}
		}
	}
	
	
	internal void SetWidthMultiplier(float a_multiplier)
	{
		if(_rectTransform != null)
		{
			float targetWidth = _baseWidth * a_multiplier;
			Vector2 size = _rectTransform.sizeDelta;
			size.x = targetWidth;;
			_rectTransform.sizeDelta = size;
		}
	}
	
	//Button callback
	public void ApplyWidthMultiplier(float a_value)
	{
		SetWidthMultiplier(a_value);
	}
}