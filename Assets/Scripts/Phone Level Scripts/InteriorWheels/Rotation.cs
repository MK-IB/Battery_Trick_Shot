using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float speed;
    public Vector3 dir = Vector3.right;

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.Rotate(dir, speed * Time.deltaTime, Space.World);
    }
}
