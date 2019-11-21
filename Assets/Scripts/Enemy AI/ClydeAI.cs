using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform trans;
    float speed = 1;
    Vector3[] path;
    int targetIndex;


    // Update is called once per frame
    void Update()
    {
        target.position = new Vector3(target.position.x, 0f, target.position.z);
        PathRequest.RequestPath(trans.position, target.position, PathFound);
        
    }

    //when the path has been found , this function will check if the path has been found, it will start the process to follow the path 
    public void PathFound(Vector3[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }
    }

    IEnumerator FollowPath()
    {
        
        //sets the current waypoint to be the first node in the path
        Vector3 WaypointCurrent = path[0];

        while (true)
        {
            if (trans.position == WaypointCurrent)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0;
                    path = new Vector3[0];
                    yield break;
                }

                WaypointCurrent = path[targetIndex];
            }

            //Vector3 targetDirection = WaypointCurrent - trans.position;
            //float step = 2f * Time.deltaTime;
            //Vector3 newDirection = Vector3.RotateTowards(trans.forward, targetDirection, step, 0.0F);
            //trans.rotation = Quaternion.LookRotation(newDirection);
            //moves the character to the target position
            
            trans.position = Vector3.MoveTowards(trans.position, WaypointCurrent, 2f * Time.deltaTime);
            yield return null;
        }
    }

}
