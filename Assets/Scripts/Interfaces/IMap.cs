using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap
{
    Dictionary<MapPoint, ICell> mapDictionary { get; set; }
    List<ICell> GetNeighbours(ICell cell);
}
