using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpecificHelp : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }
}
