using UnityEngine;
using UnityEditor;
using System.Collections;
using FF;

[CustomEditor(typeof(AGameState),true)]
public class StateEditor : Editor
{
	protected bool _folded = true;
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		AGameState script = target as AGameState;
		LoadingState loading = script.GetComponent<LoadingState>();
		
		// Loading State
		if(loading == script)
		{
			loading.panelNamesToLoad = FFUtils.BitMaskToUiScenes(loading.panelsToLoad);
			
			_folded = EditorGUILayout.Foldout(_folded,"Debug Scenes name");
			if(_folded)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical();
				foreach(string each in loading.panelNamesToLoad)
				{
					EditorGUILayout.LabelField(each);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
		}
		
		// Other states
		if(loading != null && script != null && script != loading && !(script is ExitState))
		{
			script.panelNamesToShow = FFUtils.BitMaskToPanels(script.panelsToShow, loading.panelNamesToLoad);

			if(loading.panelNamesToLoad.Length > 0)
			{
				GUI.changed = false;
				script.panelsToShow = EditorGUILayout.MaskField("Panels to show", script.panelsToShow, loading.panelNamesToLoad);
				if (GUI.changed)
				{
					script.panelNamesToShow = FFUtils.BitMaskToPanels(script.panelsToShow, loading.panelNamesToLoad);
					EditorUtility.SetDirty(script);
					EditorApplication.MarkSceneDirty();
				}
			}
			
			_folded = EditorGUILayout.Foldout(_folded,"Debug Scenes name");
			if(_folded)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.BeginVertical();
				foreach(string each in script.panelNamesToShow)
				{
					EditorGUILayout.LabelField(each);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.indentLevel--;
			}
		}
	}
}
