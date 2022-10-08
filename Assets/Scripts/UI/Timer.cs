using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Platformer.Mechanics;

public class Timer : MonoBehaviour
{
    public Text timer;
    public PlayerController player1;
    public PlayerController player2;
    public float startTime;

    void Update()
    {
        float timeLeft = Mathf.Max(startTime - Time.time, 0);
        timer.text = timeLeft.ToString();
        if (timeLeft == 0)
        {
            player1.controlEnabled = false;
            player2.controlEnabled = false;
        }
    }
}
