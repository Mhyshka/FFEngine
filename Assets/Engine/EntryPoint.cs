using UnityEngine;
using System.Collections;

namespace FFEngine
{
	public class EntryPoint : MonoBehaviour
	{
		private FFEngine _engine;
	
		// Use this for initialization
		void Awake()
		{
			_engine = new FFEngine();
			FFEngine.Initialize();
		}
		
		void Update()
		{
			FFEngine.DoUpdate();
		}
	}
}