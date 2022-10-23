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
    public PokerManager pokerManager;
    public float startTime;
    public bool debugMode;

    void Update()
    {
        if (debugMode) {
            player1.controlEnabled = true;
            player2.controlEnabled = true;
            return;
        }
        float timeLeft = Mathf.Max(startTime - Time.time, 0);
        timer.text = Mathf.Round(timeLeft).ToString();
        if (timeLeft == 0)
        {
            player1.controlEnabled = false;
            player2.controlEnabled = false;
            int x = pokerManager.CompareDecks();
            if (x == 0)
                timer.text = "Players tied!";
            if (x == 1)
                timer.text = "Player 1 (white) wins!";
            if (x == 2)
                timer.text = "Player 2 (green) wins!";
        }
    }
}
