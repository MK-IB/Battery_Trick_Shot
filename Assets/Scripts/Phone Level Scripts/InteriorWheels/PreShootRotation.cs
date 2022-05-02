using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class PreShootRotation : MonoBehaviour
{
    private float yInput;
    public float sensitivity;
    public Vector3 dir = Vector3.right;
    public string mouseAxis = "Mouse Y";
    
    private void Update()
    {
        yInput = Input.GetMouseButton(0) ? Input.GetAxis(mouseAxis) : 0;
        transform.Rotate(dir * yInput * sensitivity * Time.deltaTime, Space.World);
    }
}
