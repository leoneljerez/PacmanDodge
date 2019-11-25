using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIpathing : MonoBehaviour
{
    [SerializeField] private Transform trans;
    [SerializeField] public Transform[] waypoints;
    int curW = 0;
    public float speed = 2f;

    void Update ()
    {
        if (trans.position != waypoints[curW].position)
        {
            trans.position = Vector3.MoveTowards(trans.position, waypoints[curW].position, speed * Time.deltaTime);

        }

        else
        {
            curW = (curW + 1) % waypoints.Length;
          
        }
    
    }
}
