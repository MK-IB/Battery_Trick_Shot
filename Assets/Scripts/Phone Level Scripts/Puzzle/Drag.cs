using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Drag : MonoBehaviour
{
    private Vector3 offset, initialPos;
    private float zCoord;
    [SerializeField] private Vector3 gridSize;
    
    public bool inZ;
    public bool canMoveRight, canMoveLeft;
    public float speed;
    public int axisMul;


    private Vector3 startPos, currentPos;
    public float rayDist = 2;
    public float minDist = 0.8f;
    
    private void Start()
    {
        float time = Random.Range(1f, 1.5f);
    }

    private void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.right * rayDist, Color.green);
        Debug.DrawRay(transform.position, -transform.right * rayDist, Color.green);
        if (Physics.Raycast(transform.position, transform.right, out hit, rayDist))
        { 
            canMoveRight = false;
        }
        else
        {
            canMoveRight = true;
            axisMul = 1;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDist))
        {

            canMoveLeft = false;
        }
        else
        {
            canMoveLeft = true;
            axisMul = -1;
        }
    }

    

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        startPos = Input.mousePosition;
        initialPos = transform.position;
    }
    void OnMouseDrag()
    {
        /*Vector2 mousePoint = Input.mousePosition.normalized;
        transform.position += new Vector3(mousePoint.x, 0, mousePoint.y) * Time.deltaTime * speed;*/
        Vector3 deltaPos = (Input.mousePosition - startPos).normalized;

        var diffX = deltaPos.x;
        var diffY = deltaPos.y;
        
        
        if (canMoveRight)
        {
            if (!inZ && diffX > 0)
            {
                float rightX = GetMouseWorldPos().x > 0 ? GetMouseWorldPos().x : 0;
                transform.position = initialPos + new Vector3(diffX + gridSize.x, 0, 0);
                //SnapToGridX();
            }
            else if(inZ && diffY > 0)
            {
                transform.position = initialPos + new Vector3(0, 0, diffY + gridSize.z);
                //SnapToGridZ();
            }  
        }

        if (canMoveLeft)
        {
            if (!inZ && diffX < 0)
            {
                transform.position = initialPos + new Vector3(diffX - gridSize.x, 0, 0);
                //SnapToGridX();
            }
            else if(inZ && diffY < 0)
            {
                transform.position = initialPos + new Vector3(0, 0, diffY - gridSize.z);
                //SnapToGridZ();
            }
        }
    }
    private void OnMouseUp()
    {
        //transform.position = new Vector3(Mathf.Round(transform.position.x / gridSize.x) * gridSize.x, 0, Mathf.Round(transform.position.z / gridSize.z) * gridSize.z);
        AudioManager.instance.PlayClip(AudioManager.instance.slide);
        Vibration.Vibrate(17);
    }
    public void UpdateLocalPosition()
    {
        
    }
    void SnapToGridX()
    {
        Vector3 pos = new Vector3(Mathf.Round(transform.position.x / gridSize.x) * gridSize.x, 0, transform.position.z);
       
    }
    void SnapToGridZ()
    {
        Vector3 pos = new Vector3(transform.position.x, 0, Mathf.Round(transform.position.z / gridSize.z) * gridSize.z);
    }
}
