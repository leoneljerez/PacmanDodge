using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomize : MonoBehaviour
{
    [SerializeField] private Transform target;
   
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "RandomLocation1")
        {
            target.position = new Vector3(target.position.x, 1f, target.position.z);
               }
    }
}
