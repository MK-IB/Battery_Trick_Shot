using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailCounterDontDestroy : MonoBehaviour
{
    public static FailCounterDontDestroy instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private int failCounter;
    public void CountFail()
    {
        failCounter++;
        if (failCounter >= 2)
        {
            UIManager.instance.skipButton.SetActive(true);
            failCounter = 0;
        }
    }

}
