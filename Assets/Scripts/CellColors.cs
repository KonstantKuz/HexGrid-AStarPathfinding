using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CellColors")]
public class CellColors : ScriptableObject
{
    public Color normalCellColor;
    public Color closedCellColor;
    public Color selectedCellColor;
    public Color startCellColor;
    public Color endCellColor;
}