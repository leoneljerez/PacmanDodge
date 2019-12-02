using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGreen2 : MonoBehaviour
{
    public GameObject GreenEnemy;


    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(60f);

        Instantiate(GreenEnemy, new Vector3(-22f, 1f, -3.3f), Quaternion.identity);
    }

}

