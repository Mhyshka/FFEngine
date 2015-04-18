using UnityEngine;
using System.Collections;

internal class DontDestroyMePlz : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
}
