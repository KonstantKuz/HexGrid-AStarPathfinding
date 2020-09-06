using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HexCell : MonoBehaviour, ICell
{
    [SerializeField] private CellColors cellColors;
    public CellColors CellColors
    {
        get { return cellColors; }
    }

    [SerializeField] private MeshRenderer renderer;

    public MapPoint MapPoint { get; set; }
    public Vector3 WorldPosition { get; set; }
    public bool IsOpened { get; set; }
    public int GCost{ get; set; }
    public int HCost{ get; set; }
    public ICell Parent{ get; set; }
    
    public int FCost
    {
        get
        {
            return GCost + HCost;
        }
    }

    public void Construct(MapPoint mapPoint, Vector3 worldPosition, bool isOpened)
    {
        MapPoint = mapPoint;
        WorldPosition = worldPosition;
        SetCellStatus(isOpened);
    }

    public void SetCellStatus(bool isOpened)
    {
        IsOpened = isOpened;
        Color colorToSet = IsOpened ? cellColors.normalCellColor : cellColors.closedCellColor;
        SetColor(colorToSet);
        EventHolder.OnSomeCellChanged();
    }

    public void SetColor(Color color)
    {
        renderer.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == Constants.ObstacleLayer)
        {
            SetCellStatus(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == Constants.ObstacleLayer)
        {
            SetCellStatus(true);
        }
    }
}
