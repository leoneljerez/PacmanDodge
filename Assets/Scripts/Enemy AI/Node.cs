using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{

    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;

    public int Gcost;
    public int Hcost;
    public Node parent;
    int heapIndex;

    //function that will create the nodes for the grid 
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    //defines what the Fcost will be in the game
    public int Fcost
    {
        get
        {
            return Gcost - Hcost;
        }
    }
    //uses the heap index to help keep track of 
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    //function used to compare two nodes, involving their F cost as well as H cost
    public int CompareTo(Node nodeComparingTo)
    {
        int compare = Fcost.CompareTo(nodeComparingTo.Fcost);
        if (compare == 0)
        {
            compare = Hcost.CompareTo(nodeComparingTo.Hcost);
        }

        return -compare;
    }
}

