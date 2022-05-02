using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;
    public event Action TangleSolvedEvent;

    private void Awake()
    {
        instance = this;
    }

    public void StartTangleSolvedEvent()
    {
        if (TangleSolvedEvent != null)
        {
            TangleSolvedEvent();
        }
    }
}
