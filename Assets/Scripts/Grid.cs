using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject NodeWall;
    public GameObject Node;
    public float NodeRadius = 0.5f;
    public LayerMask Layer;
    public Transform enemyTransform;  //start position
    public Transform endPos;

    public class NodeForMap
    {
    
        public bool isWall;
        public Vector3 pos;
        public int x, y;
        public int gCost;
        public int hCost;
        public int fCost
        {
            get { return gCost + hCost; }
        }

       
        public NodeForMap parent;
        public NodeForMap(bool isWall, Vector3 pos, int x, int y)
        {
            this.isWall = isWall;
            this.pos = pos;
            this.x = x;
            this.y = y;
        }
    }

    private NodeForMap[,] grid;
    private int w, h;

    private GameObject WallRange, PathRange;
    private List<GameObject> pathObject = new List<GameObject>();

    void Awake()
    {
        Node = GameObject.Find("Node");
        NodeWall = GameObject.Find("NodeWall");
        enemyTransform = transform.parent.gameObject.transform.parent.transform;
        //according to the scene
        w = 30;
        h = 30;
        grid = new NodeForMap[w, h];

        WallRange = new GameObject("WallRange");
        PathRange = new GameObject("PathRange");

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                Vector3 pos = new Vector3(x-15 , -0.8f, y-5 );
                bool isWall = Physics.CheckSphere(pos, NodeRadius, Layer);
                grid[x, y] = new NodeForMap(isWall, pos, x, y);
                if (isWall)
                {
                    GameObject obj = GameObject.Instantiate(NodeWall, pos, Quaternion.identity) as GameObject;
                    obj.transform.SetParent(WallRange.transform);
                }
            }
        }

    }

    public void UpdatePath(List<NodeForMap> path)
    {
        int Size = pathObject.Count;
        for (int i = 0, max = path.Count; i < max; i++)
        {
            if (i < Size)
            {
                pathObject[i].transform.position = path[i].pos;
                pathObject[i].SetActive(true);
            }
            else
            {
                GameObject obj = GameObject.Instantiate(Node, path[i].pos, Quaternion.identity) as GameObject;
                obj.transform.SetParent(PathRange.transform);
                pathObject.Add(obj);
            }
        }
        for (int i = path.Count; i < Size; i++)
        {
            pathObject[i].SetActive(false);
        }
    }

    public NodeForMap GetNode(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.z);
        return grid[x+15, y+5];
    }

    public List<NodeForMap> GetNeibours(NodeForMap node)
    {
        List<NodeForMap> list = new List<NodeForMap>();
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
              
                if (i == 0 && j == 0)
                    continue;
                int x = node.x + i;
                int y = node.y + j;
               
                if (x < w && x >= 0 && y < h && y >= 0)
                    list.Add(grid[x, y]);
            }
        }
        return list;
    }
}
