using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        GridGenerator gene = (GridGenerator)target;

        // 인스펙터에 모든 필드 표시 
        DrawDefaultInspector();

        if (GUILayout.Button("Generate 3D Grid"))
        {
            gene.F_GenerateGrid3D();
        }

    }
    
}
