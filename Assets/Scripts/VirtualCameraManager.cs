using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : MonoBehaviour
{
    public static VirtualCameraManager instance;
    public CinemachineBrain cinemachineBrain;

    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera initialLowBatteryFocus;
    public CinemachineVirtualCamera sadGirlFocus;
    public CinemachineVirtualCamera phoneFollower;
    public CinemachineVirtualCamera phoneLastFocus;
    public CinemachineVirtualCamera halfWayAction;

    [Header("Data")]
    public bool bedLevel;
    public float newBlendTime;
    public float slowMoFactor;
    public GameObject girlSad;

    [HideInInspector] public float gameStartTime = 0f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if(cinemachineBrain)
            cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        StartCoroutine(GameStartScene());
    }

    public IEnumerator GameStartScene()
    {
        //Shooter.instance.gameStarted = true;
        yield return new WaitForSeconds(0.3f); //5
        if(phoneFollower)
            phoneFollower.Priority = 11;


        yield return new WaitForSeconds(0.2f); //3
    }

    public IEnumerator HalfWayCameraAction()
    {
        cinemachineBrain.m_DefaultBlend.m_Time = newBlendTime;
        halfWayAction.Priority = 12;
        Time.timeScale = slowMoFactor;
        yield return new WaitForSeconds(0.3f);
        cinemachineBrain.m_DefaultBlend.m_Time = 1f;
        halfWayAction.Priority = 8;
        VirtualCameraManager.instance.phoneLastFocus.Priority = 20;
        Time.timeScale = 1f;
    }

    public void ResetCameras()
    {
        phoneFollower.Priority = 8;
        sadGirlFocus.Priority = 11;
        halfWayAction.Priority = 8;
        halfWayAction.Priority = 8;
    }
}
