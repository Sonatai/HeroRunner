using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
  
    public static int timerSeconds = 90;
    public static float gravity = -0.005f;

    public static int[] gridBorder =
    {
        0,64,0,64
    };

    public static int injections = 0;
    public static int bigJumps = 0;
    public static int speedUps = 0;
}
