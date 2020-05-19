using Assets.Scripts.UnitScript;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : Actor
{
    public Button useStandardActionButton;
    public Button useMoveActionButton;
    public Button useMinorActionButton;

    public Button moveNext;

    public int tileX;
    public int tileY;
    public Map tileMap;

    public List<Node> currentPath = null;

    public Vector3 destination;
    public int speed = 6;
    float moveSpeed = 1;
    public int maxMoves = 6;
    public int movedSpaces = 0;

    public bool isMoving;

    public bool isUsingStandardAction;
    public bool isUsingMoveAction;
    public bool isUsingMinorAction;
    public bool isUsingFreeAction;
    public bool isUsingActionPoint;

    void Start()
    {
    }
    void Update()
    {
        if (currentPath != null)
        {
            int currNode = 0;
            if (currentPath.Count - 1 <= maxMoves && movedSpaces < maxMoves)
            {

                while (currNode < currentPath.Count - 1)
                {
                    if(tileMap == null)
                    {
                        Debug.LogError("tileMap is null!");
                    }
                    Vector3 start = tileMap.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 1f, 0);
                    Vector3 end = tileMap.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].y) + new Vector3(0, 1f, 0);

                    Debug.DrawLine(start, end, Color.red);

                    currNode++;
                }
            }
            else
            {
                Debug.Log("Too far!");
                currentPath = null;
            }

        }
    }

    public void ResetMovement()
    {
        movedSpaces = 0;
    }
    public void MoveNextTile()
    {

        float remaingmovement = moveSpeed;
        while (remaingmovement > 0)
        {
            if (this.currentPath == null)
                return;
            var tilecost = tileMap.CostToEnterTile(this.currentPath[0].x, this.currentPath[0].y, this.currentPath[1].x, this.currentPath[1].y);

            if ((int)tilecost + movedSpaces > maxMoves)
            {
                break;
            }
            remaingmovement -= tilecost;
            movedSpaces += (int)tileMap.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

            this.tileX = currentPath[1].x;
            tileY = currentPath[1].y;

            transform.position = tileMap.TileCoordToWorldCoord(tileX, tileY);
            Debug.Log("moved spaces: " + movedSpaces);
            Debug.Log("stepped cost: " + tileMap.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y));
            Thread.Sleep(500);

            currentPath.RemoveAt(0);

            if (currentPath.Count == 1)
            {
                currentPath = null;
            }
            if (movedSpaces >= maxMoves)
            {
                currentPath = null;
            }
        }
    }
}
