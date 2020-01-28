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
    [SerializeField] private List<Transform> listOfSpawnpoints;
    private static List<Transform> useableSpawnpoints;

    private Action onSpeedUp = delegate { Globals.bigJumps++; };
    
    private Action onBigJump = delegate { Globals.speedUps++; };
    
    void Awake()
    {
        if (i == null)
            i = this;
    }
    
    void Start()
    {
        useableSpawnpoints = new List<Transform>();
        foreach (Transform spawnpoint in listOfSpawnpoints)
        {
            useableSpawnpoints.Add(spawnpoint);
        }
        TimeController.i.onFinished = delegate()
        {
            PlayerController.i.isMoving = false;
            PlayerController.i.animation.Stop();
            GameOverPanelController.i.fadeIn();
        };
        
        Reset();
        
    }

    private void Update()
    {
        infoText.text = "Injections: " + Globals.injections + "\nWeed [m]: " + Globals.bigJumps  + "\nSpeed [n]: " + Globals.speedUps;
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
            Destroy(c.gameObject);
        powerups.Clear();
        
        powerups = new List<PowerUpController>();

        for (int i = 0; i < 7; i++)
        {
            powerups.Add(spawnPowerUp(0, calculateSpawnpoint()));
            powerups.Add(spawnPowerUp(1, calculateSpawnpoint()));
            
        }

    }
    
    public static Vector2 calculateSpawnpoint()
    {
        if (useableSpawnpoints == null)
            return new Vector2(-12.39f,19.62f);
        Transform choosenSpawnpoint = useableSpawnpoints[Random.Range(0, useableSpawnpoints.Count)];
        /*Vector2 spawnpoint = new Vector2(Random.Range(Globals.gridBorder[0],Globals.gridBorder[1])+3f,
            Random.Range(Globals.gridBorder[2],Globals.gridBorder[3]+3f));*/
        //Vector2 spawnpoint = new Vector2(choosenSpawnpoint.position.x, choosenSpawnpoint.position.z);
        Vector2 spawnpoint = new Vector2(((int)Random.Range(0,15)*4)+2,((int)Random.Range(0,15)*4)+2);
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
