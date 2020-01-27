using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{

    public static GameController i;
    public GameObject powerUpPrefab;

    public List<PowerUpController> powerups;
    public Text infoText;

    
    private Action onSpeedUp = delegate { Globals.speedUps++; };
    
    private Action onBigJump = delegate { Globals.bigJumps++; };
    
    void Awake()
    {
        if (i == null)
            i = this;
    }
    
    void Start()
    {
        TimeController.i.onFinished = delegate()
        {
            PlayerController.i.isMoving = false;
            PlayerController.i.animation.Stop();
            GameOverPanelController.i.fadeIn();
        };
        
        powerups = new List<PowerUpController>();

        for (int i = 0; i < 5; i++)
        {
            powerups.Add(spawnPowerUp(0, calculateSpawnpoint()));
            powerups.Add(spawnPowerUp(1, calculateSpawnpoint()));
            
        }
    }

    private void Update()
    {
        infoText.text = "Injections: " + Globals.injections + "\nBig Jumps [m]: " + Globals.bigJumps  + "\nSpeed Ups [n]: " + Globals.speedUps;
    }

    public void Reset()
    {
        Globals.injections = 0;
        Globals.bigJumps = 0;
        Globals.speedUps = 0;
        
        DOTween.KillAll();
        PlayerController.i.Reset();
        DrugController.i.Reset();
        CameraShaderController.i.Reset();
        foreach (var c in powerups)
        {
            powerups.Remove(c);
            Destroy(c.gameObject);
        }
        
    }
    
    public static Vector2 calculateSpawnpoint()
    {
        Vector2 spawnpoint = new Vector2(Random.Range(Globals.gridBorder[0],Globals.gridBorder[1])+3f,
            Random.Range(Globals.gridBorder[2],Globals.gridBorder[3]+3f));
        
        Debug.Log("New Position: X/"+spawnpoint.x + " Z/" + spawnpoint.y);
        return spawnpoint;
    }


    public PowerUpController spawnPowerUp(int type, Vector2 position)
    {
        GameObject powerup = Instantiate(powerUpPrefab);
        powerup.transform.position = new Vector3(position.x,1.6f,position.y);
        PowerUpController controller = powerup.GetComponent<PowerUpController>();
        controller.init(type);
        if (type == 0)
            controller.onTouch = onBigJump;
        if (type == 1)
            controller.onTouch = onSpeedUp;
        return controller;
    }
}
