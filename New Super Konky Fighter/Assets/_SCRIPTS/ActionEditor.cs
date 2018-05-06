using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ActionEditorWindow : EditorWindow
{
    private ActionInfo targetAction;
    private ActionInfo[] actions = new ActionInfo[0];
    private string[] actionNames;
    private int currentActionIndex = 0;
    private int currentFrame = 0;

    [MenuItem("SKF/Action Editor")]
    public static void ShowWindow()
    {
        GetWindow<ActionEditorWindow>(false, "Actions", true);
    }

    private void OnInspectorUpdate()
    {
        actions = Resources.FindObjectsOfTypeAll<ActionInfo>();
        actionNames = new string[actions.Length];
        for (int i = 0; i < actions.Length; ++i)
        {
            actionNames[i] = actions[i].name;
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Target Action");
        currentActionIndex = EditorGUILayout.Popup(currentActionIndex, actionNames);
        targetAction = actions[currentActionIndex];
        EditorGUILayout.EndVertical();

        //EditorGUI.DrawRect

        GUILayout.FlexibleSpace();


        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();

        if (targetAction != null)
        {
            currentFrame = EditorGUILayout.IntSlider(currentFrame, 0, targetAction.getFrameCount());
            if (GUILayout.Button("<", GUILayout.Width(25), GUILayout.Height(25)))
            {
                --currentFrame;
            }
            if (GUILayout.Button(">", GUILayout.Width(25), GUILayout.Height(25)))
            {
                ++currentFrame;
            }
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(25)))
        {
            
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

}
