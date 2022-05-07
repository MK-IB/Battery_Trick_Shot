using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ui : MonoBehaviour
{
    public static ui Instance;
    public GameObject helppanel;
    public AudioSource sounds;
    public AudioSource plugInSound;
    public static int  addNo;
    public TextMeshProUGUI coinsText;
    
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }

    }

    private void Start()
    {
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
    }

    public void nextlevel()
    {
        if (addNo == 1)
        {
            addNo = 0;
        }
        else
            addNo++;
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }
    public void looplevel()
    {
        PlayerPrefs.SetInt("level", 1);
        SceneManager.LoadScene(1);
    }
    public void restart()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void helppanelon()
    {
        helppanel.SetActive(true);
    }
    public void closehelppanel()
    {
        helppanel.SetActive(false);
    }
    public void PrivacyPolicy()
    {
        Application.OpenURL("https://seepeegames.blogspot.com/2020/01/privacy-policy-this-privacy-policy.html#more");
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && helppanel)
        {
            Debug.Log("on to off");
            helppanel.SetActive(false);
        }
    }
}
