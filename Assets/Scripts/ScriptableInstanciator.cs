using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class ScriptableInstanciator : MonoBehaviour
{
	public bool createIt = false;
	public string className = "ClassName";
	
	void Update()
	{
		if(createIt)
		{
			createIt = false;
			ScriptableObject instance = ScriptableObject.CreateInstance(System.Type.GetType(className));
			
			AssetDatabase.CreateAsset(instance, "Assets/Database/new" + className + ".asset");
			AssetDatabase.SaveAssets();
		}
	}
}
