using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControlle : MonoBehaviour
{

    public float watchdog;
    public Slider indicator;
    private float time;
    void Start()
    {
        //indicator.SetValueWithoutNotify(0.5f);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > watchdog)
            time = 0;
        
        //indicator.value = time / watchdog;
    }
}
