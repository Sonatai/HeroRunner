using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrugController : MonoBehaviour
{

    public GameObject drugModel;
    public Vector2[] spawnLocations;
    private float timer = 0f;
    void Start()
    {
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + transform.up*4f);
        transform.position += transform.up * (float)Math.Sin(timer*3f) * 0.01f;
    }

    private void spawn()
    {
        Vector2 point = spawnLocations[Random.Range(0, spawnLocations.Length)];
        transform.position = new Vector3(point.x,1.4f,point.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDED!");
        if (other.gameObject.name.Equals("PlayerContainer"))
            spawn();
    }
}
