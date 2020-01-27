using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class PowerUpController : MonoBehaviour
{
    
    private float timer = 0f;
    public GameObject joint;
    public GameObject bottle;
    public BoxCollider collider;
    private int type = 0;
    public AudioSource audioSource;
    public AudioClip soundclip;

    public Action onTouch;

    public void init(int type)
    {
        this.type = type;
        
        if (this.type == 0)
        {
            joint.SetActive(false);
        }

        if (this.type == 1)
        {
            bottle.SetActive(false);
        }
    }
    
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + transform.up*4f);
        transform.position += transform.up * (float)Math.Sin(timer*3f) * 0.01f;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PlayerContainer"))
        {
            onTouch.Invoke();
            audioSource.PlayOneShot(soundclip);
            Destroy(joint);
            Destroy(bottle);
            Destroy(collider);
        }
    }

}
