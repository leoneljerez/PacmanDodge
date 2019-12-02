using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGreen : MonoBehaviour
{
    public GameObject GreenEnemy;


    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(50f);

        Instantiate(GreenEnemy, new Vector3(21.77f, 1f, 23f), Quaternion.identity);
    }

}
