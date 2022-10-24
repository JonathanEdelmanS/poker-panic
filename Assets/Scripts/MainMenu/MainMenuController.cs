using System.Collections;
using System.Collections.Generic;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public int nextScene;

    public float SceneLoadedTime;

    public PokerManager pm;
    public Cooldown p1Cooldown;
    public Cooldown p2Cooldown;
    public PlayerController player1Controller;
    public PlayerController player2Controller;

    public static Vector3 player1Start = new Vector3(0.66f, -0.49f, 0.0f);
    public static Vector3 player2Start = new Vector3(6.24f, -0.49f, 0.0f);
 
    public void PlayGame()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + nextScene);
    }

    public void ResetGame()
    {
        pm.BeginGame();
        p1Cooldown.Reset();
        p2Cooldown.Reset();
        player1Controller.Teleport(player1Start);
        player2Controller.Teleport(player2Start);
    }

    public float getSceneLoadedTime()
    {
          return SceneLoadedTime;
    }

}
