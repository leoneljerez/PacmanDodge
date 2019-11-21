using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    //PathRequest request;
    PathRequest request;
    Grid grid; 

    void Awake()
    {
        request = GetComponent<PathRequest>();
        grid = GetComponent<Grid>();

    }

  
    //starts the coroutine for the path based on the starting position and target position 
    public void StartThePath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccessful = false;

        Node startNode = grid.NodeFromPointInWorld(startPos);
        Node targetNode = grid.NodeFromPointInWorld(targetPos);
        //as long as the node is walkable, the rest of the code in the function will run. otherwise, it will not
        if (targetNode.walkable)
        {
            //uses heap to store the information about the nodes
            Heap<Node> openList = new Heap<Node>(grid.MaximumSize);
            HashSet<Node> closedList = new HashSet<Node>();
            openList.Add(startNode);

            while (openList.Count > 0)
            {
                Node currentNode = openList.RemoveFirst();
                closedList.Add(currentNode);
                //if we have reached our destination 
                if (currentNode == targetNode)
                {
                    pathSuccessful = true;
                    break;
                }
                //finds the neighbors of the current Node
                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    //checks to make sure the neighbor node is walkable and that the closed list has the neighbor node
                    if (!neighbor.walkable || closedList.Contains(neighbor))
                    {
                        continue;
                    }
                    //sets the cost to move to the neighbror node as well as grabs the distance between the current Node and the neighbor
                    int CostToMoveToNeighbor = currentNode.Gcost + GetDist(currentNode, neighbor);
                    if (CostToMoveToNeighbor < neighbor.Gcost || !openList.Contains(neighbor))
                    {
                        //sets the G cost, the H cost, and the Parent for the current node
                        neighbor.Gcost = CostToMoveToNeighbor;
                        neighbor.Hcost = GetDist(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openList.Contains(neighbor))
                        {
                            openList.Add(neighbor);
                        }

                        else
                        {
                            openList.UpdateItem(neighbor);
                        }

                    }
                }
            }
        }
        yield return null;
        if (pathSuccessful)
        {
            waypoints = RetraceThePath(startNode, targetNode);
        }
        request.PathFinishedProcessing(waypoints, pathSuccessful);
    }
           
        Vector3[] RetraceThePath(Node startingNode, Node endingNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endingNode;

            while (currentNode != startingNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            if(currentNode == startingNode)
        {
            path.Add(currentNode);
        }
        Vector3[] waypoint = SimplifyingThePath(path);
        //reverses the path so that it will be in the right order
        Array.Reverse(waypoint);
        return waypoint;
        }

    //function used that will make the path more accurate and smooth for the invisible leader 
    Vector3[] SimplifyingThePath(List<Node> path)
    {
        List<Vector3> waypoint = new List<Vector3>();
        Vector2 OldDirection = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 NewDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (NewDirection != OldDirection)
            {
                waypoint.Add(path[i].worldPosition);
            }
            OldDirection = NewDirection;
        }
        return waypoint.ToArray();
    }

    // function used that will get the distance between two nodes 
    int GetDist(Node NodeA, Node NodeB)
        {
            int distanceX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
            int distanceY = Mathf.Abs(NodeA.gridY - NodeB.gridY);

            if (distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

}
