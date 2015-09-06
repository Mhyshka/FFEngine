using UnityEngine;
using UnityEditor;
using System.Collections;
using FFEngine;

[CustomEditor(typeof(AGameState),true)]
public class StateEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		AGameState script = target as AGameState;
		LoadingState loading = script.GetComponent<LoadingState>();
		
		
		if(loading != null && script != null && script != loading && !(script is ExitState))
		{
			string[] panels = FFUtils.BitMaskToUiScenes(loading.panelsToLoad);
			
			if(panels.Length > 0)
			{
				GUI.changed = false;
				script.panelsToShow = EditorGUILayout.MaskField("Panels to show", script.panelsToShow, panels);
				if (GUI.changed)
					EditorUtility.SetDirty(script);
			}
		}
	}
}
