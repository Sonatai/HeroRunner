using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DrugController : MonoBehaviour
{

    public GameObject drugModel;
    public Text infoText;
    public Vector2[] spawnLocations;
    private float timer = 0f;
    private int injectionsCount = 0;
    private Vector2 lastPosition;

    public static DrugController i;
    
    void Awake()
    {
        if (i == null)
            i = this;
    }
    
    void Start()
    {
        spawn();
    }

    void FixedUpdate()
    {
        timer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + transform.up*4f);
        transform.position += transform.up * (float)Math.Sin(timer*3f) * 0.01f;
    }

    private void spawn()
    {
        TimeController.i.startTimer(Globals.timerSeconds);
        //Vector2 point;
        //while ((point = spawnLocations[Random.Range(0, spawnLocations.Length)]).Equals(lastPosition)) ;
        int[] spawnpoint = calculateSpawnpoint();
        transform.position = new Vector3(spawnpoint[0],1.6f,spawnpoint[1]);
    }

    private int[] calculateSpawnpoint()
    {
        int[] spawnpoint =
        {
            Random.Range(Globals.gridBorder[0],Globals.gridBorder[1]),
            Random.Range(Globals.gridBorder[2],Globals.gridBorder[3])
        };
        Debug.Log("New Position: X/"+spawnpoint[0] + " Z/" + spawnpoint[1]);
        return spawnpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PlayerContainer"))
        {
            injectionsCount++;
            infoText.text = "Injections: " + injectionsCount;
            spawn();
        }
    }

    public void Reset()
    {
        injectionsCount = 0;
        infoText.text = "Injections: " + injectionsCount;
        spawn();
    }
}
