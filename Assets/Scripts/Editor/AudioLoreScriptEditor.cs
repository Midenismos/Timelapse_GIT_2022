using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioLoreScript)), CanEditMultipleObjects]

public class AudioLoreScriptEditor : Editor
{
    AudioLoreScript AudioLoreScript;

    SerializedProperty ChangeTimePeriod1Property;
    SerializedProperty ChangeTimePeriod2Property;

    SerializedProperty SoundSpeedBeforeTimePeriodProperty;
    SerializedProperty SoundSpeedBetweenTimePeriodsProperty;
    SerializedProperty SoundSpeedAfterTimePeriodProperty;

    void OnEnable()
    {
        AudioLoreScript = target as AudioLoreScript;
        ChangeTimePeriod1Property = serializedObject.FindProperty("ChangeTimePeriod1");
        ChangeTimePeriod2Property = serializedObject.FindProperty("ChangeTimePeriod2");
        SoundSpeedBeforeTimePeriodProperty = serializedObject.FindProperty("SoundSpeedBeforeTimePeriod");
        SoundSpeedBetweenTimePeriodsProperty = serializedObject.FindProperty("SoundSpeedBetweenTimePeriods");
        SoundSpeedAfterTimePeriodProperty = serializedObject.FindProperty("SoundSpeedAfterTimePeriod");

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        if(AudioLoreScript.HasChangeTimePeriod)
        {
            if (AudioLoreScript.HowManyTimePeriod == 1)
            {
                EditorGUILayout.PropertyField(ChangeTimePeriod1Property);
                EditorGUILayout.PropertyField(SoundSpeedBeforeTimePeriodProperty);
                EditorGUILayout.PropertyField(SoundSpeedAfterTimePeriodProperty);
            }
            else if (AudioLoreScript.HowManyTimePeriod == 2)
            {
                EditorGUILayout.PropertyField(ChangeTimePeriod1Property);
                EditorGUILayout.PropertyField(ChangeTimePeriod2Property);
                EditorGUILayout.PropertyField(SoundSpeedBeforeTimePeriodProperty);
                EditorGUILayout.PropertyField(SoundSpeedAfterTimePeriodProperty);
                EditorGUILayout.PropertyField(SoundSpeedBetweenTimePeriodsProperty);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
