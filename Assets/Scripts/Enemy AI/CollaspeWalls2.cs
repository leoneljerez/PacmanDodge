using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollaspeWalls2 : MonoBehaviour
{
    [SerializeField] private Transform trans;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Collaspe());
        StopCoroutine(Collaspe());
    }

    IEnumerator Collaspe()
    {
        yield return new WaitForSeconds(20f);

        trans.position = new Vector3(trans.position.x, -4, trans.position.z);

    }
}
