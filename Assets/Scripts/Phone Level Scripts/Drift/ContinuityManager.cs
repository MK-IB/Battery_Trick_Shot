using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ContinuityManager : MonoBehaviour
{
    public static ContinuityManager instance;

    public List<GameObject> continuitySetList;
    private int continuitySetCounter;
    
    [SerializeField] int maxShoot;
    [Space(30)]
    public int shootNumCounter = 1;

    [HideInInspector] public PhoneMovementContinuity currentPhoneScript;
    [HideInInspector] public Vector3 phonePosition;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxShoot = continuitySetList.Count + 1;
    }

    public void CheckLevelComplete()
    {
        if (shootNumCounter == maxShoot)
        {
            StartCoroutine(currentPhoneScript.ChangeToChargingScreen());
            StartCoroutine(currentPhoneScript.LevelCompleted());
        }
        else
        {
            shootNumCounter++;
            GameObject newSet = continuitySetList[continuitySetCounter];
            newSet.SetActive(true);
            VirtualCameraManager.instance.phoneFollower.m_Follow =
                newSet.transform.GetChild(0);
            ShooterWithContinuity.instance.SetNewContinuity(newSet.transform);
            continuitySetCounter++;
        }
    }
}
