using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShooterWithDrift : MonoBehaviour
{
    public static ShooterWithDrift instance;
    
    public Transform controllerNode;
    public Transform firstPointControllerNode;
    public int mobileMoveSpeed;
    public GameObject phone;
    public GameObject hand;
    
    SplineComputer splineComputer;
    SplineMesh splineMesh;

    private Vector3 offset;
    private float zCoord;
    public SplineFollower _phoneSplineFollower;

    private float clipRangeVal;
    bool movePhone;

    public Material pathArrowMaterial;
    public Color transparentColor;

    [HideInInspector] public bool gameStarted;
    public List<Transform> chargerPoints;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        splineComputer = _phoneSplineFollower.spline;
        splineMesh = splineComputer.GetComponent<SplineMesh>();
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
                    controllerNode.position = new Vector3(GetMouseWorldPos().x + offset.x, controllerNode.position.y, controllerNode.position.z);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                
                    hand.transform.parent = null;
                    _phoneSplineFollower.follow = true;
                    _phoneSplineFollower.followSpeed = mobileMoveSpeed;
                    _phoneSplineFollower.motion.applyRotationY = false;
                    movePhone = true;
                    phone.GetComponent<Rotation>().enabled = true;
                    phone.GetComponent<PreShootRotation>().enabled = false;
                    //pathArrowMaterial.color = transparentColor;
                    splineMesh.SetClipRange(0, 0);
                    //_phoneSplineFollower.motion.rotationOffset.y = false;
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

    private int chargerPointCounter;
    public void SetNewPhone(GameObject newPhone)
    {
        movePhone = false;
        clipRangeVal = 0;
        hand.SetActive(false);
        hand = newPhone.transform.GetChild(1).gameObject;
        newPhone.GetComponent<SplineFollower>().spline = splineComputer;
        _phoneSplineFollower = newPhone.GetComponent<SplineFollower>();
        _phoneSplineFollower.SetClipRange(0,1);
        splineMesh.SetClipRange(0, 1);
        firstPointControllerNode.position = chargerPoints[chargerPointCounter].position;

        /*Vector3 dirNorm = (firstPointControllerNode.position - phone.transform.position).normalized;
        controllerNode.position = dirNorm * 2;*/
        chargerPointCounter++;
    }
}
