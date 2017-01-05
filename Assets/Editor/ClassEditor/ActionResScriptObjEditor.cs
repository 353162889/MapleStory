using UnityEditor;
using UnityEngine;
using Game;

[CustomEditor(typeof(ActionResScriptObj))]
public class ActionResScriptObjEditor : Editor
{
    private bool isShowing;
    private bool[] foldOuts;
    private bool[][] frameFoldOuts;

    void OnEnable()
    {
        if (foldOuts != null && frameFoldOuts != null)
            return;
        ActionResScriptObj obj = (ActionResScriptObj)target;
        if (obj == null)
            return;
        int foldOutCount = obj.actionInfos == null ? 0 : obj.actionInfos.Count;
        foldOuts = new bool[foldOutCount];

        frameFoldOuts = new bool[foldOutCount][];
        for (int i = 0; i < foldOutCount; i++)
        {
            frameFoldOuts[i] = new bool[obj.actionInfos.ListValues[i].frameInfos.Length];
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ActionResScriptObj obj = (ActionResScriptObj)target;
        if (obj.actionInfos != null && obj.actionInfos.Count != 0)
        {
            for (int i = 0; i < obj.actionInfos.Count; i++)
            {
                string actionName = obj.actionInfos.ListKeys[i];
                ActionResInfo actionInfo = obj.actionInfos.ListValues[i];
                foldOuts[i] = EditorGUILayout.Foldout(foldOuts[i], actionName + "  [frames:" + actionInfo.frameInfos.Length + "]");
                if (foldOuts[i])
                {
                    for (int j = 0; j < actionInfo.frameInfos.Length; j++)
                    {
                        FrameResInfo frameInfo = actionInfo.frameInfos[j];
                        if (frameInfo.frameResInfos == null)
                            continue;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(20);
                        frameFoldOuts[i][j] = EditorGUILayout.Foldout(frameFoldOuts[i][j], "frame:" + j);
                        EditorGUILayout.EndHorizontal();
                        if (frameFoldOuts[i][j])
                        {
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            EditorGUILayout.LabelField("delay", GUILayout.Width(40));
                            frameInfo.delay = EditorGUILayout.FloatField(frameInfo.delay, GUILayout.Width(100));
                            EditorGUILayout.EndHorizontal();
                            for (int k = 0; k < frameInfo.frameResInfos.Count; k++)
                            {
                                string partKey = frameInfo.frameResInfos.ListKeys[k];
                                EditorGUILayout.BeginHorizontal();
                                GUILayout.Space(20);
                                EditorGUILayout.LabelField(partKey, GUILayout.Width(50));
                                EditorGUILayout.LabelField("z", GUILayout.Width(20));
                                frameInfo.frameResInfos[partKey].z = EditorGUILayout.TextField(frameInfo.frameResInfos[partKey].z,GUILayout.Width(50));
                                EditorGUILayout.LabelField("group", GUILayout.Width(50));
                                frameInfo.frameResInfos[partKey].group = EditorGUILayout.TextField(frameInfo.frameResInfos[partKey].group, GUILayout.Width(50));
                                EditorGUILayout.LabelField("sprite", GUILayout.Width(50));
                                frameInfo.frameResInfos[partKey].sprite = (Sprite)EditorGUILayout.ObjectField(frameInfo.frameResInfos[partKey].sprite, typeof(Sprite), true);
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                    }
                }
            }

        }
        else
        {
            EditorGUI.DropShadowLabel(new Rect(5, 60, 100, 10), "this is no data");
        }
        serializedObject.ApplyModifiedProperties();
    }
}

