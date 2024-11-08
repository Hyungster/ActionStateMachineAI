using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;
using Codice.Client.Common.GameUI;


[CustomEditor(typeof(Character))]
public class Character_Inspector : Editor
{
    private List<Type> allActionTypes = new List<Type>();
    private List<Type> allStateTypes = new List<Type>();
    private List<Type> allStatusTypes = new List<Type>();
    private List<Type> allFunctionClipTypes = new List<Type>();

    public override void OnInspectorGUI()
    {

        //get all FunctionClip subtypes, get string names
        allActionTypes = CharacterUtil.GetAllActionTypes();
        string[] actionNames = allActionTypes.Select(t => t.Name).ToArray();

        allStateTypes = CharacterUtil.GetAllStateTypes();
        string[] stateNames = allStateTypes.Select(t => t.Name).ToArray();

        allStatusTypes = CharacterUtil.GetAllStatusTypes();
        string[] statusNames = allStatusTypes.Select(t => t.Name).ToArray();

        allFunctionClipTypes = CharacterUtil.GetAllFunctionClipTypes();
        string[] functionClipNames = allFunctionClipTypes.Select(t => t.Name).ToArray();



        DrawDefaultInspector();

        Character character = (Character)target;

        SerializedObject s_character = new SerializedObject(character);

        SerializedProperty s_actionTransitionTable = s_character.FindProperty("actionTransitionTable");
        var s_actionClipRows = s_actionTransitionTable.FindPropertyRelative("clipRows");
        var s_actionConditionsColumns = s_actionTransitionTable.FindPropertyRelative("conditionsColumns");

        SerializedProperty s_stateTransitionTable = s_character.FindProperty("actionTransitionTable");
        var s_stateConditionsColumns = s_stateTransitionTable.FindPropertyRelative("conditionsColumns");

        SerializedProperty s_statusTransitionTable = s_character.FindProperty("actionTransitionTable");
        var s_statusConditionsColumns = s_statusTransitionTable.FindPropertyRelative("conditionsColumns");


        GUILayout.Label("Action Transition Table", EditorStyles.boldLabel);
        Rect line = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth, 5);
        EditorGUI.DrawRect(line, Color.green);

        Undo.RecordObject(character, "Editing Initial Action Index");
        character.initialActionIndex = EditorGUILayout.Popup(character.initialActionIndex, actionNames);


        GUILayout.Label("Possible Actions");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            int index = s_actionClipRows.arraySize;
            if (index < 0) index = 0;
            s_actionClipRows.InsertArrayElementAtIndex(index);
            s_character.ApplyModifiedProperties();
            s_character.Update();
            
        }
        if (GUILayout.Button("-") && character.actionTransitionTable.clipRows.Count > 1) 
        {
            int index = s_actionClipRows.arraySize - 1;
            if (index < 0) index = 0;
            s_actionClipRows.DeleteArrayElementAtIndex(index);
            s_character.ApplyModifiedProperties();
            s_character.Update();
            
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Conditions");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            int index = s_actionConditionsColumns.arraySize - 1;
            if (index < 0) index = 0;
            s_actionConditionsColumns.InsertArrayElementAtIndex(index);
            s_character.ApplyModifiedProperties();
            s_character.Update();
            
        }

        if (GUILayout.Button("-") && character.actionTransitionTable.conditionsColumns.Count > 1)
        {
            int index = s_actionConditionsColumns.arraySize - 1;
            if (index < 0) index = 0;
            s_actionConditionsColumns.DeleteArrayElementAtIndex(index);
            s_character.ApplyModifiedProperties();
            s_character.Update();
            
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical();
        for (int i = 0; i <= character.actionTransitionTable.clipRows.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (i == 0) //top row
            {
                for (int j = 0; j <= character.actionTransitionTable.conditionsColumns.Count; j++)
                {
                    if (j == 0)
                    {
                        Rect emptyRect = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth / (1.2f + s_actionConditionsColumns.arraySize), EditorGUIUtility.singleLineHeight);
                        EditorGUI.DrawRect(emptyRect, Color.green);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(s_actionConditionsColumns.GetArrayElementAtIndex(j - 1), GUIContent.none);
                        s_character.ApplyModifiedProperties();
                        s_character.Update();
                    }
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                for (int j = 0; j <= character.actionTransitionTable.conditionsColumns.Count; j++)
                {
                    if (j == 0)
                    {
                        Undo.RecordObject(character, "Editing Initial Clip Choices Index");
                        character.actionTransitionTable.clipChoiceIndices[i] = EditorGUILayout.Popup(character.actionTransitionTable.clipChoiceIndices[i], actionNames);
                        EditorUtility.SetDirty(character);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(character);

                    }
                    else
                    {
                        Undo.RecordObject(character, "Editing Clip Choices Table");
                        character.actionTransitionTable.indexTableRows[i - 1].columns[j - 1] = EditorGUILayout.Popup(character.actionTransitionTable.indexTableRows[i - 1].columns[j - 1], functionClipNames);
                        EditorUtility.SetDirty(character);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(character);

                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        if (Input.GetMouseButtonDown(0))
        {
            EditorUtility.SetDirty(character);
            PrefabUtility.RecordPrefabInstancePropertyModifications(character);
        }
    }
}
