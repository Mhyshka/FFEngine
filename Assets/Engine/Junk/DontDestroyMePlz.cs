using UnityEngine;
using System.Collections;

namespace FFEngine
{
	internal class DontDestroyMePlz : MonoBehaviour
	{
		// Use this for initialization
		void Awake ()
		{
			DontDestroyOnLoad (transform.gameObject);
		}
	}
}