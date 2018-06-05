using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class ActionEditorWindow : EditorWindow
{
    private ActionInfo targetAction;
    private ActionInfo[] actions = default(ActionInfo[]);
    private List<string> actionNames;

    private int currentActionIndex = default(int);
    private int currentFrame = default(int);
    private int numActions = default(int);

    [MenuItem("SKF/Action Editor")]
    public static void ShowWindow()
    {
        GetWindow<ActionEditorWindow>(false, "Actions", true);
    }

    private void OnInspectorUpdate()
    {
#if UNITY_EDITOR
        actions = UnityEditor.AssetDatabase.FindAssets("t:ActionInfo", new string[] { "Assets/_SOS/_ACTIONS" })
                            .Select(guid => UnityEditor.AssetDatabase.GUIDToAssetPath(guid))
                            .Select(path => UnityEditor.AssetDatabase.LoadAssetAtPath<ActionInfo>(path))
                            .Where(b => b).ToArray();
#else
					actions = Resources.FindObjectsOfTypeAll<ActionInfo>();
#endif
        if (actions.Length != numActions)
        {
            numActions = actions.Length;
            foreach (string action in actions.OfType<string>())
                actionNames.Add(action);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("Target Action");
        currentActionIndex = EditorGUILayout.Popup(currentActionIndex, actionNames.ToArray() ?? new string[] { "no actions" });
        targetAction = actions?[currentActionIndex];
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
