using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public TextMeshProUGUI coinsText;
    public Sprite lockedImg;
    public Sprite unlockedImg;
    public Sprite normalImage;
    public Button randomSkinUnlockButton;
   

    [Space(20)] public Image demoSkin;
    [Space(20)] public List<Image> skinHoldersList;
    public List<Image> skinsList;

    private List<Sprite> _mobileSkinsList;
    private int _skinCost = 1000;

    public int currentUnlockIndex = -1;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        _mobileSkinsList = ShopDataHolder.instance.mobileSkins;
       
        UpdateCoinsDisplay();
        UnlockBoughtSkins();
    }

    void UpdateCoinsDisplay()
    {
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
    }

    private void Start()
    {
       
      //Invoke(nameof(CheckRandomUnlockButtonState),0.25f);  
    }

    private int unlockedSkinNum;
    

    public void UnlockBoughtSkins()
    {
        demoSkin.sprite = _mobileSkinsList[ShopDataHolder.instance.GetDemoSkinIndex()];

        List<int> unlockedIndicesList = GetUnlockedIndicesList();
        //unlockedIndicesList.RemoveAt(0);
        for (int i = 0; i < unlockedIndicesList.Count; i++)
        {
            print(unlockedIndicesList[i]);
        }
        
        if(ShopDataHolder.instance.GetSkinLockState() == 1)
        {
            for (int i = 0; i < unlockedIndicesList.Count; i++)
            {
                if (unlockedIndicesList[i] < skinHoldersList.Count)
                {
                    skinHoldersList[unlockedIndicesList[i]].sprite = normalImage;
                    skinHoldersList[unlockedIndicesList[i]].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }

    //RETURNS THE INDICES OF SKINS UNLOCKED
    List<int> GetUnlockedIndicesList()
    {
        var unlockedIndicesNum = ShopDataHolder.instance.GetUnlockedSkin();
        char[] chars = unlockedIndicesNum.ToCharArray();
        int[] indicesArr = Array.ConvertAll(chars, c => (int)Char.GetNumericValue(c));
        return new List<int>(indicesArr);
    }

    public void ChangeSkinInDemo(int skinIndex)
    {
        List<int> allUnlockedIndicesList = new List<int>();
        var boughtIndices = GetUnlockedIndicesList();
        

        for (int i = 0; i < boughtIndices.Count; i++)
        {
            if (i == skinIndex)
            {
                demoSkin.sprite = _mobileSkinsList[skinIndex];
                ShopDataHolder.instance.SetDemoSkinIndex(skinIndex);
            }
        }
    }

    void CheckRandomUnlockButtonState()
    {
        // if (ShopDataHolder.instance.GetCoins() < _skinCost)
        //     randomSkinUnlockButton.interactable = false;
        // else randomSkinUnlockButton.interactable = true;
    }

    public void GetCoinsWithAd()
    {
        if (ISManager.instance)
            ISManager.instance.ShowInterstitialAds();
        ShopDataHolder.instance.SetCoins(ShopDataHolder.instance.GetCoins() + 800);
        UpdateCoinsDisplay();
        CheckRandomUnlockButtonState();
    }

    public void UnlockRandomSkin()
    {
        if (ShopDataHolder.instance.GetCoins() >= _skinCost)
        {
            int boughtIndex = 0;
            List<int> unlockedIndicesList = GetUnlockedIndicesList();
            
            for (int i = 0; i < skinsList.Count; i++)
            {
                if (!unlockedIndicesList.Contains(i))
                {
                    skinHoldersList[i].sprite = unlockedImg;
                    skinHoldersList[i].transform.GetChild(0).gameObject.SetActive(true);
                    boughtIndex = i;
                    break;
                }
            }

            //spend coins
            var totalCoins = ShopDataHolder.instance.GetCoins();
            var availableCoins = totalCoins - _skinCost;
            ShopDataHolder.instance.SetCoins(availableCoins);
            UpdateCoinsDisplay();

            //save the randomly unlocked skins
            SaveRandomlyUnlockedSkins(boughtIndex);
            CheckRandomUnlockButtonState();
        }
    }

    void SaveRandomlyUnlockedSkins(int boughtIndex)
    {
        var indicesInt = ShopDataHolder.instance.GetUnlockedSkin();
        ShopDataHolder.instance.SetUnlockedSkin(ShopDataHolder.instance.GetUnlockedSkin() + boughtIndex);
    }

    IEnumerator UnlockingAnimation()
    {
        yield break;
    }
}