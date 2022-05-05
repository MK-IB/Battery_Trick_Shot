using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SkinUnlockManager : MonoBehaviour
{
    public static SkinUnlockManager instance;
    
    private float _fillAmount;

    public Image currentSkin;
    public Image fillingImage;
    public GameObject getSkinWithAdButton;

    public TextMeshProUGUI percSkinLoadedText;
    public RectTransform multiplierIndicator;
    public TextMeshProUGUI coinMultipliedText;
    public TextMeshProUGUI coinsText;
    private int _skinUnlockedIndex;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        StartCoroutine(UnlockSkin());
    }

    private void Start()
    {
        
    }

    IEnumerator UnlockSkin()
    {
        coinsText.SetText(ShopDataHolder.instance.GetCoins().ToString());
        currentSkin.sprite = ShopDataHolder.instance.mobileSkins[ShopDataHolder.instance.GetSkin()];
        _fillAmount = PlayerPrefs.GetFloat("skinFillAmount", 0);
        _fillAmount += 0.25f;
        DOTween.To(() => fillingImage.fillAmount, x => fillingImage.fillAmount = x,  _fillAmount, 1f);
        percSkinLoadedText.SetText((_fillAmount * 100).ToString() + "% UNLOCKED");
        if (_fillAmount < 1)
        {
            getSkinWithAdButton.SetActive(false);
            percSkinLoadedText.gameObject.SetActive(true);
        }
        else
        {
            fillingImage.fillAmount = 0;
            _fillAmount = 0;
            PlayerPrefs.SetFloat("skinFillAmount", 0);

            yield return new WaitForSeconds(0.25f);
            getSkinWithAdButton.SetActive(true);
            percSkinLoadedText.gameObject.SetActive(false);
            _skinUnlockedIndex++;
            ShopDataHolder.instance.SetSkin(_skinUnlockedIndex);
            print("Added Skin added = " + _skinUnlockedIndex);
        }
    }
    
    public void GetUnlockedSkin()
    {
        ISManager.instance.ShowInterstitialAds();
        getSkinWithAdButton.SetActive(false);
        percSkinLoadedText.gameObject.SetActive(true);
        percSkinLoadedText.SetText("Congratulations ! Skin Added");
    }

    public void SaveSkinFillAmount()
    {
        PlayerPrefs.SetFloat("skinFillAmount", _fillAmount);
    }

    public void GetMultiplierCoins()
    {
        var value = Mathf.InverseLerp(-312, 312,  multiplierIndicator.anchoredPosition.x);

        if (value > 0.6f)
            value = 1f - value;
        
        if(value < 0.2f)
            UpdateCoinClaimValue(2);
        else if(value < 0.4f && value > 0.2f)
            UpdateCoinClaimValue(3);
        else 
            UpdateCoinClaimValue(5);
    }

    private int _claimedCoins;
    void UpdateCoinClaimValue(float val)
    {
        _claimedCoins = (int)(val * 100);
        coinMultipliedText.SetText(_claimedCoins.ToString());
    }

    public void PressMultipliedCoins()
    {
        multiplierIndicator.GetComponent<DOTweenAnimation>().tween.Pause();
        int totalCoins = _claimedCoins + ShopDataHolder.instance.GetCoins();
        coinsText.SetText(totalCoins.ToString());
        ShopDataHolder.instance.SetCoins(totalCoins);
    }
}
