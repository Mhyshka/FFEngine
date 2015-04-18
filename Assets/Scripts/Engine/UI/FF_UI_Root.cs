using UnityEngine;
using System.Collections;

internal class FF_UI_Root : MonoBehaviour
{
	internal void Awake()
	{
		FFEngine.UI.root = this;
	}
}
