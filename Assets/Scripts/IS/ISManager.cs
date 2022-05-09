using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ISManager : MonoBehaviour
{
    public static ISManager instance;

    public bool canShowAds, validateAdNetworks; // please set validation adnetwork it back to false
    public bool isRewardedVideoAvaliable, isInterstitialAdsAvaliable;

    public float timeGap;
    public string appKey, admobID;

    public TMP_Text timeTxt;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }

        Init();
    }

    void Start()
    {
        canShowAds = Connectivity();

        if (validateAdNetworks) 
            IronSource.Agent.validateIntegration();
        
        if(ConnectedToInternet())
            Invoke("LoadAds", 0.25f);
    }

    void Init()
    {
        //For Rewarded Video
        IronSource.Agent.init(appKey, IronSourceAdUnits.REWARDED_VIDEO);
        //For Interstitial
        IronSource.Agent.init(appKey, IronSourceAdUnits.INTERSTITIAL);
        //For Banners
        IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER);
    }

    void LoadAds()
    {
        LoadBannerAds();
        LoadInterstitialAds();
        ShowBannerAds();
    }

    void Update()
    {
        canShowAds = Connectivity();
        if (!canShowAds)
            return;
        if (!Application.isEditor)
        {
            isInterstitialAdsAvaliable = IronSource.Agent.isInterstitialReady();
            isRewardedVideoAvaliable = IronSource.Agent.isRewardedVideoAvailable();
        }
        else
        {
            isInterstitialAdsAvaliable = true;
            isRewardedVideoAvaliable = true;
        }
       
    }

    public bool ConnectedToInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;

        return true;
    }

    public bool TimeGapFinished()
    {
        timeGap -= Time.deltaTime;
        if (timeGap <= 0)
        {
            return true;
        }

        return false;
    }

    bool Connectivity()
    {
        return ConnectedToInternet() && TimeGapFinished();
    }

    public void ShowRewardedVideo()
    {
        if (!canShowAds || !isRewardedVideoAvaliable)
            return;
        HideBannerAds();
        PrintOut("CanShowAds @ ShowRewardedVideo");
        IronSource.Agent.showRewardedVideo();
    }


    public void LoadInterstitialAds()
    {
        if (!canShowAds)
            return;

        PrintOut("CanShowAds @ LoadInterstitialAds");

        IronSource.Agent.loadInterstitial();
    }

    private int val = 0;
    public void ShowInterstitialOnLC()
    {
         val++;
         if(val < 2)
             return;
        
         val = 0;
        ShowInterstitialAds();
        //Debug.Log("Ad called !");
    }
    public void ShowInterstitialAds()
    {
        if (!canShowAds && !isInterstitialAdsAvaliable)
            return;

        PrintOut("CanShowAds @ ShowInterstitialAds");

        HideBannerAds();
        IronSource.Agent.showInterstitial();
    }
    public void ShowInterstitialAds2()
    {
        if (!canShowAds && !isInterstitialAdsAvaliable)
            return;
        IronSource.Agent.showInterstitial();
        HideBannerAds();
        PrintOut("CanShow + InterstitialAds");
        
        if(!Application.isEditor)
            return;
        FindObjectOfType<ISInterstitial>().InterstitialAdClosedEvent();
    }


    public void LoadBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        PrintOut("CanShowAds @ LoadBannerAds");
        IronSource.Agent.loadBanner(IronSourceBannerSize.SMART, IronSourceBannerPosition.BOTTOM);
    }

    public void ShowBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        IronSource.Agent.displayBanner();
    }

    public void HideBannerAds()
    {
        if (!ConnectedToInternet())
            return;

        IronSource.Agent.hideBanner();
    }

    public void RewardCallBacks()
    {
        SkinUnlockManager.instance.DecideRewardCallback();
    }

    public void PrintOut(string txt)
    {
        print(txt);
    }


}
