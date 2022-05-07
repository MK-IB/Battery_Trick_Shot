using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using Dreamteck.Splines.Primitives;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MobileControllerRing : MonoBehaviour
{
    public GameObject lockScreen;
    public GameObject chargingScreen;

    public Image percentageWheel;
    public TextMeshProUGUI percentageText;
    public GameObject screenBreakable;
    public GameObject sparkEx;
    Rigidbody rb;
    private SplineFollower splineFollower;

    private void Start()
    {
        splineFollower = GetComponent<SplineFollower>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("halfWayActionTrigger") && !GameManager.instance.gameOver)
        {
            VirtualCameraManager.instance.StartCoroutine(VirtualCameraManager.instance.HalfWayCameraAction());
        }
        if (other.gameObject.CompareTag("charger"))
        {
            other.GetComponent<Collider>().enabled = false;
            //rb.isKinematic = true;
            splineFollower.follow = false;
            Vibration.Vibrate(27);
            
            GetComponent<Rotation>().enabled = false;
            transform.position = new Vector3(0.8f,5.77f, 54.4f);
            transform.eulerAngles = new Vector3(-166, 0, -90);
            
            StartCoroutine(ChangeToChargingScreen());
        }
        if (other.gameObject.CompareTag("obstacle"))
        {
            other.GetComponent<Collider>().enabled = false;
            GameManager.instance.gameOver = true;
            AudioManager.instance.bgAudioSource.enabled = false;
            Vibration.Vibrate(27);
            StartCoroutine(StopMovement());
            GameManager.instance.StartCoroutine(GameManager.instance.LevelFailed(1.5f));
        }
        if (other.gameObject.CompareTag("coin"))
        {
            other.GetComponent<CoinCollection>().CollectionEffect();
        }
    }
    IEnumerator StopMovement()
    {
        Time.timeScale = 1;
        splineFollower.follow = false;
        splineFollower.enabled = false;
        GetComponent<Collider>().isTrigger = false;
        VirtualCameraManager.instance.phoneFollower.Follow = null;
        AudioManager.instance.PlayClip(AudioManager.instance.obstacleHit);
        
        BreakScreen();

        //VirtualCameraManager.instance.phoneFollower.LookAt = transform;
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z - 0.5f), 0.3f);
        transform.DORotate(new Vector3(Random.Range(300, 360), Random.Range(300, 360), Random.Range(250, 360)), 0.3f).OnComplete(() =>
        {
            rb.isKinematic = false;
        });
        yield return null;
        //rb.isKinematic = false;
        yield return new WaitForSeconds(0.1f);
        //rb.isKinematic = true;
    }
    public void BreakScreen()
    {
        //VirtualCameraManager.instance.phoneFollower.GetComponent<ParticleSystem>().Play();
        AudioManager.instance.PlayClip(AudioManager.instance.glassBreak);
        screenBreakable.transform.parent = null;
        screenBreakable.SetActive(true);
        for(int i = 0; i < screenBreakable.transform.childCount; i++)
        {
            Rigidbody rb = screenBreakable.transform.GetChild(i).GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(Random.Range(100, 500), transform.position, 5);
            }
        }
    }
    IEnumerator ChangeToChargingScreen()
    {

        VirtualCameraManager.instance.cinemachineBrain.m_DefaultBlend.m_Time = 1;
        VirtualCameraManager.instance.phoneLastFocus.Priority = 20;
        yield return new WaitForSeconds(0.5f);
        sparkEx.SetActive(true);
        lockScreen.SetActive(false);
        AudioManager.instance.PlayClip(AudioManager.instance.chargingStarted);
        AudioManager.instance.bgAudioSource.enabled = false;
        chargingScreen.SetActive(true);

        float targetPerc = (float)System.Math.Round(Random.Range(0.5f, 1.0f), 2);
        float chargePerc = 0f;
        while (chargePerc < targetPerc)
        {
            percentageWheel.fillAmount = chargePerc;
            percentageText.SetText(Mathf.Round(chargePerc * 100).ToString() + "%");
            chargePerc += Time.deltaTime;
            yield return new WaitForSeconds(0.03f);
        }

        GameManager.instance.StartCoroutine(GameManager.instance.LevelComplete(1.5f));
    }
}
