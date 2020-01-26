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
        Vector2 point;
        while ((point = spawnLocations[Random.Range(0, spawnLocations.Length)]).Equals(lastPosition)) ;
        transform.position = new Vector3(point.x,1.4f,point.y);
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
