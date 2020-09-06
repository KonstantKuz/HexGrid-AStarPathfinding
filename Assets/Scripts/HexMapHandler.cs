using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security;
using UnityEditor;
using UnityEngine;

public class HexMapHandler : MonoBehaviour, IMap
{
    [SerializeField] private GameObject hexesHolder;

    [SerializeField] private GameObject hexPrefab;
    [SerializeField] private Vector2 distanceBtwnHexes;
    [SerializeField] private Vector2 mapSize;
    
    public Dictionary<MapPoint, ICell> mapDictionary { get; set; }
    private MapPoint tmpPosition;

    private void Start()
    {
        CreateMap();
    }

    [ContextMenu("Create grid")]
    private void CreateMap()
    {
        mapDictionary = new Dictionary<MapPoint, ICell>();

        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                HexCell hex = Instantiate(hexPrefab).GetComponent<HexCell>();
                Vector3 hexWorldPosition = GetWorldPosition(new MapPoint(x,y));
                //y / 2 is subtracted from x because we are using straight axis coordinate system
                MapPoint hexMapPoint = new MapPoint(x-y/2,y);
                SetUpHex(hex, hexMapPoint, hexWorldPosition);
                
                mapDictionary.Add(hexMapPoint, hex);
            }
        }
    }
    
    private void SetUpHex(HexCell hex, MapPoint hexMapPoint, Vector3 hexWorldPosition)
    {
        hex.Construct(hexMapPoint, hexWorldPosition,!IsThereObstacle(hexWorldPosition));
        hex.transform.position = hexWorldPosition;
        hex.transform.parent = hexesHolder.transform;
    }

    private bool IsThereObstacle(Vector3 cellPosition)
    {
        if (Physics.CheckSphere(cellPosition, 1f, 1<<Constants.ObstacleLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private Vector3 GetWorldPosition(MapPoint mapPoint)
    {
        Vector3 initPos = GetInitialPosition();
        
        //Every second row is offset by half of the tile width
        float offset = 0;
        if (mapPoint.Y % 2 != 0)
        {
            offset = distanceBtwnHexes.x / 2;
        }
        float x =  initPos.x + offset + mapPoint.X * distanceBtwnHexes.x;
        
        //Every new line is offset in z direction by 3/4 of the hexagon height
        float z = initPos.z - mapPoint.Y * distanceBtwnHexes.y * 0.75f;
        return new Vector3(x, 0, z);
    }
 
    private Vector3 GetInitialPosition()
    {
        Vector3 initialPos = new Vector3(-distanceBtwnHexes.x * mapSize.x / 2f + distanceBtwnHexes.x / 2, 0,
                                                     mapSize.y / 2f * distanceBtwnHexes.y - distanceBtwnHexes.y / 2);
        return initialPos;
    }

    public List<ICell> GetNeighbours(ICell cell)
    {
        List<ICell> neighbours = new List<ICell>();
        List<MapPoint> neighboursShift = NeighboursShift;
        foreach (MapPoint point in neighboursShift)
        {
            int neighbourX = cell.MapPoint.X + point.X;
            int neighbourY = cell.MapPoint.Y + point.Y;
            //x coordinate offset specific to straight axis coordinates
            int xOffset = neighbourY / 2;
 
            //If every second hexagon row has less hexagons than the first one, just skip the last one when we come to it
            if (neighbourY % 2 != 0 &&
                neighbourX + xOffset == (int)mapSize.x - 1)
                continue;
            //Check to determine if currently processed coordinate is still inside the board limits
            if (neighbourX >= 0 - xOffset &&
                neighbourX < (int) mapSize.x - xOffset &&
                neighbourY >= 0 && neighbourY < (int) mapSize.y)
            {
                MapPoint neighbourMapPoint = new MapPoint(neighbourX, neighbourY);
                neighbours.Add(mapDictionary[neighbourMapPoint]);
            }
        }

        return neighbours;
    }

    public static List<MapPoint> NeighboursShift
    {
        get
        {
            return new List<MapPoint>
            {
                new MapPoint(0, 1),
                new MapPoint(1, 0),
                new MapPoint(1, -1),
                new MapPoint(0, -1),
                new MapPoint(-1, 0),
                new MapPoint(-1, 1),
            };
        }
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                Vector3 hexWorldPosition = GetWorldPosition(new MapPoint(x,y));
                Gizmos.color = IsThereObstacle(hexWorldPosition)? Color.red : Color.green;
                Gizmos.DrawWireSphere(hexWorldPosition, 0.5f);
            }
        }
    }
}

[System.Serializable]
public struct MapPoint
{
    public int X;
    public int Y;

    public MapPoint(int x, int y)
    {
        X = x;
        Y = y;
    }
}