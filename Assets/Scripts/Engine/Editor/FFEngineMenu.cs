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
			//GameConstants.PREVIOUS_EDITED_SCENE = EditorApplication.currentScene;
			EditorApplication.OpenScene("Assets/Scenes/Root.unity");
			EditorApplication.isPlaying = true;
		}
	}
	
	[MenuItem ("FFMenu/QuickLoad %#k")]
	static void QuickLoad ()
	{
		/*Debug.Log("Quick Load");
		if(!string.IsNullOrEmpty(GameConstants.PREVIOUS_EDITED_SCENE))
		{
			EditorApplication.isPlaying = false;
			EditorApplication.OpenScene(GameConstants.PREVIOUS_EDITED_SCENE);
			GameConstants.PREVIOUS_EDITED_SCENE = "";
		}*/
	}
}