using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController i;
    
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
    }

    public void Reset()
    {
        DOTween.KillAll();
        PlayerController.i.Reset();
        DrugController.i.Reset();
        CameraShaderController.i.Reset();
        
    }
}
