using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(Chunk))]
public class ChunkEditor : Editor
{
    private int niveau = 0;

    public override void OnInspectorGUI()
    {
        Chunk c = (Chunk)target;

        DrawDefaultInspector();

        GUILayout.HorizontalSlider(niveau, 0, 64);
        GUILayout.Space(10);
        GUILayout.Label("" + c.data.Length.ToString());
    }
}
