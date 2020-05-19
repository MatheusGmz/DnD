using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x;
    public int y;

    public List<Node> neighbours;
    public Node()
    {
        neighbours = new List<Node>();
    }
}
