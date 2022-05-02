using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScale : MonoBehaviour
{
    public static TapScale instance;
    [HideInInspector] public bool isCorrectTime;

    private void Awake()
    {
        instance = this;
    }
    
    public void IsCorrectTime()
    {
        isCorrectTime = true;
    }
    public void IsNotCorrectTime()
    {
        isCorrectTime = false;
    }
}
