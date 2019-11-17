using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animation animation;
    public CharacterController cController;
    void Start()
    {
        animation.wrapMode = WrapMode.Loop;
        animation.Play("run");
    }

    void FixedUpdate()
    {
        cController.Move(transform.forward*0.1f);
    }
}
