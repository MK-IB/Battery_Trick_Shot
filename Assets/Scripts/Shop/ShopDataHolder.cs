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
    }

    void DeactivateSkinUnlockCanvas()
    {
        _skinUnlockCanvas.SetActive(false);
    }
    public void SetUnlockedSkin(string indices)
    {
        PlayerPrefs.SetString("skinIndices", indices);
    }
    
    public string GetUnlockedSkin()
    {
        return PlayerPrefs.GetString("skinIndices", "0");
    }

    public void SetBoughtSkins(int indices)
    {
        PlayerPrefs.SetInt("boughtSkins", indices);
    }
    public int GetBoughtSkins()
    {
        return PlayerPrefs.GetInt("boughtSkins", 9);
    }

    public void SetSkinLockState(int i)
    {
        PlayerPrefs.SetInt("skinLockState", i);
    }
    public int GetSkinLockState()
    {
        return PlayerPrefs.GetInt("skinLockState", 0);
    }
    public void SetCoins(int coin)
    {
        PlayerPrefs.SetInt("coins", coin);
    }

    public int GetCoins()
    {
        return PlayerPrefs.GetInt("coins", 0);
    }

    public void SetDemoSkinIndex(int index)
    {
        PlayerPrefs.SetInt("demoSkinIndex", index);
    }
    public int GetDemoSkinIndex()
    {
        return PlayerPrefs.GetInt("demoSkinIndex", 0);
    }
}
