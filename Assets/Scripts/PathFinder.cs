using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PathFinder : MonoBehaviour
{
    private Grid grid;
    public List<Grid.NodeForMap> path;
    // Use this for initialization
    void Start()
    {
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // A* finding
    public void FindingPath(Vector3 start, Vector3 end)
    {
        Grid.NodeForMap startNode = grid.GetNode(start);
        Grid.NodeForMap endNode = grid.GetNode(end);

        List<Grid.NodeForMap> openList = new List<Grid.NodeForMap>();
        HashSet<Grid.NodeForMap> closeList = new HashSet<Grid.NodeForMap>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Grid.NodeForMap currentNode = openList[0];

            for (int i = 0, max = openList.Count; i < max; i++)
            {
                if (openList[i].fCost <= currentNode.fCost &&
                    openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            // find end node
            if (currentNode == endNode)
            {
                GeneratePath(startNode, endNode);
                return;
            }

            // choose best node from neighbors
            foreach (var item in grid.GetNeibours(currentNode))
            {
                
                if (item.isWall || closeList.Contains(item))
                    continue;
              
                int newCost = currentNode.gCost + GetDistanceNodes(currentNode, item);
                
                if (newCost < item.gCost || !openList.Contains(item))
                {
                   
                    item.gCost = newCost;
                   
                    item.hCost = GetDistanceNodes(item, endNode);
                  
                    item.parent = currentNode;
                    
                    if (!openList.Contains(item))
                    {
                        openList.Add(item);
                    }
                }
            }
        }

        GeneratePath(startNode, null);
    }

  
    void GeneratePath(Grid.NodeForMap startNode, Grid.NodeForMap endNode)
    {
        path = new List<Grid.NodeForMap>();
        if (endNode != null)
        {
            Grid.NodeForMap temp = endNode;
            while (temp != startNode)
            {
                path.Add(temp);
                temp = temp.parent;
            }
          
            path.Reverse();
        }
       
        grid.UpdatePath(path);
    }

    // heuristics Diagonal distance
    int GetDistanceNodes(Grid.NodeForMap a, Grid.NodeForMap b)
    {
        int X = Mathf.Abs(a.x - b.x);
        int Y = Mathf.Abs(a.y - b.y);
    
        if (X > Y)
        {
            return 14 * Y + 10 * (X - Y);
        }
        else
        {
            return 14 * X + 10 * (Y - X);
        }
    }


}