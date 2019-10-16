using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text timetext = null;

    private float seconds;

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.performed)
        {
            seconds += Time.deltaTime;
        
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            string currTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            timetext.text = currTime;
        }
       
    }
}
