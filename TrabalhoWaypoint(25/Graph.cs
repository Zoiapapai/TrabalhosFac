using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    List<Node> pathList = new List<Node>();

    public Graph() {}

    public void AddNode(Vector3 id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(Vector3 fromNode, Vector3 toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgeList.Add(e);
        }
    }

    Node FindNode(Vector3 id)
    {
        foreach(Node n in nodes)
        {
            if(n.getId() == id) return n;
        }
        return null;
    }

    public bool AStar(Vector3 startId, Vector3 endId)
    {
        Node start = FindNode(startId);
        Node end = FindNode(endId);

        if(start == null || end == null) return false;

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentativeG_Score = 0;

        bool tentativeIsBetter;
        start.g = 0;
        start.h = distance(start, end);
        start.f = start.h;

        open.Add(start);
        while(open.Count > 0)
        {
            int i = lowestF(open);
            Node thisNode = open[i];
            if(thisNode.getId() == endId)
            {
                ReconstructPath(start, end);
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbour;

            foreach(Edge e in thisNode.edgeList)
            {
                neighbour = e.endNode;
                if(closed.IndexOf(neighbour) > -1) continue;

                tentativeG_Score = thisNode.g + distance(thisNode, neighbour);
                if(open.IndexOf(neighbour) == -1)
                {
                    open.Add(neighbour);
                    tentativeIsBetter = true;
                }
                else if(tentativeG_Score < neighbour.g)
                {
                    tentativeIsBetter = true;
                }
                else
                {
                    tentativeIsBetter = false;
                }

                if(tentativeIsBetter)
                {
                    neighbour.cameFrom = thisNode;
                    neighbour.g = tentativeG_Score;
                    neighbour.h = distance(thisNode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }
            }
        }
        return false;
    }


    public void ReconstructPath(Node startId, Node endId)
    {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;

        while(p!= startId && p != null)
        {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startId);
    }





    float distance(Node a, Node b)
    {
        return (Vector3.SqrMagnitude(a.getId() - b.getId()));
    }

    int lowestF(List<Node> l)
    {
        float lowestF = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestF = l[0].f;

        for(int i = 1; i<l.Count; i++)
        {
            if(l[i].f < lowestF)
            {
                lowestF = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;
    }
}
