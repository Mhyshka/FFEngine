using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace FF
{
	public class FFUtils
	{
        public static string[] UiScenes()
        {
#if UNITY_EDITOR
            Regex rootPath = new Regex(Regex.Escape("Assets/Scenes/")); /*+ Regex.Unescape(".+$")*/
            Regex ui = new Regex(".+(Panel)|(Toast)|(Popup)|(Screen)\\.unity");

            int size = 0;
            string[] allScenes = new string[UnityEditor.EditorBuildSettings.scenes.GetLength(0)];
            for (int i = 0; i < allScenes.Length; i++)
            {
                allScenes[i] = UnityEditor.EditorBuildSettings.scenes[i].path;
                string[] split = rootPath.Split(allScenes[i]);
                if (split.Length > 1)
                {
                    allScenes[i] = split[1];
                    if (!ui.IsMatch(allScenes[i]) || !UnityEditor.EditorBuildSettings.scenes[i].enabled)
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

            string[] scenes = new string[size];
            int j = 0;
            for (int i = 0; i < allScenes.Length; i++)
            {
                if (allScenes[i] != null)
                {
                    string[] split = allScenes[i].Split('/');
                    string scene = split[split.Length - 1];
                    string[] test = { "." };
                    scene = scene.Split(test, System.StringSplitOptions.RemoveEmptyEntries)[0];
                    scenes[j] = scene;
                    j++;
                }
            }

            return scenes;
#else
            return null;
#endif
        }


		public static string[] BitMaskToUiScenes(int a_bitMask)
		{
	#if UNITY_EDITOR
			Regex rootPath = new Regex(Regex.Escape("Assets/Scenes/")); /*+ Regex.Unescape(".+$")*/
			Regex ui = new Regex(".+(Panel)|(Toast)|(Popup)|(Screen)\\.unity");
			
			int size = 0;
			string[] allScenes = new string[UnityEditor.EditorBuildSettings.scenes.GetLength(0)];
			for(int i = 0 ; i < allScenes.Length ; i++)
			{
				allScenes[i] = UnityEditor.EditorBuildSettings.scenes[i].path;
				string[] split = rootPath.Split(allScenes[i]);
				if(split.Length > 1)
				{
					allScenes[i] = split[1];
					if(!ui.IsMatch(allScenes[i]) || !UnityEditor.EditorBuildSettings.scenes[i].enabled || ((1 << size) & a_bitMask) != (1 << size))
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
			
			string[] scenes = new string[size];
			int j = 0;
			for(int i = 0 ; i < allScenes.Length ; i++)
			{
				if(allScenes[i] != null)
				{
					string[] split = allScenes[i].Split('/');
					string scene = split[split.Length-1];
					string[] test = {"."};
					scene = scene.Split(test, System.StringSplitOptions.RemoveEmptyEntries)[0];
					scenes[j] = scene;
					j++;
				}
			}
			
			return scenes;
	#else
			return null;
	#endif
		}
		
		public static string[] BitMaskToPanels(int a_bitMask, string[] a_loadedPanels)
		{
			int size = 0;
			string[] valids = new string[a_loadedPanels.Length];
			for(int i = 0 ; i < valids.Length ; i++)
			{
				valids[i] = a_loadedPanels[i];
				if(((1 << i) & a_bitMask) != (1 << i))
				{
					valids[i] = null;
				}
				else
					size++;
			}
			
			string[] scenes = new string[size];
			int j = 0;
			for(int i = 0 ; i < valids.Length ; i++)
			{
				if(valids[i] != null)
				{
					string[] split = valids[i].Split('/');
					string scene = split[split.Length-1];
					string[] test = {"."};
					scene = scene.Split(test, System.StringSplitOptions.RemoveEmptyEntries)[0];
					scenes[j] = scene;
					j++;
				}
			}
			
			return scenes;
		}
		
		public static int BitWiseMask(int aMask, string[] aValues)
		{
			int[] values = new int[aValues.Length];
			for(int i = 0; i < aValues.Length; i++)
			{
				values[i] = 1 << i;
			}
			
			int val = aMask;
			int maskVal = 0;
			for(int i = 0; i < aValues.Length; i++)
			{
				if (values[i] != 0)
				{
					if ((val & values[i]) == values[i])
						maskVal |= 1 << i;
				}
				else if (val == 0)
					maskVal |= 1 << i;
			}
			
			return maskVal;
		}
		
		public static float Rerange(float a_value,
									Vector2 baseRange,
									Vector2 targetRange)
		{	
			float ratio = Mathf.Clamp01(Mathf.InverseLerp(baseRange.x, baseRange.y, a_value));
			return Mathf.Lerp (targetRange.x, targetRange.y, ratio);
		}
		
		public static Vector3 SuperSmoothLerp( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float time, float speed )
		{
			Vector3 f = pastPosition - pastTargetPosition + (targetPosition - pastTargetPosition) / (speed * time);
			return targetPosition - (targetPosition - pastTargetPosition) / (speed*time) + f * Mathf.Exp(-speed*time);
		}
	}
}
