using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class CoupleHitTrigger2 : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera manReactionCamera;
    public bool isMan = true;
    public RemoveAnimRig removeAnimRig;
    public Transform partner;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(StopMobile(other.gameObject));
            other.GetComponent<PhoneMovement>().BreakScreen();
            AudioManager.instance.bgAudioSource.enabled = false;
            if (isMan)
                AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());
            else
                AudioManager.instance.PlayClip(AudioManager.instance.girlHit);

            manReactionCamera.Priority = 21;
            animator.transform.DORotate(new Vector3(0, 180, 0), 0.55f);
            partner.position = new Vector3(partner.position.x, 0, partner.position.z);
            animator.SetTrigger("angry");
            Vibration.Vibrate(27);
            partner.GetComponent<Animator>().SetTrigger("disbelief");
        }
    }

    IEnumerator StopMobile(GameObject mobile)
    {
        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;
        manReactionCamera.Priority = 12;

        if (removeAnimRig)
            removeAnimRig.Scold(transform);

        yield return new WaitForSeconds(2.5f);
        mobile.SetActive(false);
        GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(2.5f));

    }
}
