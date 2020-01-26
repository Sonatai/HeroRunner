﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    public static TimeController i;
    
    public RectTransform timeElapsedBar;
    public Text timeText;
    
    private float maxWidth;
    private float timeLeft;
    private float maxTime;
    private bool timerStarted;

    public Action onFinished;

    void Awake()
    {
        if (i == null)
            i = this;
    }
    void Start()
    {
        maxWidth = timeElapsedBar.sizeDelta.x;
        timerStarted = false;
        
        //startTimer(Globals.timerSeconds);
    }

    void FixedUpdate()
    {
        if (!timerStarted) return;
        
        timeElapsedBar.sizeDelta = new Vector3(maxWidth*(timeLeft/maxTime),14);
        timeText.text = ((int)timeLeft).ToString();
        if (timeLeft <= 0)
        {
            timerStarted = false;
            onFinished?.Invoke();
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }
    public void startTimer(int seconds)

    {
        maxTime = seconds;
        timeLeft = maxTime;
        timeText.text = ((int)timeLeft).ToString();
        timerStarted = true;
    }
}
