using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGamePressed()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void CreditsPressed()
    {
        
    }
    
    public void ExitPressed()
    {
        Application.Quit();
    }
}
