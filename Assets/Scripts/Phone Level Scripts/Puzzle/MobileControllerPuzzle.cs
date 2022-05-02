using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MobileControllerPuzzle : MonoBehaviour
{
    public GameObject lockScreen;
    public GameObject chargingScreen;

    public Image percentageWheel;
    public TextMeshProUGUI percentageText;
    public GameObject screenBreakable;
    public GameObject sparkEx;
    
    private Vector3 offset, initialPos;
    public bool inZ;
    public bool canMoveRight, canMoveLeft;
    public float rayDist = 2;
    private Vector3 startPos, currentPos, moveDir;
    public float moveSpeed;
    public enum Direction {Right, Left, Forward, Backward}
    private bool move;

    public Direction dir;

    private void Start()
    {
        if(dir == Direction.Right) moveDir = Vector3.right;
        else if(dir == Direction.Left) moveDir = Vector3.left;
        else if(dir == Direction.Forward) moveDir = Vector3.forward;
        else if(dir == Direction.Backward) moveDir = -Vector3.forward;
        
        /*switch (dir)
        {
            case Direction.Right: moveDir = Vector3.right;
                break;
            case Direction.Left: moveDir = Vector3.left;
                break;
            case Direction.Up: moveDir = Vector3.up;
                break;
            case Direction.Down: moveDir = Vector3.down;
                break;
        }*/
    }

    private void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right * rayDist, Color.green);
        Debug.DrawRay(transform.position, -transform.right * rayDist, Color.green);
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDist))
        {
            if (hit.transform.tag == "mobile")
            {
                move = false;
                canMoveRight = false;
            }
            
            
        }
        else
        {
            canMoveRight = true;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDist))
        {

            if (hit.transform.tag == "mobile")
            {
                move = false;
                canMoveRight = false;
            }
        }
        else
        {
            canMoveLeft = true;
        }

        if (move)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnMouseDown()
    {
        startPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        /*Vector3 deltaPos = (Input.mousePosition - startPos).normalized;
        var diffX = deltaPos.x;
        var diffY = deltaPos.y;
        
        
        if (canMoveRight)
        {
            if (!inZ && diffX > 0)
            {
                moveDir = Vector3.right;
                move = true;
            }
            else if(inZ && diffY > 0)
            {
                moveDir = Vector3.up;
                move = true;
            }    
        }

        if (canMoveLeft)
        {
            if (!inZ && diffX < 0)
            {
                moveDir = Vector3.left;
                move = true;
            }
            else if(inZ && diffY < 0)
            {
                moveDir = Vector3.down;
                move = true;
            }
        }*/
    }

    private void OnMouseUp()
    {
        AudioManager.instance.PlayClip(AudioManager.instance.slide);
        Vibration.Vibrate(17);
        move = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("halfWayActionTrigger") && !GameManager.instance.gameOver)
        {
            VirtualCameraManager.instance.StartCoroutine(VirtualCameraManager.instance.HalfWayCameraAction());
        }
        if (other.gameObject.CompareTag("charger"))
        {
            other.GetComponent<Collider>().enabled = false;
            Vibration.Vibrate(27);
            StartCoroutine(ChangeToChargingScreen());
            //GetComponent<Drag>().enabled = false;
            moveSpeed = 0;
        }
    }
    IEnumerator ChangeToChargingScreen()
    {

        VirtualCameraManager.instance.cinemachineBrain.m_DefaultBlend.m_Time = 1;
        VirtualCameraManager.instance.phoneLastFocus.Priority = 20;
        yield return new WaitForSeconds(0.5f);
        sparkEx.SetActive(true);
        lockScreen.SetActive(false);
        AudioManager.instance.PlayClip(AudioManager.instance.chargingStarted);
        AudioManager.instance.bgAudioSource.enabled = false;
        chargingScreen.SetActive(true);

        float targetPerc = (float)System.Math.Round(Random.Range(0.5f, 1.0f), 2);
        float chargePerc = 0f;
        while (chargePerc < targetPerc)
        {
            percentageWheel.fillAmount = chargePerc;
            percentageText.SetText(Mathf.Round(chargePerc * 100).ToString() + "%");
            chargePerc += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }

        GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete(1.5f));
    }
}
