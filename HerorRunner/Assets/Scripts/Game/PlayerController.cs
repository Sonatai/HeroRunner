using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{

    public Animation animation;
    public CharacterController cController;
    private Vector3 cPosition;

    private bool isMoving = true;
    
    private bool isRotating = false;
    private bool searchForCenter = false;
    private int centerFrames = 30; //amount of frames it takes to center
    private int currentCenterFrames = 0;
    private float centerAmount = 0;
    private float turnSpeed = 0.2f;
    private float runSpeed = 0.1f;

    void Start()
    {
        animation.wrapMode = WrapMode.Loop;
        animation.Play("run");
    }

    void FixedUpdate()
    {
        if (currentCenterFrames < centerFrames)
        {
            currentCenterFrames++;
        }
        else
        {
            centerAmount = 0;
        }
        if(isMoving)
            cController.Move(transform.forward*runSpeed +  transform.right*centerAmount);

        //centerDiff = 0;
    }

    private void Update()
    {
        cPosition = transform.position;
        bool leftWall = false;
        bool rightWall = false;
        Debug.DrawRay(cPosition, transform.forward*3);
        if (!isRotating)
        {
            if (!Physics.Raycast(cPosition, transform.right, 2.5f))
            {
                if (Input.GetKeyDown("d"))
                {
                    isRotating = true;
                    transform.DOLocalRotate(transform.up * (transform.rotation.eulerAngles.y + 90f),
                        turnSpeed).OnComplete(delegate() { isRotating = false; searchForCenter = true;});
                }
                    
            }
            else
            {
                rightWall = true;
            }

            if (!Physics.Raycast(cPosition, transform.right * (-1), 2.5f))
            {
                if (Input.GetKeyDown("a"))
                {
                    isRotating = true;
                    transform.DOLocalRotate(transform.up * (transform.rotation.eulerAngles.y - 90f),
                        turnSpeed).OnComplete(delegate() { isRotating = false;
                        searchForCenter = true;
                    });
                }
            }
            else
            {
                leftWall = true;
            }

            if (Physics.Raycast(cPosition, transform.forward, 1.5f) && leftWall && rightWall) //player hits dead end
            {
                isMoving = false;
                transform.DOLocalRotate(transform.up * (transform.rotation.eulerAngles.y - 180f), turnSpeed*2);//.OnComplete(delegate() { isRotating = false;
                //     searchForCenter = true;
                //     isMoving = true;
                // });
            }
            
            if(searchForCenter && !isRotating && rightWall && leftWall)
                center();
        }

    }

    private void center()
    {
        RaycastHit left;
        Ray leftRay = new Ray(transform.position,transform.right * (-1));
        RaycastHit right;
        Ray rightRay = new Ray(transform.position,transform.right);
        float leftDist = 0;
        float rightDist = 0;
        if (Physics.Raycast(leftRay, out left))
        {
            leftDist = left.distance;
        }

        if (Physics.Raycast(rightRay,out right))
        {
            rightDist = right.distance;
        }

        float totalDist = leftDist + rightDist;
        float diff = totalDist / 2 - leftDist;
        centerAmount = diff / (float) centerFrames;
        currentCenterFrames = 0;
        searchForCenter = false;
    }
    
//    private void OnTriggerEnter(Collider other)
//    {
//        cController.Move(transform.up*10f);
//    }
}