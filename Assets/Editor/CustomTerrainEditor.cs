using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    #region Properties
    private SerializedProperty _resetHeightMap;
    private SerializedProperty _randomHeightRange;
    private SerializedProperty _heightMapScale;
    private SerializedProperty _heightMapImage;
    private SerializedProperty _perlinXScale;
    private SerializedProperty _perlinYScale;
    private SerializedProperty _perlinXOffset;
    private SerializedProperty _perlinYOffset;
    private SerializedProperty _perlinOctaves;
    private SerializedProperty _perlinPersistance;
    private SerializedProperty _perlinHeightScale;
    #endregion

    #region Fold Outs
    private bool _showRandom;
    private bool _showLoadHeights;
    private bool _showPerlinNoise;

    #endregion

    void OnEnable()
    {
        _resetHeightMap = serializedObject.FindProperty("ResetHeightMap");
        _randomHeightRange = serializedObject.FindProperty("RandomHeightRange");
        _heightMapScale = serializedObject.FindProperty("HeightMapScale");
        _heightMapImage = serializedObject.FindProperty("HeightMapImage");
        _perlinXScale = serializedObject.FindProperty("PerlinXScale");
        _perlinYScale = serializedObject.FindProperty("PerlinYScale");
        _perlinXOffset = serializedObject.FindProperty("PerlinXOffset");
        _perlinYOffset = serializedObject.FindProperty("PerlinYOffset");
        _perlinOctaves = serializedObject.FindProperty("PerlinOctaves");
        _perlinPersistance = serializedObject.FindProperty("PerlinPersistance");
        _perlinHeightScale = serializedObject.FindProperty("PerlinHeightScale");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CustomTerrain terrain = (CustomTerrain)target;
        EditorGUILayout.PropertyField(_resetHeightMap);
            
        _showRandom = EditorGUILayout.Foldout(_showRandom, "Random");
        if (_showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set Heights Between Random Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_randomHeightRange);
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }

        _showLoadHeights = EditorGUILayout.Foldout(_showLoadHeights, "Load Heights");
        if (_showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights From Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_heightMapImage);
            EditorGUILayout.PropertyField(_heightMapScale);
            if (GUILayout.Button("Load Texture"))
            {
                terrain.LoadTexture();
            }
        }
        
        _showPerlinNoise = EditorGUILayout.Foldout(_showPerlinNoise, "Perlin Noise");
        if (_showPerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Generate Heights From Perlin Noise", EditorStyles.boldLabel);
            EditorGUILayout.Slider(_perlinXScale, 0, 0.01f, new GUIContent("X Scale"));
            EditorGUILayout.Slider(_perlinYScale, 0, 0.01f, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(_perlinXOffset, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(_perlinYOffset, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.IntSlider(_perlinOctaves, 0, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(_perlinPersistance, 0.1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(_perlinHeightScale, 0, 1, new GUIContent("Height Scale"));
            if (GUILayout.Button("Generate"))
            {
                terrain.Perlin();
            }
        }
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        
        if (GUILayout.Button("Reset"))
        {
            terrain.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
