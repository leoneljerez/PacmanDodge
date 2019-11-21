using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequest : MonoBehaviour
{
    Queue<PathRequesting> pathRequestQueue = new Queue<PathRequesting>();

    //stores current Path request
    PathRequesting currentPathRequest;

    static PathRequest instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        //creates a reference to the Pathfinding class
        pathfinding = GetComponent<Pathfinding>();
    }
    //function needed to request the path using the starting position of the path, the ending position of the path, and the callback
    public static void RequestPath(Vector3 pathStartPos, Vector3 pathEndPos, Action<Vector3[], bool> callback)
    {
        //intitiates a new request
        PathRequesting newRequest = new PathRequesting(pathStartPos, pathEndPos, callback);

        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    //function used to process and start the path using a Queue
    void TryProcessNext()
    {
        if (!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartThePath(currentPathRequest.pathStartPos, currentPathRequest.pathEndPos);
        }
    }

    //function used after the path has been processed 
    public void PathFinishedProcessing(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    //struct needed to help with the path being requested, as it holds the starting postion, ending position, and callback
    struct PathRequesting
    {
        public Vector3 pathStartPos;
        public Vector3 pathEndPos;
        public Action<Vector3[], bool> callback;

        //actual method for pathrequesting 
        public PathRequesting(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStartPos = _start;
            pathEndPos = _end;
            callback = _callback;
        }
    }

}
