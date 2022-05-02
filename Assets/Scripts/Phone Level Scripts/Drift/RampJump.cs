using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RampJump : MonoBehaviour
{
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ramp"))
        {
            //other.GetComponent<Collider>().enabled = false;
            transform.position = new Vector3(transform.position.x, other.transform.position.y + 1.2f, transform.position.z);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ramp"))
        {
            Vector3 newPos = new Vector3(transform.position.x, initialPos.y, transform.position.z);
            transform.DOMove(newPos, 0.5f);
        }
    }
}
