using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace Audune.Social.Steam.Editor
{
  // Class that defines an editor for a StreamingAssets locale loader
  [CustomEditor(typeof(SteamSocialProvider))]
  public class SteamSocialProviderEditor : UnityEditor.Editor
  {
    // Constants
    private const string _applicationSettingsURL = "https://partner.steamgames.com/apps/landing/{0}";
    private const string _overviewURL = "https://partner.steamgames.com/doc/home";
    private const string _apiDocumentationURL = "https://partner.steamgames.com/doc/api";
    
    
    // Properties of the editor
    private SerializedProperty _priority;
    private SerializedProperty _executionMode;
    private SerializedProperty _steamApplicationId;
    private SerializedProperty _steamClientRequired;

    // Foldout state of the editor
    private bool _applicationDetailsFoldout = true;
    private bool _executionSettingsFoldout = false;

    // Return the target object of the editor
    public new SteamSocialProvider target => serializedObject.targetObject as SteamSocialProvider;


    // OnEnable is called when the component becomes enabled
    protected void OnEnable()
    {
      // Initialize the properties
      _priority = serializedObject.FindProperty("_priority");
      _executionMode = serializedObject.FindProperty("_executionMode");
      _steamApplicationId = serializedObject.FindProperty("_steamApplicationId");
      _steamClientRequired = serializedObject.FindProperty("_steamClientRequired");
    }

    // Draw the inspector GUI
    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      _applicationDetailsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_applicationDetailsFoldout, "Application Details");
      if (_applicationDetailsFoldout)
      {
        EditorGUILayout.PropertyField(_steamApplicationId, new GUIContent("Application ID", _steamApplicationId.tooltip));
        EditorGUILayout.PropertyField(_steamClientRequired);
        
        EditorGUILayout.Space();
        
        if (_steamApplicationId.uintValue != 0 && GUILayout.Button("Open Application Settings"))
          Application.OpenURL(string.Format(_applicationSettingsURL, _steamApplicationId.uintValue.ToString(CultureInfo.InvariantCulture)));

        EditorGUILayout.Space();
      }
      EditorGUI.EndFoldoutHeaderGroup();

      _executionSettingsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_executionSettingsFoldout, "Execution Settings");
      if (_executionSettingsFoldout)
      {
        EditorGUILayout.PropertyField(_priority);
        EditorGUILayout.PropertyField(_executionMode);
      }
      EditorGUI.EndFoldoutHeaderGroup();
      
      EditorGUILayout.Space();
      
      if (GUILayout.Button("Open Steamworks SDK Overview"))
        Application.OpenURL(_overviewURL);
      if (GUILayout.Button("Open Steamworks SDK API Documentation"))
        Application.OpenURL(_apiDocumentationURL);

      if (EditorGUI.EndChangeCheck())
        serializedObject.ApplyModifiedProperties();
    }
  }
}