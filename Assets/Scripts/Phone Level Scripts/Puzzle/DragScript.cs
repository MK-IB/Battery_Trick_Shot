using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class DragScript : MonoBehaviour
{
    private Vector3 offset, initialPos;
    private float zCoord;
    [SerializeField] private Vector3 gridSize;
    
    public bool inZ;
    public bool canMoveRight, canMoveLeft;
    public float speed;
    public int axisMul;


    private Vector2 startPos, currentPos;
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
        currentPos = Input.mousePosition;
        Vector3 deltaPos = currentPos - startPos;

        currentPos = currentPos.normalized;
        var diffX = currentPos.x - startPos.x;
        var diffY = currentPos.y - startPos.y;
        
        
        if (canMoveRight)
        {
            if (!inZ && diffX > 0)
            {
                float rightX = GetMouseWorldPos().x > 0 ? GetMouseWorldPos().x : 0;
                transform.position = new Vector3(GetMouseWorldPos().x + offset.x * axisMul, 0, transform.position.z);
                SnapToGridX();
            }
            else if(inZ && diffY > 0)
            {
                transform.position = new Vector3(transform.position.x, 0, GetMouseWorldPos().z + offset.z * axisMul);
                SnapToGridZ();
            }    
        }

        if (canMoveLeft)
        {
            if (!inZ && diffX < 0)
            {
                transform.position = new Vector3(GetMouseWorldPos().x + offset.x * axisMul, 0, transform.position.z);
                SnapToGridX();
            }
            else if(inZ && diffY < 0)
            {
                transform.position = new Vector3(transform.position.x, 0, GetMouseWorldPos().z + offset.z * axisMul);
                SnapToGridZ();
            }
        }
    }
    private void OnMouseUp()
    {
        //transform.DOScale(new Vector3(1.3f, 1.3f, 1f), 0.15f);
    }

    void SnapToGridX()
    {
        Vector3 pos = new Vector3(Mathf.Round(transform.position.x / gridSize.x) * gridSize.x, 0, transform.position.z);
        transform.DOMove(pos, 0.3f);
    }
    void SnapToGridZ()
    {
        Vector3 pos = new Vector3(transform.position.x, 0, Mathf.Round(transform.position.z / gridSize.z) * gridSize.z);
        transform.DOMove(pos, 0.3f);
    }
}
