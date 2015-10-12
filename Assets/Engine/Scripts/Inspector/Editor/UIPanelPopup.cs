using UnityEngine;
using UnityEditor;
using System.Collections;
using FF;

public class UIPanelPopup : EditorWindow
{
    protected LoadingState _loading;
    protected string[] _uiScenes;
    protected bool[] _checkedScenes;

    protected Rect windowRect = new Rect(100, 100, 200, 200);

    public static UIPanelPopup Init(LoadingState a_state, string[] a_uiScenes, bool[] a_checked)
    {
        UIPanelPopup window = new UIPanelPopup(a_state, a_uiScenes, a_checked);
        //window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 50);
        window.Show(true);
        return window;
    }

    public UIPanelPopup(LoadingState a_state, string[] a_uiScenes, bool[] a_checked)
    {
        _loading = a_state;
        _uiScenes = a_uiScenes;
        _checkedScenes = a_checked;
    }

    void DoWindow(int a_unusedWindow)
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleLeft;

        //GUIStyle styleCheck = new GUIStyle();
        //styleCheck.fixedWidth = 50f;

        GUILayout.BeginVertical();

        //Header
        GUILayout.BeginHorizontal();
        GUILayout.Label("Load");
        GUILayout.Label("Scene name");
        GUILayout.EndVertical();

        for (int i = 0; i < _uiScenes.Length; i++)
        {
            //Row
            GUILayout.BeginHorizontal();
            
            _checkedScenes[i] = GUILayout.Toggle(_checkedScenes[i], "");
            EditorGUILayout.LabelField(_uiScenes[i], style);
            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Modify!"))
        {
            int count = 0;
            foreach (bool each in _checkedScenes)
            {
                if(each)
                    count++;
            }

            string[] result = new string[count];
            int current = 0;
            for (int i = 0; i < _uiScenes.Length; i++)
            {
                if (_checkedScenes[i])
                {
                    result[current] = _uiScenes[i];
                    current++;
                }
            }

            _loading.panelsToLoad = result;

            Close();
        }

        GUILayout.EndVertical();
    }

    void OnGUI()
    {
        BeginWindows();

        // All GUI.Window or GUILayout.Window must come inside here
        DoWindow(1);
        //windowRect = GUILayout.Window(1, windowRect, DoWindow, "Hi There");

        EndWindows();
        
    }
}