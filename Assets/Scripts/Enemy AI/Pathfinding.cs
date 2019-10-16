using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void FindPath(Vector3 startingPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromPointInWorld(startingPos);
        Node targetNode = grid.NodeFromPointInWorld(targetPos);

        Heap<Node> openList = new Heap<Node>(grid.MaximumSize);
        HashSet<Node> closedList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList.RemoveFirst();
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetraceThePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if(!neighbor.walkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                int CostToMoveToNeighbor = currentNode.Gcost + GetDist(currentNode, neighbor);
                if(CostToMoveToNeighbor < neighbor.Gcost || !openList.Contains(neighbor))
                {
                    neighbor.Gcost = CostToMoveToNeighbor;
                    neighbor.Hcost = GetDist(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }

                }
            }
        }


        void RetraceThePath(Node startingNode, Node endingNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = endingNode;

            while(currentNode != startingNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();
        }

        int GetDist(Node NodeA, Node NodeB)
        {
            int distanceX = Mathf.Abs(NodeA.gridX - NodeB.gridX);
            int distanceY = Mathf.Abs(NodeA.gridY - NodeB.gridY);

            if(distanceX > distanceY)
            {
                return 14 * distanceY + 10 * (distanceX - distanceY);
            }
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }

    }



}
