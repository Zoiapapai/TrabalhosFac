using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public struct Link
    {
        public enum direction {UNI, BI}
        public Vector3 node1;
        public Vector3 node2;
        public direction dir;
    }
public class GraphWAypoint : MonoBehaviour
{
    Camera cam;
    [SerializeField] List<Vector3> waypoints;
    Vector3 currentWaypoint;
    [SerializeField] float speed;
    int id = 0;
    bool canMove = false;
    public GameObject wpPrefab;
    public List<GameObject> wpBallList = new List<GameObject>();  


    public Link[] links;
    public Graph graph = new Graph();

    void Start()
    {
        cam = Camera.main;
        currentWaypoint = transform.position;
        waypoints.Clear();
        id = 0;
    }
    public void StartMove()
    {
        canMove = true;
    }
    public void StopMove()
    {
        canMove = false;
    }
    public void ClearWaypints()
    {
        waypoints.Clear();
        int max  = wpBallList.Count;
        for (int i = 0; i < max; i++)
        {   
            Destroy(wpBallList[0]);
            wpBallList.RemoveAt(0);
        }
        
    }
    void Update()
    {
        RayCastDestination();
        if(canMove == true)
        {
            FollowWaypoints();
        }
    }
    
    void RayCastDestination() // Apertando com o mouse, você seta um waypoint novo
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.tag == "Chão")
                {
                    waypoints.Add(hit.point);
                    GameObject wpBalls = (GameObject)Instantiate(wpPrefab, hit.point, Quaternion.identity);
                    wpBallList.Add(wpBalls);
                }
                /*if(hit == true)
                {
                    this.GameObject.transform.position = Vector3.MoveTowards(!transform.position, currentWaypoint, speed * Time.deltaTime);
                }
                */
            }
        }
    }

    void FollowWaypoints() // O player vai seguir os pontos
    {
        if(waypoints.Count <= id) canMove = false;
        else canMove = true;
        if(transform.position == currentWaypoint && canMove)
        {
            currentWaypoint = waypoints[id];
            id++;
        }
        else
        {   
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
        }
    }


    void StartGraph()
    {
        if(waypoints.Count > 0)
        {
            foreach(Vector3 wp in waypoints)
            {
                graph.AddNode(wp);
            }
            foreach(Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);
                if(l.dir == Link.direction.BI) graph.AddEdge(l.node2, l.node1);
            }
        }
    }
}