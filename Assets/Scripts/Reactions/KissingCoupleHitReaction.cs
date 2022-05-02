using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class KissingCoupleHitReaction : MonoBehaviour
{
    Animator animator;
    Transform rootParent;
    public CinemachineVirtualCamera manReactionCamera;
    public GameObject stringSpline;
    public Animator partnerAnimator;
    public GameObject angryEmoji;
    public GameObject partnerAngryEmoji;
    public GameObject heartParticles;

    private void Start()
    {
        rootParent = transform.root;
        animator = rootParent.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(StopMobile(other.gameObject));
            heartParticles.SetActive(false);
            other.GetComponent<PhoneMovement>().BreakScreen();
            Vibration.Vibrate(27);
            if (transform.name.Contains("Boy"))
            {
                AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());
            }
            else {
                AudioManager.instance.PlayClip(AudioManager.instance.girlHit);
            }
        }
    }
    public IEnumerator StopMobile(GameObject mobile)
    {
        AudioManager.instance.bgAudioSource.enabled = false;

        angryEmoji.SetActive(true);
        partnerAngryEmoji.SetActive(true);
        rootParent.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.55f);
        stringSpline.SetActive(false);
        animator.SetTrigger("argue2");
        partnerAnimator.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.55f);
        partnerAnimator.SetTrigger("argue");

        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;
        manReactionCamera.Priority = 12;

        yield return new WaitForSeconds(3.5f);
        mobile.SetActive(false);
        GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(2.5f));

    }
}
