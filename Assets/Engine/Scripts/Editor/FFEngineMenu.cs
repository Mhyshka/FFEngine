using UnityEditor;
using UnityEngine;
using System.IO;

public class FFEngineMenu : MonoBehaviour
{
	[MenuItem ("FFMenu/Play %#m")]
	static void Play ()
	{
		if(!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.SaveCurrentSceneIfUserWantsTo())
		{
			Debug.Log("Custom Play");

			StreamWriter writer = new StreamWriter("Assets/Editor/lastScene");
			writer.WriteLine(EditorApplication.currentScene);
			writer.Close();
			AssetDatabase.SaveAssets();
			
			EditorApplication.OpenScene("Assets/Engine/Scenes/EntryPoint.unity");
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
			EditorApplication.OpenScene(text);
		}
	}
}