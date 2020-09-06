using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathStatusViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI startCoord;
    [SerializeField] private TextMeshProUGUI endCoord;
    [SerializeField] private TextMeshProUGUI pathLength;

    private void OnEnable()
    {
        EventHolder.OnPathBuilded += ShowPathStatus;
    }

    private void OnDisable()
    {
        EventHolder.OnPathBuilded -= ShowPathStatus;
    }
    
    private void ShowPathStatus(PathData pathData)
    {
        startCoord.SetText($"Start coord : {pathData.startCell.WorldPosition} ({pathData.startCell.MapPoint.X}:{pathData.startCell.MapPoint.Y})");
        endCoord.SetText($"End coord : {pathData.endCell.WorldPosition} ({pathData.endCell.MapPoint.X}:{pathData.endCell.MapPoint.Y})");
        pathLength.SetText($"Path length : {pathData.unitLength()} units ({pathData.cellsLength()} cells)");
    }
}
