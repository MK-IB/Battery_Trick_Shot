using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gameOver;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Vibration.Init();
        if(GA_FB.instance)
            GA_FB.instance.LevelStart(PlayerPrefs.GetInt("level", 1).ToString());
        //CommonUIEventsManager.instance.StartLevelStartEvent();
    }
    public bool skipLvl;
    public void SkipLevel()
    {
        if(ISManager.instance)
            ISManager.instance.ShowInterstitialAds();
        NextLevel();
        //ISManager.instance.RewardCallBacks();
        /*if (skipLvl || !ISManager.instance)
            return;

        skipLvl = true;
        ISManager.instance.ShowRewardedVideo();*/
    }
    public void NextLevel()
    {
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
        //ISManager.instance.ShowRewardedVideo();
       
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      
        if(ISManager.instance)
            ISManager.instance.ShowInterstitialAds();
        //VirtualCameraManager.instance.ResetCameras();
    }

    public IEnumerator LevelComplete(float waitTime)
    {
        gameOver = true;
        yield return new WaitForSeconds(waitTime);
        ISManager.instance.ShowInterstitialOnLC();
        
        //UIManager.instance.winPanel.SetActive(true);
        CommonUIEventsManager.instance.StartLevelCompleteEvent();
        AudioManager.instance.PlayClip(AudioManager.instance.win);
        WinAndFailUIManager.instance.SetWinUI();
        
        if(GA_FB.instance)
            GA_FB.instance.LevelComplete(PlayerPrefs.GetInt("level", 1).ToString());
            
    }

    public IEnumerator LevelFailed(float waitTime)
    {
        gameOver = true;
        yield return new WaitForSeconds(waitTime);
        AudioManager.instance.PlayClip(AudioManager.instance.fail);
        UIManager.instance.losePanel.SetActive(true);
        //FailCounterDontDestroy.instance.CountFail();
        
        if(GA_FB.instance)
            GA_FB.instance.LevelFail(PlayerPrefs.GetInt("level", 1).ToString());
        /*if(ISManager.instance)
            ISManager.instance.ShowInterstitialAds();*/
    }
}
