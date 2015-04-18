using UnityEngine;
using System.Text.RegularExpressions;

public class BitMaskUIScenesAttribute : PropertyAttribute
{
	public string[] scenes;
	public BitMaskUIScenesAttribute()
	{
		Regex rootPath = new Regex(Regex.Escape("Assets/Scenes/")); /*+ Regex.Unescape(".+$")*/
		Regex ui = new Regex(".+(Panel)|(Popup)|(Screen)\\.unity");
		
		int size = 0;
		string[] allScenes = new string[UnityEditor.EditorBuildSettings.scenes.GetLength(0)];
		for(int i = 0 ; i < allScenes.Length ; i++)
		{
			allScenes[i] = UnityEditor.EditorBuildSettings.scenes[i].path;
			allScenes[i] = rootPath.Split(allScenes[i])[1];
			if(!ui.IsMatch(allScenes[i]) || !UnityEditor.EditorBuildSettings.scenes[i].enabled)
			{
				allScenes[i] = null;
			}
			else
				size++;
		}
		
		scenes = new string[size];
		int j = 0;
		for(int i = 0 ; i < allScenes.Length ; i++)
		{
			if(allScenes[i] != null)
			{
				scenes[j] = allScenes[i];
				j++;
			}
		}
	}
}