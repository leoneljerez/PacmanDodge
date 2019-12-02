using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnClyde2 : MonoBehaviour
{
    public GameObject Clyde;


    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(70f);

        Instantiate(Clyde, new Vector3(-0.41f, 1f, -23f), Quaternion.identity);
    }

}
