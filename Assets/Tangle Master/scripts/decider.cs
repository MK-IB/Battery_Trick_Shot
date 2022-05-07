using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class decider : MonoBehaviour
{
    public selecter selecter;
    public Animator deviceanimation;
    private bool lefted;
    public int intouch;
    private float timer;
    public float starttime;
    public GameObject winpanel;
    private GameObject papereffect;
    [HideInInspector] public bool levelComplete;

    private void Awake()
    {
        //CommonUIEventsManager.instance.StartLevelStartEvent();
    }

    void Start()
    {
        papereffect = GameObject.Find("papereffect");
        papereffect.SetActive(false);
        starttime = 0.75f;
        lefted = false;
        intouch = 0;
        timer = starttime;
        winpanel.SetActive(false);
        selecter = GameObject.Find("Main Camera").GetComponent<selecter>();
        GA_FB.instance.LevelStart(PlayerPrefs.GetInt("level", 1).ToString());
    }
    void Update()
    {
        if (intouch == 0 && lefted)
        {
            //  Debug.Log("ntg happens");
        }
        else if (intouch == 0 && lefted != true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !levelComplete)
            {
                StartCoroutine(LevelCompleteEvents());
                levelComplete = true;

            }
        }
        else
        {
            timer = starttime;
        }
    }

    IEnumerator LevelCompleteEvents()
    {
        CameraZoomIn();
        EventsManager.instance.StartTangleSolvedEvent();
        
        yield return new WaitForSeconds(2f);
        //winpanel.SetActive(true);
        CommonUIEventsManager.instance.StartLevelCompleteEvent();
        PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(selecter);
        papereffect.SetActive(true);
        //deviceanimation.enabled = true;
        GA_FB.instance.LevelComplete(PlayerPrefs.GetInt("level", 1).ToString());
        ISManager.instance.ShowInterstitialAds();
    }

    void CameraZoomIn()
    {
        GameObject[] mobiles = GameObject.FindGameObjectsWithTag("mobile");
        float totX = 0;
        
        for (int i = 0; i < mobiles.Length; i++)
        {
            totX += mobiles[i].transform.position.x;
        }
        Transform cam = Camera.main.transform;
        Vector3 camNewPos = new Vector3(totX / 2, cam.position.y, cam.position.z + 3);
        cam.DOMove(camNewPos, 0.3f).SetEase(Ease.Linear);
    }
    public void stillincontact()
    {
        intouch++;
    }
    public void onincontact()
    {
        intouch--;
    }
    public void lefteed()
    {
        lefted = true;
    }
    public void dropeed()
    {
        lefted = false;
    }

}
