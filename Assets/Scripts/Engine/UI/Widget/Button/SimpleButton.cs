using UnityEngine;
using System.Collections;

internal class SimpleButton : MonoBehaviour
{
	internal delegate void Callback();
	
	internal Callback callback;
	
	void OnClick()
	{
		callback.Invoke();
	}
}
