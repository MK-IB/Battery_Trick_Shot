using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class CommonLevelManager : MonoBehaviour
{
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
        StartCoroutine(DisableSkinUnlockCanvas());
    }

    IEnumerator DisableSkinUnlockCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject skinUnlockObj = GetComponent<ShopDataHolder>()._skinUnlockCanvas.gameObject;
        skinUnlockObj.SetActive(false);
        skinUnlockObj.GetComponent<SkinUnlockManager>().SaveSkinFillAmount();
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        ISManager.instance.ShowInterstitialAds();
    }

    public void PausePlayGame(int i)
    {
        Time.timeScale = i;
    }
}