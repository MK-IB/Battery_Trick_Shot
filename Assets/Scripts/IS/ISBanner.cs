using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISBanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IronSourceEvents.onBannerAdClickedEvent += onBannerAdClickedEvent;
        IronSourceEvents.onBannerAdLeftApplicationEvent += onBannerAdLeftApplicationEvent;
        IronSourceEvents.onBannerAdLoadedEvent += onBannerAdLoadedEvent;
        IronSourceEvents.onBannerAdLoadFailedEvent += onBannerAdLoadFailedEvent;
        IronSourceEvents.onBannerAdScreenDismissedEvent += onBannerAdScreenDismissedEvent;
        IronSourceEvents.onBannerAdScreenPresentedEvent += onBannerAdScreenPresentedEvent;
    }

    private void onBannerAdScreenPresentedEvent()
    {
        print("onBannerAdScreenPresentedEvent");
    }

    private void onBannerAdScreenDismissedEvent()
    {
        print("onBannerAdScreenDismissedEvent");
        ISManager.instance.LoadBannerAds();
    }

    private void onBannerAdLoadFailedEvent(IronSourceError obj)
    {
        print(obj.getDescription());
        ISManager.instance.LoadBannerAds();
    }

    private void onBannerAdLoadedEvent()
    {
        print("onBannerAdLoadedEvent");
    }

    private void onBannerAdLeftApplicationEvent()
    {
        print("onBannerAdLeftApplicationEvent");
    }

    private void onBannerAdClickedEvent()
    {
        print("onBannerAdClickedEvent");
    }
}
