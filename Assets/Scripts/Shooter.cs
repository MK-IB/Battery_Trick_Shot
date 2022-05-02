using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.EventSystems;

public class Shooter : MonoBehaviour
{
    public static Shooter instance;

    public Transform controllerNode;
    public int mobileMoveSpeed;
    public GameObject phone;
    public GameObject hand;
    public SplineMesh splineMesh;

    private Vector3 offset;
    private float zCoord;
    private SplineFollower _phoneSplineFollower;

    private float clipRangeVal;
    bool movePhone;

    public Material pathArrowMaterial;
    public Color transparentColor;

    [HideInInspector] public bool gameStarted = true;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _phoneSplineFollower = phone.GetComponent<SplineFollower>();
    }
    private void Update()
    {
        if (!GameManager.instance.gameOver && !movePhone)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                else
                {
                    zCoord = Camera.main.WorldToScreenPoint(controllerNode.position).z;
                    offset = controllerNode.position - GetMouseWorldPos();
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                else
                {
                    controllerNode.position = new Vector3(GetMouseWorldPos().x + offset.x, controllerNode.position.y ,controllerNode.position.z);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                
                    hand.transform.parent = null;
                    _phoneSplineFollower.follow = true;
                    _phoneSplineFollower.followSpeed = mobileMoveSpeed;
                    movePhone = true;
                    //pathArrowMaterial.color = transparentColor;
                    splineMesh.SetClipRange(0, 0);
                    AudioManager.instance.PlayClip(AudioManager.instance.slideWoosh1);
                    AudioManager.instance.PlayClip(AudioManager.instance.slideWoosh);
            }
        }

        if (movePhone)
        {
            clipRangeVal += Time.deltaTime / 4;
            _phoneSplineFollower.SetClipRange(clipRangeVal, 1);
            if (clipRangeVal >= 1)
            {
                movePhone = false;
                //CameraFollower.instance.LastRotation();
            }
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
