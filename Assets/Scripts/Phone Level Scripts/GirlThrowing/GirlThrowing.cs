using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.EventSystems;

public class GirlThrowing : MonoBehaviour
{
   public GameObject phoneOnHand;
   public GameObject phoneOnTable;
   public CinemachineVirtualCamera followerVCam;

   private bool isThrown;
   private Animator _animator;

   private void Start()
   {
      _animator = GetComponent<Animator>();
   }

   private void Update()
   {
      if (Input.GetMouseButtonDown(0) && !isThrown && !EventSystem.current.IsPointerOverGameObject())
      {
         if (TapScale.instance.isCorrectTime)
         {
            ThrowPhone();
         }else
         {
            phoneOnHand.SetActive(false);
            phoneOnTable.SetActive(true);
            StartCoroutine(phoneOnTable.GetComponent<MobileControllerGirlThrow>().StopMovement());
            _animator.SetTrigger("fail");
            StartCoroutine(StopTapScaleAnimation());
         }
         isThrown = true;
      }
   }

   public void ThrowPhone()
   {
      phoneOnHand.SetActive(false);
      phoneOnTable.SetActive(true);
      followerVCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 5, -6);
      followerVCam.transform.eulerAngles = new Vector3(23.5f, 0, 0);
      phoneOnTable.GetComponent<SplineFollower>().enabled = true;
      _animator.speed = 1;
      StartCoroutine(StopTapScaleAnimation());
   }

   IEnumerator StopTapScaleAnimation()
   {
      Transform tapScale = TapScale.instance.transform;
      tapScale.GetComponent<Animator>().enabled = false;
      
      yield return new WaitForSeconds(0.7f);
      tapScale.parent.gameObject.SetActive(false);
   }

}
