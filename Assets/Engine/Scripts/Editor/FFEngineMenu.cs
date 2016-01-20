using UnityEditor;
using UnityEngine;
using System.IO;

using UnityEditor.SceneManagement;

public class FFEngineMenu : MonoBehaviour
{
	[MenuItem ("FFMenu/Play %#m")]
	static void Play ()
	{
		if(!EditorApplication.isPlayingOrWillChangePlaymode && EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			Debug.Log("Custom Play");

			StreamWriter writer = new StreamWriter("Assets/Editor/lastScene");
			writer.WriteLine(EditorSceneManager.GetActiveScene().path);
			writer.Close();
			AssetDatabase.SaveAssets();

            EditorSceneManager.OpenScene("Assets/Scenes/EntryPoint.unity");
			EditorApplication.isPlaying = true;
		}
	}
	
	[MenuItem ("FFMenu/QuickLoad %#k")]
	static void QuickLoad ()
	{
		Debug.Log("Custom Load");
		
		EditorApplication.isPlaying = false;
		StreamReader reader = new StreamReader("Assets/Editor/lastScene");
		string text = reader.ReadLine();
		reader.Close();
		if(!string.IsNullOrEmpty(text))
		{
			EditorSceneManager.OpenScene(text);
		}
	}
}