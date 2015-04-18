using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ClickEventButton),true)]
public class EventButtonEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		ClickEventButton script = target as ClickEventButton;
		
		GUI.changed = false;
		script.eventType = (EEventType)EditorGUILayout.EnumPopup("Event Type", script.eventType);
		if(script.eventType == EEventType.Custom)
		{
			script.eventKey = EditorGUILayout.TextField("Event Key", script.eventKey);
		}
		
		if (GUI.changed)
		{
			EditorUtility.SetDirty(script);
		}
	}
}
