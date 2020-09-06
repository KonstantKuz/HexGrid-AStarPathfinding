using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PathRequester : MonoBehaviour
{
    [SerializeField] private Selector selector;
    [SerializeField] private AStarPathFinder aStarPathFinder;
    [SerializeField] private HexMapHandler hexMapHandler;
    [SerializeField] private LineRenderer pathTrail;
    
    private IMap map;
    private IPathFinder pathFinder;

    private void OnEnable()
    {
        EventHolder.OnSomeCellChanged += TryBuildPath;
    }

    private void OnDisable()
    {
        EventHolder.OnSomeCellChanged -= TryBuildPath;
    }

    private void Start()
    {
        map = hexMapHandler;
        pathFinder = aStarPathFinder;
    }

    private void TryBuildPath()
    {
        if (selector.Start == null || selector.End == null)
            return;
        
        IList<ICell> path = pathFinder.FindPathOnMap(selector.Start, selector.End, map);

        if (path != null)
        {
            VisualizePath(path);
            SendPath(selector.Start, selector.End, path);
        }
        else
        {
            pathTrail.positionCount = 0;
        }
    }

    private void VisualizePath(IList<ICell> path)
    {
        int trailPointsCount = path.Count;
        pathTrail.positionCount = trailPointsCount;

        Vector3 heightOffset = Vector3.up/2;
        for (int i = 0; i < trailPointsCount; i++)
        {
            pathTrail.SetPosition(i, path[i].WorldPosition + heightOffset);
        }
    }

    private void SendPath(ICell startCell, ICell endCell, IList<ICell> path)
    {
        PathData pathData = new PathData(startCell, endCell, path);
        EventHolder.OnPathBuilded(pathData);
    }
}

public class PathData
{
    public ICell startCell;
    public ICell endCell;
    public IList<ICell> path;

    public PathData(ICell startCell, ICell endCell, IList<ICell> path)
    {
        this.startCell = startCell;
        this.endCell = endCell;
        this.path = path;
    }
    
    public float unitLength()
    {
        float unitLength = 0;
        for (int i = 1; i < path.Count; i++)
        {
            unitLength += (path[i].WorldPosition - path[i - 1].WorldPosition).magnitude;
        }
        return unitLength;
    }

    public int cellsLength()
    {
        return path.Count - 1;
    }
}