using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class PlayingGirlsHitReaction : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera manReactionCamera;
    public GameObject stringSpline;
    public GameObject chairOnHand;
    public GameObject chairOnFloor;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            other.GetComponent<Collider>().enabled = false;
            StartCoroutine(StopMobile(other.gameObject));
            GetComponent<SplineFollower>().enabled = false;
            other.GetComponent<PhoneMovement>().BreakScreen();
            Vibration.Vibrate(27);
            GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(4.5f));
        }
    }

    public IEnumerator StopMobile(GameObject mobile)
    {
        AudioManager.instance.bgAudioSource.enabled = false;
        //AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());

        manReactionCamera.Priority = 21;
        transform.DOLocalRotate(new Vector3(0, 180, 0), 0.55f);
        stringSpline.SetActive(false);
        animator.SetTrigger("run");
        transform.DOMove(chairOnFloor.transform.position, 0.5f).OnComplete(()=> {
            ThrowChairCommon.instance.chair = chairOnHand;
            animator.SetTrigger("throwIn");
            chairOnFloor.SetActive(false);
            //StartCoroutine(ThrowChair());
        });

        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;

        yield return new WaitForSeconds(3.5f);
        mobile.SetActive(false);

    }

    IEnumerator ThrowChair()
    {
        yield return new WaitForSeconds(0.5f);
        chairOnFloor.SetActive(false);
        chairOnHand.SetActive(true);
        chairOnHand.transform.parent = null;
        chairOnHand.transform.DORotate(new Vector3(Random.Range(0,180), Random.Range(0, 360), Random.Range(0, 180)), 0.7f);
        Transform camTransform = Camera.main.transform;
        Vector3 movePos = new Vector3(camTransform.position.x + 0.2f, camTransform.position.y - 0.2f, camTransform.position.z);
        chairOnHand.transform.DOMove(movePos, 0.5f).OnComplete(() =>
        {
            chairOnHand.SetActive(false);
            UIManager.instance.glassBreakPanel.SetActive(true);
            Vibration.Vibrate(27);
        });
    }
}
