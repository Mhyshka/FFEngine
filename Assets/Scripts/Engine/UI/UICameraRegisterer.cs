using UnityEngine;
using System.Collections;

internal class UICameraRegisterer : MonoBehaviour
{
	internal void Awake()
	{
		FFEngine.UI.RegisterUiCamera(GetComponent<Camera>());
	}
}
