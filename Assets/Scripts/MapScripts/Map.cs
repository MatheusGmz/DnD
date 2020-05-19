using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    int mapSizeX = 10;
    int mapSizeY = 10;
    int[,] tiles;
    public Node[,] graph;
    public TileType[] tileType;
    GameObject selectedPlayer;
    Color originalColor;

    void Start()
    {
        GenerateMapData();
        GeneratePathFindingGraph();
        GenerateMapVisual();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
            Tile hitTile = ourHitObject.GetComponentInChildren<Tile>();

            if (hitTile != null)
            {

            }

            if (ourHitObject.GetComponent<Player>() != null)
            {
                MouseOver_Unit(ourHitObject);
            }
            if (selectedPlayer != null && Input.GetKeyDown("k"))
            {
                var mr = selectedPlayer.GetComponentInChildren<MeshRenderer>();
                mr.material.color = originalColor;
                selectedPlayer = null;
            }
            if (selectedPlayer != null && Input.GetKeyDown("a"))
            {
                Debug.Log("Attack!");
            }

        }
    }

    void MouseOver_Unit(GameObject ourHitObject)
    {

        var mr = ourHitObject.GetComponentInChildren<MeshRenderer>();
        if (Input.GetMouseButtonDown(1))
        {
            selectedPlayer = ourHitObject;
            selectedPlayer.GetComponent<Player>().tileX = (int)selectedPlayer.transform.position.x;
            selectedPlayer.GetComponent<Player>().tileY = (int)selectedPlayer.transform.position.z;
            selectedPlayer.GetComponent<Player>().tileMap = this;
            if (mr.material.color != Color.grey)
            {
                originalColor = mr.material.color;
                mr.material.color = Color.grey;
            }
        }

    }

    void GenerateMapData()
    {
        tiles = new int[(int)mapSizeX, (int)mapSizeY];

        int x, y;

        int stoneTile = 2;
        int mudTile = 1;
        for (x = 0; x < mapSizeX; x++)
        {
            for (y = 0; y < mapSizeY; y++)
            {
                tiles[x, y] = 0;
            }
        }
        for (x = 3; x <= 5; x++)
        {
            for (y = 0; y < 4; y++)
            {
                tiles[x, y] = mudTile;
            }
        }
        tiles[4, 5] = stoneTile;
        tiles[5, 5] = stoneTile;
        tiles[6, 5] = stoneTile;
        tiles[7, 5] = stoneTile;
        tiles[8, 5] = stoneTile;
        tiles[4, 6] = stoneTile;
        tiles[4, 7] = stoneTile;
        tiles[8, 6] = stoneTile;
        tiles[8, 7] = stoneTile;
    }
    void GenerateMapVisual()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType tt = tileType[tiles[x, y]];
                GameObject tile_go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                tile_go.name = "Square_" + x + "_" + y;
                Tile tile = tile_go.GetComponentInChildren<Tile>();
                tile.tileX = x;
                tile.tileY = y;
                tile.tileMap = this;
                tile_go.transform.SetParent(this.transform);
                tile_go.isStatic = true;

            }
        }
    }
    void GeneratePathFindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;
            }
        }
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (x > 0)
                {
                    //Add left tile
                    graph[x, y].neighbours.Add(graph[x - 1, y]);
                    if (y > 0)
                        //add left down tile
                        graph[x, y].neighbours.Add(graph[x - 1, y - 1]);
                    if (y < mapSizeY - 1)
                        //add left up tile
                        graph[x, y].neighbours.Add(graph[x - 1, y + 1]);
                }

                if (x < mapSizeX - 1)
                {
                    //add right tile
                    graph[x, y].neighbours.Add(graph[x + 1, y]);
                    if (y > 0)
                        //add right tile down
                        graph[x, y].neighbours.Add(graph[x + 1, y - 1]);
                    if (y < mapSizeY - 1)
                        //add right tile up
                        graph[x, y].neighbours.Add(graph[x + 1, y + 1]);
                }

                if (y > 0)
                    graph[x, y].neighbours.Add(graph[x, y - 1]);
                if (y < mapSizeY - 1)
                    graph[x, y].neighbours.Add(graph[x, y + 1]);
            }

        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x, 0, y);
    }
    public bool UnitCanEnterTile(int x, int y)
    {

        return tileType[tiles[x, y]].isWalkable;
    }
    public void GeneratePathTo(int x, int y) 
    {
        selectedPlayer.GetComponent<Player>().currentPath = null;

        if (UnitCanEnterTile(x, y) == false)
        {
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[
                            selectedPlayer.GetComponent<Player>().tileX,
                            selectedPlayer.GetComponent<Player>().tileY
                            ];

        Node target = graph[
                            x,
                            y
                            ];

        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;
            }

            unvisited.Remove(u);

            foreach (Node v in u.neighbours)
            {
                float alt = dist[u] + CostToEnterTile(u.x, u.y, v.x, v.y);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        if (prev[target] == null)
        {
            Debug.LogError("No way to get there!");
            return;
        }

        List<Node> currentPath = new List<Node>();

        Node curr = target;

        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        currentPath.Reverse();

        selectedPlayer.GetComponent<Player>().currentPath = currentPath;
    }
    public float CostToEnterTile(int sourceX, int sourceY, int targetX, int targetY)
    {

        TileType tt = tileType[tiles[targetX, targetY]];

        if (UnitCanEnterTile(targetX, targetY) == false)
            return Mathf.Infinity;

        float cost = tt.movementCost;

        if (sourceX != targetX && sourceY != targetY)
        {
            return cost + 1;
        }
        else
        {
            return cost;
        }

    }
}

