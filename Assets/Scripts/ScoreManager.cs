using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText = null;

    public static int scores = 0;

    void Update()
    {
        scoreText.text = scores.ToString();
    }

}
