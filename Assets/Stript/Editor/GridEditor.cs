using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridGenerator))]
public class GridEditor : Editor
{
    /// <summary>
    /// Map Grid 2�����迭�� ����� ���� Editor
    /// </summary>

    public override void OnInspectorGUI()
    {
        GridGenerator gene = (GridGenerator)target;

        // �ν����Ϳ� ��� �ʵ� ǥ�� 
        DrawDefaultInspector();

        if (GUILayout.Button("Generate 3D Grid"))
        {
            gene.F_GenerateGrid3D();
        }

    }
    
}
