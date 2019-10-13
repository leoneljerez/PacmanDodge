using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPowerPellet : MonoBehaviour
{

    [SerializeField] private AudioSource audiodata;
    [SerializeField] private GameObject scoreText = null;
    [SerializeField] private int point= 0;

    // Start is called before the first frame update
    void Start()
    {
        audiodata = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Random turn on/off power pellet halo
        float num = Random.Range(0.0f, 10f);
        if (num > 5.0f && num < 5.1f)
        {
            Behaviour halo = (Behaviour) GetComponent("Halo");
            halo.enabled = true;
        }
        else if (num > 0.0f && num < 0.1f)
        {
            Behaviour halo = (Behaviour) GetComponent("Halo");
            halo.enabled = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            ScoreManager.scores += point;
            audiodata.Play();
            ShowFloatingText();
            gameObject.GetComponent<Renderer>().enabled = false;
            (gameObject.GetComponent(typeof(SphereCollider)) as Collider).enabled = false;
            gameObject.GetComponent<Behaviour>().enabled = false;
            Destroy(gameObject, audiodata.clip.length + 1f); // Keep + 1f to leave a trail when the floating text shows
        }
        
    }

    void ShowFloatingText()
    {
        var go = Instantiate(scoreText, transform.position, Quaternion.identity, transform);
        go.GetComponent<TextMesh>().text = point.ToString();
    }
}
