using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelController : MonoBehaviour
{
    public static GameOverPanelController i;

    void Awake()
    {
        if (i == null)
            i = this;
        
        fadeOut();
    }

    public void retryPressed()
    {
        GameController.i.Reset();
        fadeOut();
    }

    public void exitPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void fadeIn()
    {
        gameObject.SetActive(true);
    }

    public void fadeOut()
    {
        gameObject.SetActive(false);
    }
}
