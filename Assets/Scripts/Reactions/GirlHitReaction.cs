using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;
public class GirlHitReaction : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera manReactionCamera;
    public GameObject stringSpline;
    public GameObject potInHand;
    public GameObject potOnTable;
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
            AudioManager.instance.PlayClip(AudioManager.instance.girlHit);
            other.GetComponent<PhoneMovement>().BreakScreen();
        }
    }

    public IEnumerator StopMobile(GameObject mobile)
    {
        AudioManager.instance.bgAudioSource.enabled = false;
        //AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());

        manReactionCamera.Priority = 21;
        
        animator.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.55f);
        stringSpline.SetActive(false);
        animator.SetTrigger("throw");

        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;
        manReactionCamera.Priority = 12;

        yield return new WaitForSeconds(2f);
        mobile.SetActive(false);
        Vibration.Vibrate(27);
        GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(2.5f));

    }

    public void ThrowPot()
    {
        potOnTable.SetActive(false);
        potInHand.SetActive(true);
        potInHand.transform.parent = null;
        potInHand.transform.DORotate(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)), 0.3f);
        Transform camTransform = Camera.main.transform;
        Vector3 movePos = new Vector3(camTransform.position.x + 0.2f, camTransform.position.y - 0.2f, camTransform.position.z);
        potInHand.transform.DOMove(movePos, 0.5f).OnComplete(()=> {
            potInHand.SetActive(false);
            UIManager.instance.glassBreakPanel.SetActive(true);
            Vibration.Vibrate(27);
        });
    }
}
