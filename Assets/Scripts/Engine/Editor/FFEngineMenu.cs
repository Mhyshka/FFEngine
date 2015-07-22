using UnityEditor;
using UnityEngine;

public class FFEngineMenu : MonoBehaviour
{
 	

	[MenuItem ("FFMenu/Play %#m")]
	static void Play ()
	{
		if(!EditorApplication.isPlayingOrWillChangePlaymode && EditorApplication.SaveCurrentSceneIfUserWantsTo())
		{
			Debug.Log("Custom Play");
			
			/*TextAsset text = new TextAsset();
			AssetDatabase.CreateAsset(text, Application.dataPath + "/Editor/LastScene.txt");
			text.text = EditorApplication.currentScene;
			AssetDatabase.SaveAssets();*/
			
			EditorApplication.OpenScene("Assets/Scenes/Root.unity");
			EditorApplication.isPlaying = true;
		}
	}
	
	[MenuItem ("FFMenu/QuickLoad %#k")]
	static void QuickLoad ()
	{
		/*EditorApplication.isPlaying = false;
		TextAsset lastScene = (TextAsset)AssetDatabase.LoadAssetAtPath(Application.dataPath + "/Editor/LastScene.txt",typeof(TextAsset));
		if(lastScene != null)
		{
			EditorApplication.OpenScene(lastScene.text);
		}*/
	}
}