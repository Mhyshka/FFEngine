using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
using System.Collections;

using FF.UI;
using FF;

[CustomEditor(typeof(FFButtonEvent), true)]
public class FFButtonMessageEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		FFButtonEvent script = target as FFButtonEvent;
		GUI.changed = false;
		
		script.eventType = (FFEventType)EditorGUILayout.EnumPopup("Event type",script.eventType);
		
		if(script.eventType == FFEventType.Custom)
		{
			EditorGUI.indentLevel++;
			script.eventKey = EditorGUILayout.TextField("Event Key",script.eventKey);
		}
		
		if(GUI.changed)
		{
			EditorUtility.SetDirty(script);
			EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
		}
	}
}
