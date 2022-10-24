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
    HashSet<float> passedTimes;
    public float duration;
    float startTime;
    public bool debugMode;
    public bool active = false;

    public void StartTimer()
    {
        active = true;
        startTime = Time.time;
        passedTimes = new HashSet<float>();
    }

    public void StopTimer()
    {
        active = false;
    }

    void Update()
    {
        if (debugMode) {
            player1.controlEnabled = true;
            player2.controlEnabled = true;
            return;
        }
        if (!active || passedTimes == null) {
            return;
        }
        player1.controlEnabled = true;
        player2.controlEnabled = true;
        float timeElapsed = Time.time - startTime;
        float timeLeft = Mathf.Max(duration - timeElapsed, 0);
        timer.text = Mathf.Round(timeLeft).ToString();
        for (int i = 0; i < 4; i++) {
            float time = pokerManager.times[i]; 
            if (!passedTimes.Contains(time) && timeElapsed >= time) {
                passedTimes.Add(time);
                pokerManager.FlipStreetCard(i);
            }
        }
        if (timeLeft <= 0)
        {
            player1.controlEnabled = false;
            player2.controlEnabled = false;
            var winner = pokerManager.CompareDecks();
            if (winner == -1)
                timer.text = "No winner!";
            if (winner == 0)
                timer.text = "Tie";
            if (winner == 1)
                timer.text = "Player 1 (white) wins";
            if (winner == 2)
                timer.text = "Player 2 (green) wins";
        }
    }
}
