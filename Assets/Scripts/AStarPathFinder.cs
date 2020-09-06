using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder : MonoBehaviour, IPathFinder
{
    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        List<ICell> openSet = new List<ICell>();
        HashSet<ICell> closedSet = new HashSet<ICell>();
        
        openSet.Add(cellStart);

        while (openSet.Count > 0)
        {
            ICell currentCell = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost <= currentCell.FCost && openSet[i].HCost < currentCell.HCost)
                {
                    currentCell = openSet[i];
                }
            }
            
            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if (currentCell == cellEnd)
            {
                List<ICell> pathToReturn = RetracePath(cellStart, cellEnd);
                return pathToReturn;
            }

            foreach (ICell neighbour in map.GetNeighbours(currentCell))
            {
                if (!neighbour.IsOpened || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = currentCell.GCost + GetDistance(currentCell, neighbour);
                if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, cellEnd);
                    neighbour.Parent = currentCell;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }

    private List<ICell> RetracePath(ICell startCell, ICell endCell)
    {
        List<ICell> path = new List<ICell>();
        ICell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.Parent;
        }
        
        path.Reverse();

        return path;
    }
    
    private int GetDistance(ICell startCell, ICell endCell)
    {
             int dstX = Mathf.Abs(startCell.MapPoint.X - endCell.MapPoint.X);
             int dstY = Mathf.Abs(startCell.MapPoint.Y - endCell.MapPoint.Y);
            
            return  Mathf.Abs (dstX + dstY);
            
            // if (dstX > dstY)
            //     return 14 * dstY + 10 * (dstX - dstY);
            // return 14 * dstX + 10 * (dstY - dstX);
            
            // float deltaX = Mathf.Abs(endCell.GridPoint.X - startCell.GridPoint.X);
            // float deltaY = Mathf.Abs(endCell.GridPoint.Y - startCell.GridPoint.Y);
            // int z1 = -(startCell.GridPoint.X + startCell.GridPoint.Y);
            // int z2 = -(endCell.GridPoint.X + endCell.GridPoint.Y);
            // float deltaZ = Mathf.Abs(z2 - z1);
            //
            // return (int)Mathf.Max(deltaX, deltaY, deltaZ);
    }
}
