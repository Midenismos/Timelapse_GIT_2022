using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestLoreScript)), CanEditMultipleObjects]
public class BookEditor : Editor
{
    TestLoreScript TestLoreScript;

    SerializedProperty LoreTextNormal;

    Vector2 ScrollPositionNormal;


    void OnEnable()
    {
        TestLoreScript = target as TestLoreScript;
        LoreTextNormal = serializedObject.FindProperty("LoreTextNormal");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        

        //Fait apparaître les variables LoreTextLeft et LoreTextRight si le type de doc est un livre
        if (TestLoreScript.LoreType == TestLoreScript.Type.BOOK)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Il faut mettre le texte dans PaperPages (variables BookTextLeftPages et BookTextRightPages)", EditorStyles.boldLabel);
        }
        else if (TestLoreScript.LoreType == TestLoreScript.Type.WHITE)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Il faut mettre le texte dans PaperPages (variable TextSheet)", EditorStyles.boldLabel);
        }
        else
        {
            ScrollPositionNormal = GUILayout.BeginScrollView(ScrollPositionNormal, GUILayout.Width(410), GUILayout.Height(200));
            EditorGUILayout.PrefixLabel(LoreTextNormal.displayName);
            LoreTextNormal.stringValue = EditorGUILayout.TextArea(LoreTextNormal.stringValue, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
