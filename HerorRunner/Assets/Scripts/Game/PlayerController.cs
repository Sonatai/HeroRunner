using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour
{
    public static PlayerController i;
    public Animation animation;
    public CharacterController cController;
    public Camera camera;
    private Vector3 cPosition;

    public AudioSource audioSource;
    public AudioClip smallJumpSound;
    public AudioClip bigJumpSound;
    public AudioMixer audioMixer;

    public bool isMoving = true;
    
    private bool isRotating = false;
    private bool searchForCenter = false;
    private int centerFrames = 30; //amount of frames it takes to center
    private int currentCenterFrames = 0;
    private float centerAmount = 0;
    private float turnSpeed = 0.2f;
    private float runSpeed = 0.1f;

    private float currentEarthAcc = 0;
    private bool isJumping = true;
    private bool isSpeeding = false;
    private float jumpPressedTime = 0;
    private Quaternion originalRoatation;

    void Awake()
    {
        if (i == null)
            i = this;
    }

    private void Start()
    {
        originalRoatation = camera.transform.localRotation;
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


        currentEarthAcc += Globals.gravity;

        if(isMoving)
            cController.Move(transform.forward*runSpeed +  transform.right*centerAmount + transform.up*currentEarthAcc);

    }

    public void Reset()
    {
        isMoving = false;
        transform.position = new Vector3(34,16,32);
        MapController.i.mapFog(false);
        DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
        {
            isMoving = true;
            isJumping = true;
            runSpeed = 0.1f;
            currentEarthAcc = 0;
            animation.wrapMode = WrapMode.Once;
            animation.Play("flip");
            jumpPressedTime = Time.time;
        });
    }

    private void Update()
    {
        cPosition = transform.position;
        bool leftWall = false;
        bool rightWall = false;
        
        if (cController.isGrounded && isJumping && (Time.time - jumpPressedTime) >= 0.1f)
        {
            Debug.Log("JUMP LANDED");
            camera.transform.DOLocalRotate(originalRoatation.eulerAngles,1f);
            animation.wrapMode = WrapMode.Loop;
            animation.Play("run");
            isJumping = false;
            audioMixer.DOSetFloat("HighPass", 10, 1);
            MapController.i.mapFog(false);
        }
        if (!isRotating)
        {


            if (!Physics.Raycast(cPosition, transform.right, 3f) || isJumping)
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

            if (!Physics.Raycast(cPosition, transform.right * (-1), 3f) || isJumping)
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

            if (Input.GetKeyDown("space") && isJumping == false)
            {
                isJumping = true;
                animation.Stop();
                animation.wrapMode = WrapMode.Once;
                animation.Play("flip");
                jumpPressedTime = Time.time;
                currentEarthAcc = 0.2f;
                //MapController.i.mapFog(true);
                TimeController.i.reduceTime(10);
                audioSource.PlayOneShot(smallJumpSound);

            }
            
            if (Input.GetKeyDown("m") && isJumping == false && Globals.bigJumps > 0)
            {
                Globals.bigJumps--;
                isJumping = true;
                animation.Stop();
                animation.wrapMode = WrapMode.Once;
                animation.Play("flip");
                jumpPressedTime = Time.time;
                currentEarthAcc = 1f;
                MapController.i.mapFog(true);
                camera.transform.DOLocalRotate(originalRoatation.eulerAngles + new Vector3(30, 0, 0),1f);
                audioMixer.DOSetFloat("HighPass", 4000, 3);
                audioSource.PlayOneShot(bigJumpSound);
            }
            
            if (Input.GetKeyDown("n") && isSpeeding == false && Globals.speedUps > 0)
            {
                Globals.speedUps--;
                isSpeeding = true;
                runSpeed = 0.3f;
                audioMixer.DOSetFloat("Pitch", 2, 5);
                DOTween.Sequence().AppendInterval(10f).AppendCallback(delegate
                {
                    audioMixer.DOSetFloat("Pitch", 1, 0.5f);
                    runSpeed = 0.1f;
                    isSpeeding = false;
                });

            }

            if (Physics.Raycast(cPosition, transform.forward, 1f) && leftWall && rightWall &&!isRotating) //player hits dead end
            {
                isMoving = false;
                isRotating = true;
                transform.DOLocalRotate(transform.up * (transform.rotation.eulerAngles.y - 180f), turnSpeed*2).OnComplete(delegate() { isRotating = false;
                     searchForCenter = true;
                     isMoving = true;
                     isRotating = false;
                });
            }
            
            if(searchForCenter && !isRotating && rightWall && leftWall)
                center();

            if(!isRotating)
                snapRotation();
        }

    }

    private void snapRotation()
    {

        float angle = transform.rotation.eulerAngles.y;
        angle = Mathf.Round(angle / 90.0f) * 90.0f;
        if (Math.Abs((double)transform.rotation.eulerAngles.y - (double)angle) > 0.1f)
        {
            transform.DOLocalRotate(transform.up * (angle), turnSpeed*2);
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

}