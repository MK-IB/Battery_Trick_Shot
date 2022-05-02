using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollower : MonoBehaviour
{
    public static CameraFollower instance;

    public Transform phone;
    public Vector3 offset;
    public float damping;

    [HideInInspector] public bool followPlayer;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        followPlayer = true;
    }
    private void Update()
    {
        if (followPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, phone.position + offset, Time.deltaTime * damping);
        }
        else
        {
            transform.LookAt(phone);
        }
    }

    public void LastRotation()
    {
        followPlayer = false;
        transform.DOMove(new Vector3(0.3f, 1.22f, 50), 2);
        transform.DORotate(new Vector3(82.85f, -180, 0), 2);
    }
}
