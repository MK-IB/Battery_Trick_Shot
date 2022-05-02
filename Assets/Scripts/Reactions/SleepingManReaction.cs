using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Dreamteck.Splines;

public class SleepingManReaction : MonoBehaviour
{
    Animator animator;
    public CinemachineVirtualCamera manReactionCamera;
    AnimatorClipInfo[] animatorInfo;
    Transform rootParent;

    public Transform boy,girl;
    public Transform runPos;
    public GameObject sleepingBuzzEffect;
    public GameObject heartParticles;
    public GameObject faceAngryEmoji;

    public RemoveAnimRig removeAnimRig;

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
            other.GetComponent<PhoneMovement>().BreakScreen();
            AudioManager.instance.bgAudioSource.enabled = false;
            AudioManager.instance.StartCoroutine(AudioManager.instance.PlayMaleHitSound());

            manReactionCamera.Priority = 21;
            animator.SetTrigger("sleepToStand");
            animator.transform.DORotate(new Vector3(0, 150, 0), 0.55f);
            sleepingBuzzEffect.SetActive(false);
            faceAngryEmoji.SetActive(true);
            Vibration.Vibrate(27);
            /*animatorInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (animatorInfo[0].clip.name == "angry") ;*/
        }
    }

    IEnumerator StopMobile(GameObject mobile)
    {
        mobile.GetComponent<SplineFollower>().enabled = false;
        mobile.GetComponent<Rigidbody>().isKinematic = false;
        VirtualCameraManager.instance.phoneFollower.m_Follow = null;
        VirtualCameraManager.instance.cinemachineBrain.m_DefaultBlend.m_Time = 1;
        manReactionCamera.Priority = 12;

        yield return new WaitForSeconds(1.2f);
        rootParent.position = new Vector3(rootParent.position.x, 0.52f, rootParent.position.z);
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(CoupleRun());
        yield return new WaitForSeconds(3f);
        mobile.SetActive(false);
        GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(2f));

    }

    IEnumerator CoupleRun()
    {
        if(removeAnimRig)
        {
            removeAnimRig.RemoveRigAnd();
        }
        heartParticles.SetActive(false);
        boy.GetComponent<Animator>().SetTrigger("run");
        boy.transform.position = new Vector3(boy.transform.position.x, 0, boy.transform.position.z);
        boy.DORotate(new Vector3(0, -90, 0), 0.55f);
        boy.DOMove(runPos.position, 6f);

        girl.transform.position = new Vector3(girl.transform.position.x, 0, girl.transform.position.z);
        girl.GetComponent<Animator>().SetTrigger("run");
        girl.DOMove(runPos.position, 10f);
        yield return null;
    }
}
