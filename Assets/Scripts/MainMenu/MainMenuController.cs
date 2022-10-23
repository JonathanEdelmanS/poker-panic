using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public int nextScene;

    public float SceneLoadedTime;
 
    public void PlayGame()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + nextScene);
    }

    public float getSceneLoadedTime()
    {
          return SceneLoadedTime;
    }

}
