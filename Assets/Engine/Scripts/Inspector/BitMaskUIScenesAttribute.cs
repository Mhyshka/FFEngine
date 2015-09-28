using UnityEngine;
using System.Text.RegularExpressions;

namespace FF
{
	public class BitMaskUIScenesAttribute : PropertyAttribute
	{
		public string[] scenes;
		
		
		public BitMaskUIScenesAttribute()
		{
			#if UNITY_EDITOR
			Regex rootPath = new Regex(Regex.Escape("Assets/Scenes/")); /*+ Regex.Unescape(".+$")*/
			Regex ui = new Regex(".+(Panel)|(Popup)|(Screen)\\.unity");
			
			int size = 0;
			string[] allScenes = new string[UnityEditor.EditorBuildSettings.scenes.GetLength(0)];
			for(int i = 0 ; i < allScenes.Length ; i++)
			{
				allScenes[i] = UnityEditor.EditorBuildSettings.scenes[i].path;
				string[] split = rootPath.Split(allScenes[i]);
				if(split.Length > 1)
				{
					allScenes[i] = split[1];
					if(!ui.IsMatch(allScenes[i]) || !UnityEditor.EditorBuildSettings.scenes[i].enabled)
					{
						allScenes[i] = null;
					}
					else
						size++;
				}
				else
				{
					allScenes[i] = null;
				}
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
			#endif
		}
	}
}