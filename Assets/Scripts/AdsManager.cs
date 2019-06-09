using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;


//namespace AdsManager
//{

public class AdsManager : MonoBehaviour
{

    public static AdsManager instance;

    [Header("Android AD Id's")]
    public string bundleIdentifierAndroid;
    public string AdmobAppIDAndriod, AdmobBannerIDAndroid, AdmobInterstitialIDAndroid, AdmobRewardVideoIDAndroid, unityIDAndroid, moreGamesPlay;
    [Header("IOS AD Id's")]
    public string AdmobAppIDIOS; 
    public string AdmobBannerIDIOS, AdmobInterstitialIDIOS, AdmobRewardVideoIDIOS, unityIDIOS, moreGamesIOS, rateusIOS;
    


    [Header("Use only while Testing")]
    public bool TestMode = false;

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;


    RewardedVideoAdScene fb_reward_video;
    // for FB_NativeBannerAdScene
    NativeBannerAdScene fb_nativeBanner;

    // for FB_NativeAdScene
    NativeAdScene fb_nativeAds;
    // for FB_InterstitialAdScene
    InterstitialAdScene fb_interstitialAdScene;

    //Unity rewarded video replacement ID
    [Header("Unity Rewarded video ID")]
    string placementIdRewarded = "rewardedVideo";
    string placementIdUnity = "video";
    string unityInterstitialAdID = "InterstitialAd";
    int intCount = 1; // Interstitial Count

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }




        //fb_nativeBanner = GameObject.FindObjectOfType<NativeBannerAdScene>();
        //fb_nativeAds = GameObject.FindObjectOfType<NativeAdScene>();
        fb_interstitialAdScene = GameObject.FindObjectOfType<InterstitialAdScene>();
        fb_reward_video = GameObject.FindObjectOfType<RewardedVideoAdScene>();
    }

    public void Start()
    {
#if UNITY_ANDROID
        string appId = AdmobAppIDAndriod;
#elif UNITY_IPHONE
		string appId = AdmobAppIDIOS;
#else
		string appId = "unexpected_platform";
#endif

        if (TestMode)
        {
            // Android Ids
#if UNITY_ANDROID
            appId = "ca-app-pub-3940256099942544~3347511713";
            AdmobBannerIDAndroid = "ca-app-pub-3940256099942544/6300978111";
            AdmobInterstitialIDAndroid = "ca-app-pub-3940256099942544/1033173712";
            AdmobRewardVideoIDAndroid = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            AdmobBannerIDIOS = "ca-app-pub-3940256099942544/2934735716";
            AdmobInterstitialIDIOS = "ca-app-pub-3940256099942544/4411468910";
            AdmobRewardVideoIDIOS = "ca-app-pub-3940256099942544/1712485313";
#endif

            // IOS IDs
        }
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        InitializeUnityAds();

        MobileAds.SetiOSAppPauseOnBackground(true);
        // Requesting Ads from Admob. When Using Demo Scene Comment all
        //AdmobRequestBanner(); // Show banner request
        //RequestInterstitial();
        //AdmobRequestRewardBasedVideo();
        //Load_Facebook_Interstitial(); // Facebook Int load request
        //Load_Facebook_Rewarded_Video(); // Load Facbook Rewarded Video Req
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    //Unity Ads
    void InitializeUnityAds()
    {
        //bool unityTestAds = false;
        if (Debug.isDebugBuild)
        {
            TestMode = true;
        }
        else
        {
            TestMode = false;
        }

#if UNITY_IOS
		if(Advertisement.isSupported)
			Advertisement.Initialize (unityIDIOS, TestMode);
#elif UNITY_ANDROID
        if (Advertisement.isSupported)
            Advertisement.Initialize(unityIDAndroid, TestMode);
#endif
    }

    public void ShowUnityInterstitial()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResultInterstitial;

        if (Advertisement.IsReady(unityInterstitialAdID))
        {
            Advertisement.Show(unityInterstitialAdID, options);
        }
        else
        {
            Debug.Log("Unity Interstitial is not ready");
        }

    }

    void HandleShowResultInterstitial(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Unity Interstitial");

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Unity Interstitial Skipp");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Unity Interstitial Fail");
        }
    }

    public void ShowUnityVideoAd()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResultVideo;

        if (Advertisement.IsReady(placementIdUnity))
        {
            Advertisement.Show(placementIdUnity, options);
        }
        else
        {
            Debug.Log("Unity video Ad is not ready");
        }

    }

    void HandleShowResultVideo(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Unity Ad");

        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Unity Ad Skipp");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Unity Ad Fail");
        }
    }

    public void ShowUnityRewardedAd()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResultRewarded;

        if (Advertisement.IsReady(placementIdRewarded))
        {
            Advertisement.Show(placementIdRewarded, options);
        }

    }

    void HandleShowResultRewarded(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            DoubleMyCash();


        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }
    //	//END REGIION UNITY ADS



    //FACEBOOK ADS Func Calling
    public void Show_Facebook_Banner()
    {
        fb_nativeBanner.LoadAd();
    }
    public void Hide_Facebook_Banner()
    {
        fb_nativeBanner.OnDestroy_FB_Banner();
    }
    public void Load_Facebook_Rewarded_Video()
    {

        fb_reward_video.LoadRewardedVideo();
    }
    public void Show_Facebook_Rewarded_Video()
    {

        fb_reward_video.ShowRewardedVideo();
    }
    public void Show_Facebook_NativeAds()
    {
        fb_nativeAds.LoadAd();
    }
    public void Load_Facebook_Interstitial()
    {
        fb_interstitialAdScene.LoadInterstitial();
    }
    public void Show_Facebook_Interstitial()
    {
        fb_interstitialAdScene.ShowInterstitial();
    }


    // Unity Func



    //BANNER ADS

    public void AdmobRequestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AdmobBannerIDAndroid;
#elif UNITY_IPHONE
		string adUnitId = AdmobBannerIDIOS;
#else
		string adUnitId = "unexpected_platform";
#endif

		if (this.bannerView != null)
        {
            this.bannerView.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        bannerView.OnAdOpening += HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        bannerView.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        // Create an empty ad request.
        this.bannerView.LoadAd(this.CreateAdRequest());
    }

    public void AdmobBannerShow()
    {
        this.bannerView.Show();
    }

    public void DestroyBanner() // Destroying at game play
    {
        this.bannerView.Destroy();
        
    }

    public void HideBanner()
    {
        this.bannerView.Hide();
    }

    // Banner events
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Admob Banner HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Admob Banner HandleFailedToReceiveAd event received with message: "
        + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("Admob Banner HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Banner HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Banner HandleAdLeavingApplication event received");
    }



    //INTERSTITIAL ADS
    public void AdmobRequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = AdmobInterstitialIDAndroid;
#elif UNITY_IPHONE
		string adUnitId = AdmobInterstitialIDIOS;
#else
		string adUnitId = "unexpected_platform";
#endif

		if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        interstitial.OnAdLoaded += HandleOnAdLoadedInterstitial;
        // Called when an ad request failed to load.
        interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadInterstitial;
        // Called when an ad is shown.
        interstitial.OnAdOpening += HandleOnAdOpenedInterstitial;
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosedInterstitial;
        // Called when the ad click caused the user to leave the application.
        interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplicationInterstitial;

        // Load.
        this.interstitial.LoadAd(this.CreateAdRequest());
    }
    
    public void AdmobShowInterstitial()
    {
                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
                }
                else
                {
                    Debug.Log("Interstitial is not loaded");

                }

    }

    public void HandleOnAdLoadedInterstitial(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoadInterstitial(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
        + args.Message);
    }

    public void HandleOnAdOpenedInterstitial(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosedInterstitial(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
        AdmobRequestInterstitial();
        //RequestBanner ();
    }

    public void HandleOnAdLeavingApplicationInterstitial(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeavingApplication event received");
    }
	

    //REWARD BASED VIDEO AD
    public void AdmobRequestRewardBasedVideo()
    {

        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;



#if UNITY_ANDROID
        string adUnitId = AdmobRewardVideoIDAndroid;
#elif UNITY_IPHONE
            string adUnitId = AdmobRewardVideoIDIOS;
#else
            string adUnitId = "unexpected_platform";
#endif
		//Load Ad
        this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
    }

    public void AdmobShowRewardVideoAd()
    {
        if (rewardBasedVideo.IsLoaded())
        {
            this.rewardBasedVideo.Show();
            Debug.Log("Admob rewarded ad shown");

        }
        else
        {
            Debug.Log("Admob ad is not loaded. Requested again");

        }

    }
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log(
                "HandleRewardBasedVideoFailedToLoad event received with message: "
                                 + args.Message);


    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoClosed event received");
        AdmobRequestRewardBasedVideo();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        DoubleMyCash();
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log(
                "HandleRewardBasedVideoRewarded event received for "
                            + amount.ToString() + " " + type);
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
    public void Load_NextScene()
    {

        //Application.LoadLevel("AdViewScene");
        //		SceneMenagment.LoadScene("AdViewScene");

    }

    public void DoubleMyCash()
    {
            int reward = (PlayerPrefs.GetInt("Currency") + 1000);
            Debug.Log("Video Reward function Check");
            PlayerPrefs.SetInt("Currency", reward);
        
    }


    private AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
            //.AddTestDevice(AdRequest.TestDeviceSimulator)
            //.AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
            //.AddKeyword("game")
            //.SetGender(Gender.Male)
            //.SetBirthday(new DateTime(1985, 1, 1))
            //.TagForChildDirectedTreatment(false)
            //.AddExtra("color_bg", "9B30FF")
            //.Build();
    }



    public void OnMoreGames()
    {
#if UNITY_ANDROID
        Application.OpenURL(moreGamesPlay);
#elif UNITY_IPHONE
                Application.OpenURL(moreGamesIOS);

#else
                string adUnitId = "unexpected_platform";
#endif
    }

    public void OnRateUs()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" +bundleIdentifierAndroid);

#elif UNITY_IPHONE
        Application.OpenURL(rateusIOS);
#endif
    }


}
