using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexCell))]
public class HexCellEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUI.BeginDisabledGroup(true);
        HexCell cell = (HexCell) target;
        EditorGUILayout.IntField("X Position on Grid", cell.MapPoint.X);
        EditorGUILayout.IntField("Y Position on Grid", cell.MapPoint.Y);
        EditorGUI.EndDisabledGroup();
    }
}
