using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopDataHolder : MonoBehaviour
{
    public static ShopDataHolder instance;

    public List<Sprite> mobileSkins;
    public int totalCoins;

    [HideInInspector] public GameObject _skinUnlockCanvas;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        _skinUnlockCanvas = transform.GetChild(0).gameObject;
        CommonUIEventsManager.instance.LevelStartEvent += DeactivateSkinUnlockCanvas;
        CommonUIEventsManager.instance.LevelCompleteEvent += ActivateSkinUnlockCanvas;
    }

    void ActivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(true);
        _skinUnlockCanvas.GetComponent<SkinUnlockManager>().multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Restart();
    }

    void DeactivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(false);
    }
    public void SetSkin(int index)
    {
        PlayerPrefs.SetInt("skinIndex", index);
    }
    
    public int GetSkin()
    {
        return PlayerPrefs.GetInt("skinIndex", 0);
    }

    public void SetCoins(int coin)
    {
        int currentTotalCoins = coin + PlayerPrefs.GetInt("coins", 0);
        PlayerPrefs.SetInt("coins", currentTotalCoins);
    }

    public int GetCoins()
    {
        return PlayerPrefs.GetInt("coins", 0);
    }
}
