using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CanvasRenderer), true)]
public class CanvasRendererInpsector : Editor
{
    public int current = 0;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CanvasRenderer renderer = target as CanvasRenderer;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Depth:");
        current = EditorGUILayout.IntField(current);
        if (GUILayout.Button("SetDepth"))
        {
            renderer.transform.SetSiblingIndex(current);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("BringToFront"))
        {
            renderer.transform.SetAsFirstSibling();
            current = Selection.activeTransform.GetSiblingIndex();
        }

        if (GUILayout.Button("BringToBack"))
        {
            renderer.transform.SetAsLastSibling();
            current = Selection.activeTransform.GetSiblingIndex();
        }
        EditorGUILayout.EndHorizontal();
    }

}
