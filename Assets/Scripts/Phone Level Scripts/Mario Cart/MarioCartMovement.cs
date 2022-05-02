using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines.Primitives;
using UnityEngine;

public class MarioCartMovement : MonoBehaviour
{
    public float speed;
    
    private bool moveCart;
    private bool isMoving;
    private Transform mobile;
    private Transform charger;
    
    private bool isCrossedArea;
    private bool isCharged;
    
    private void Start()
    {
        mobile = transform.GetChild(0);
        charger = GameObject.FindWithTag("charger").transform;
        moveCart = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCharged && !isCrossedArea)
        {
            if (TapScale.instance.isCorrectTime)
            {
                moveCart = false;
                MoveMobileToCharger();
            }
            isCharged = true;
        }
        
        if (moveCart)
        {
            transform.Translate(-Vector3.up * speed * Time.deltaTime);    
        }
    }

    void MoveMobileToCharger()
    {
        transform.DORotate(new Vector3(-90, 0, -85), 0.7f);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            mobile.parent = null;
            Vector3 movePos = new Vector3(charger.position.x, -0.04f, charger.position.z);
            mobile.DOMove(movePos, 0.6f);
            mobile.DORotate(new Vector3(-90, -50, -80), 0.2f);
        });
        StartCoroutine(StopTapScaleAnimation());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("halfWayActionTrigger"))
        {
            isCrossedArea = true;
        }
    }
    IEnumerator StopTapScaleAnimation()
    {
        Transform tapScale = TapScale.instance.transform;
        tapScale.GetComponent<Animator>().enabled = false;
      
        yield return new WaitForSeconds(0.7f);
        tapScale.parent.gameObject.SetActive(false);
    }
    public IEnumerator StopCartMovement()
    {
        yield return new WaitForSeconds(1.5f);
        moveCart = false;
    }
}
