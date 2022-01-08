using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerateur))]
public class GenerationMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerateur mapGen = (MapGenerateur)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generation"))
        {
            mapGen.Generation();
        }

        if (GUILayout.Button("Clear"))
        {
            mapGen.Clear();
        }
    }
}
