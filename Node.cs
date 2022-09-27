using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Edge> edgeList = new List<Edge>();
    public Node path = null;
    Vector3 id;
    public float xPos, yPos, zPos;
    public float f, g, h;
    public Node cameFrom;

    public Node(Vector3 i)
    {
        id = i;
        xPos = i.x;
        yPos = i.y;
        zPos = i.z;
        path = null;
    }

    public Vector3 getId()
    {
        return id;
    }
}
