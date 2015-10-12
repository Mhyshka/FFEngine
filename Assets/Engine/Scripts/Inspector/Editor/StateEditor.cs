using UnityEngine;
using UnityEditor;
using System.Collections;
using FF;

[CustomEditor(typeof(AGameState),true)]
public class StateEditor : Editor
{
	protected bool _folded = true;

    private bool ContainsScene(string[] _toLoad, string a_name)
    {
        foreach (string each in _toLoad)
        {
            if (each == a_name)
                return true;
        }
        return false;
    }
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		AGameState script = target as AGameState;
		LoadingState loading = script.GetComponent<LoadingState>();

        // Loading State
        if (loading == script)
        {
            string[] UIScenes = FFUtils.UiScenes();
            if(GUILayout.Button("Modify Scenes"))
            {
                bool[] scenesChecked = new bool[UIScenes.Length];
                for (int i = 0; i < UIScenes.Length; i++)
                {
                    if (ContainsScene(loading.panelsToLoad, UIScenes[i]))
                        scenesChecked[i] = true;
                    else
                        scenesChecked[i] = false;
                }

                UIPanelPopup popup = UIPanelPopup.Init(loading, UIScenes, scenesChecked);
                //(loading, UIScenes, new bool[UIScenes.Length]);
            }
        }
        /*if(loading == script)
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
		}*/

        // Other states
        if (loading != null && script != null && script != loading && !(script is ExitState))
		{
			if(loading.panelsToLoad.Length > 0)
			{
				GUI.changed = false;
				script.panelsToShow = EditorGUILayout.MaskField("Panels to show", script.panelsToShow, loading.panelsToLoad);
				if (GUI.changed)
				{
					script.panelNamesToShow = FFUtils.BitMaskToPanels(script.panelsToShow, loading.panelsToLoad);
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
