using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class CoupleHitTrigger : MonoBehaviour
{
    public Animator girlAnimator;
    public Animator boyAnimator;
    public CinemachineVirtualCamera coupleReactionCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mobile"))
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(StopMobile(other.gameObject));
            VirtualCameraManager.instance.cinemachineBrain.m_DefaultBlend.m_Time = 2.5f;
            AudioManager.instance.bgAudioSource.enabled = false;
            AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());

            coupleReactionCamera.Priority = 21;
            girlAnimator.transform.DOLocalMove(new Vector3(4.16f, 4.04f, -9.36f), 0.2f);
            girlAnimator.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f);
            girlAnimator.SetTrigger("disbelief");

            boyAnimator.transform.DOLocalMove(new Vector3(0.69f, 4.56f, -13.27f), 0.2f);
            boyAnimator.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.4f);
            boyAnimator.SetTrigger("angry");
            Vibration.Vibrate(27);
        }
    }

    IEnumerator StopMobile(GameObject mobile)
    {
        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;
        GameManager.instance.gameOver = true;

        GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(3.5f));
        yield return null;
    }
}
