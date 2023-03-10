using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Node currentNode;
    public List<Node> prevNode = new List<Node>();
    public float cost = 0;
    public Path(Node n1, List<Node> n2)
    {
        currentNode = n1;
        prevNode = n2;
    }
    public Path()
    {
    }
}
