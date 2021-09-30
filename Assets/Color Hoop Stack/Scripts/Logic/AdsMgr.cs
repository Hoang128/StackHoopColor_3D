using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsMgr : Singleton<AdsMgr>
{
    public float waitTimeToLoadNewAds = 5f;

    private BannerView bannerView;
    public InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    public string adBannerUnitId = "ca-app-pub-3940256099942544/6300978111";
    public string adRewardUnitId = "ca-app-pub-3940256099942544/5224354917";
    public string adInterstitialUnitId = "ca-app-pub-3940256099942544/5224354917";


    // Test app id: ca-app-pub-3940256099942544-3347511713
    // Start is called before the first frame update
    private void Awake()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        StartCoroutine(LoadRewardedAdsAfter(2f));
    }
    public void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adBannerUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

    }

    public void RequestInterstitial()
    {
        // Initialize an InterstitialAd.
        this.interstitialAd = new InterstitialAd(adInterstitialUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");

        EventDispatcher.Instance.PostEvent(EventID.ON_LOADED_REWARDED_ADS);
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.ToString());

        StartCoroutine(LoadRewardedAdsAfter(waitTimeToLoadNewAds));
        EventDispatcher.Instance.PostEvent(EventID.ON_FAILED_LOAD_REWARDED_ADS);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        GameplayMgr.Instance.AddRingStack();
        StartCoroutine(LoadRewardedAdsAfter(1f));
    }

    public void UserChoseToWatchAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
        }
    }

    private IEnumerator LoadRewardedAdsAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        rewardedAd = new RewardedAd(adRewardUnitId);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }
}
