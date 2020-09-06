using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell
{
    Vector3 WorldPosition { get; set; }
    MapPoint MapPoint { get; set; }
    bool IsOpened { get; set; }
    int GCost { get; set; }
    int HCost { get; set; }
    int FCost { get; }
    ICell Parent { get; set; }
}
