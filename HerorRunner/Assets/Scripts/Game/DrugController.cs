using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DrugController : MonoBehaviour
{

    public GameObject drugModel;
    public Vector2[] spawnLocations;
    private float timer = 0f;
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
        Vector2 spawnpoint = GameController.calculateSpawnpoint();
        transform.position = new Vector3(spawnpoint.x,1.6f,spawnpoint.y);
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PlayerContainer"))
        {
            Globals.injections++;
            spawn();
        }
    }

    public void Reset()
    {
        spawn();
    }
}
