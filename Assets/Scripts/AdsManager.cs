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
    public string AdmobAppIDAndriod, AdmobSmartBannerIDAndroid, AdmobBigBannerIDAndroid, AdmobInterstitialIDAndroid, AdmobRewardVideoIDAndroid, unityIDAndroid, moreGamesPlay;
    [Header("IOS AD Id's")]
    public string AdmobAppIDIOS; 
    public string AdmobSmartBannerIDIOS, AdmobBigBannerIDIOS, AdmobInterstitialIDIOS, AdmobRewardVideoIDIOS, unityIDIOS, moreGamesIOS, rateusIOS;
    private string AdmobRewardedAdUnitId;  // Admob Rewarded Ad unit ID


    [Header("Use only while Testing")]
    public bool TestMode = false;

    private BannerView bannerViewSmart;
    private BannerView bannerViewBig;
    private InterstitialAd interstitial;
    //private RewardBasedVideoAd rewardBasedVideo;
    private RewardedAd admobRewardAd; //New API
    


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
    string unityInterstitialAdID = "UnityInterstitial";
    int intCount = 1; // Interstitial Count


    // Life Reward vs Double Cash vs Earn 50 Coins
    int rewardType = 0;  // Type 1 is Life, type 2 is Double Cash, type 3 is 50 Coins
    // Delagates for Reward Event
    public delegate void rewardLife();
    public static event rewardLife RewardLife;
    public delegate void rewardDoubleCash();
    public static event rewardDoubleCash RewardDoubleCash;
    public delegate void reward50Coin();
    public static event reward50Coin Reward50Coin;

    bool showError;
    private GUIStyle currentStyle = null;

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



        FireBaseVersionCheck();
        //fb_nativeBanner = GameObject.FindObjectOfType<NativeBannerAdScene>();
        //fb_nativeAds = GameObject.FindObjectOfType<NativeAdScene>();
        fb_interstitialAdScene = GameObject.FindObjectOfType<InterstitialAdScene>();
        fb_reward_video = GameObject.FindObjectOfType<RewardedVideoAdScene>();
    }

    public void Start()
    {
//#if UNITY_ANDROID
//        string appId = AdmobAppIDAndriod;
//#elif UNITY_IPHONE
//		string appId = AdmobAppIDIOS;
//#else
//		string appId = "unexpected_platform";
//#endif

        if (TestMode)
        {
            // Android Ids
#if UNITY_ANDROID
            //appId = "ca-app-pub-3940256099942544~3347511713";
            AdmobSmartBannerIDAndroid = "ca-app-pub-3940256099942544/6300978111";
            AdmobBigBannerIDAndroid = "ca-app-pub-3940256099942544/6300978111";
            AdmobInterstitialIDAndroid = "ca-app-pub-3940256099942544/1033173712";
            AdmobRewardVideoIDAndroid = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            AdmobSmartBannerIDIOS = "ca-app-pub-3940256099942544/2934735716";
            AdmobBigBannerIDIOS = "ca-app-pub-3940256099942544/2934735716";
            AdmobInterstitialIDIOS = "ca-app-pub-3940256099942544/4411468910";
            AdmobRewardVideoIDIOS = "ca-app-pub-3940256099942544/1712485313";
#endif
        }


        // Initialize the Google Mobile Ads SDK.
        //MobileAds.Initialize(appId);
        MobileAds.Initialize(initStatus => { });
        InitializeUnityAds();

        MobileAds.SetiOSAppPauseOnBackground(true);

#if UNITY_ANDROID // Setting Rewarded Ad Unit
        AdmobRewardedAdUnitId = AdmobRewardVideoIDAndroid;
#elif UNITY_IPHONE
         AdmobRewardedAdUnitId = AdmobRewardVideoIDIOS;
#else
            string adUnitId = "unexpected_platform";
#endif


        // Requesting Ads from Admob. When Using Demo Scene Comment all
        //AdmobRequestBanner(); // Show banner request
        //RequestInterstitial();
        //AdmobRequestRewardBasedVideo();
        //Load_Facebook_Interstitial(); // Facebook Int load request
        //Load_Facebook_Rewarded_Video(); // Load Facbook Rewarded Video Req
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }


    void OnGUI()
    {
        if (showError)
        {
            InitStyles();
            GUI.Box(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 150), "Video Not Avaiable!\n Try again later", currentStyle);
            StartCoroutine(SetFalse());
        }

    }

    private void InitStyles()
    {
        if (currentStyle == null)
        {
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.fontSize = 40;
            currentStyle.alignment = TextAnchor.MiddleCenter;

        }
    }

    IEnumerator SetFalse()
    {
        yield return new WaitForSecondsRealtime(2);
        showError = false;
    }

    /// <summary>
    /// //////////////*********Ads Sequence*******///////////
    public void AdOnLevelComplete()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResultInterstitial;

        var optionsVideo = new ShowOptions();
        options.resultCallback = HandleShowResultVideo;

        if (Advertisement.IsReady(unityInterstitialAdID))
        {
            Advertisement.Show(unityInterstitialAdID, options);
        }
        else

        if (Advertisement.IsReady(placementIdUnity))
        {
            Advertisement.Show(placementIdUnity, optionsVideo);
        }
        else
        {
            Show_Facebook_Interstitial();
        }
    }



    public void RewardedVideoAds(int type)
    {
        rewardType = type;

        var options = new ShowOptions();
        options.resultCallback = HandleShowResultRewarded;

        
        if (Advertisement.IsReady(placementIdRewarded))  // Unity Rewarded
        {
            Advertisement.Show(placementIdRewarded, options);
            Debug.Log("Showing Unity rewarded");
        }
        else if (this.admobRewardAd.IsLoaded()) // Admob Rewarded
        {
            this.admobRewardAd.Show();
            Debug.Log("Showing Admob rewarded");
        }
        else
        {
            showError = true;
            Debug.Log("Rewarded ad is not loaded.");

        }
    }


    public void OnVideoReward()
    {
        if (rewardType == 1)  // Life Reward
        {
            RewardLife();
        }
        else if (rewardType == 2)  // Double x Cash
        {
            RewardDoubleCash();  // Event
        }
        else if (rewardType == 3)  // 50 Coins
        {
            Reward50Coin();
        }
    }





    /////////////////////////////****Unity Ads*****/////////////////////////
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
        else
        {
            showError = true;
        }

    }

    void HandleShowResultRewarded(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            OnVideoReward();


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
    
    
    //////////////////////////****END REGIION UNITY ADS*******///////////////////



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



    ////////////////////////*****Admob BANNER ADS*********/////////////////////

    public void AdmobRequestSmartBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AdmobSmartBannerIDAndroid;
#elif UNITY_IPHONE
		string adUnitId = AdmobBannerIDIOS;
#else
		string adUnitId = "unexpected_platform";
#endif

		if (this.bannerViewSmart != null)
        {
            this.bannerViewSmart.Destroy();
        }

        // Create a 320x50 banner at the top of the screen.
        this.bannerViewSmart = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Called when an ad request has successfully loaded.
        bannerViewSmart.OnAdLoaded += HandleOnAdLoadedSB;
        // Called when an ad request failed to load.
        bannerViewSmart.OnAdFailedToLoad += HandleOnAdFailedToLoadSB;
        // Called when an ad is clicked.
        bannerViewSmart.OnAdOpening += HandleOnAdOpenedSB;
        // Called when the user returned from the app after an ad click.
        bannerViewSmart.OnAdClosed += HandleOnAdClosedSB;
        // Called when the ad click caused the user to leave the application.
        bannerViewSmart.OnAdLeavingApplication += HandleOnAdLeavingApplicationSB;

        // Create an empty ad request.
        this.bannerViewSmart.LoadAd(this.CreateAdRequest());
        bannerViewSmart.Hide();  // Request banner direct banner show krdeta tha
    }

    public void AdmobShowSmartBanner()
    {
        this.bannerViewSmart.Show();
    }

    public void AdmobDestroySmartBanner() // Destroying at game play
    {
        this.bannerViewSmart.Destroy();
        
    }

    public void AdmobHideSmartBanner()
    {
        this.bannerViewSmart.Hide();
    }

    // Banner events
    public void HandleOnAdLoadedSB(object sender, EventArgs args)
    {
        Debug.Log("Admob Smart Banner HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoadSB(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Admob Smart Banner HandleFailedToReceiveAd event received with message: "
        + args.Message);
    }

    public void HandleOnAdOpenedSB(object sender, EventArgs args)
    {
        Debug.Log("Admob Smart Banner HandleAdOpened event received");
    }

    public void HandleOnAdClosedSB(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Smart Banner HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplicationSB(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Smart Banner HandleAdLeavingApplication event received");
    }


    /// <summary>
    ///  Big Banner
    /// </summary>
    public void AdmobRequestBigBanner()
    {
#if UNITY_ANDROID
        string adUnitId = AdmobBigBannerIDAndroid;
#elif UNITY_IPHONE
		string adUnitId = AdmobBigBannerIDIOS;
#else
		string adUnitId = "unexpected_platform";
#endif

        if (this.bannerViewBig != null)
        {
            this.bannerViewBig.Destroy();
        }

        // Create a Medium Rect banner at the top of the screen.
        this.bannerViewBig = new BannerView(adUnitId, AdSize.MediumRectangle, AdPosition.TopLeft);

        // Called when an ad request has successfully loaded.
        bannerViewBig.OnAdLoaded += HandleOnAdLoadedBB;
        // Called when an ad request failed to load.
        bannerViewBig.OnAdFailedToLoad += HandleOnAdFailedToLoadBB;
        // Called when an ad is clicked.
        bannerViewBig.OnAdOpening += HandleOnAdOpenedBB;
        // Called when the user returned from the app after an ad click.
        bannerViewBig.OnAdClosed += HandleOnAdClosedBB;
        // Called when the ad click caused the user to leave the application.
        bannerViewBig.OnAdLeavingApplication += HandleOnAdLeavingApplicationBB;

        // Create an empty ad request.
        this.bannerViewBig.LoadAd(this.CreateAdRequest());
        bannerViewBig.Hide();  // Request banner direct banner show krdeta tha
    }

    public void AdmobShowBigBanner()
    {
        this.bannerViewBig.Show();
    }

    public void AdmobDestroyBigBanner() // Destroying at game play
    {
        this.bannerViewBig.Destroy();

    }

    public void AdmobHideBigBanner()
    {
        this.bannerViewBig.Hide();
    }

    // Banner events
    public void HandleOnAdLoadedBB(object sender, EventArgs args)
    {
        Debug.Log("Admob Big Banner HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoadBB(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("Admob Big Banner HandleFailedToReceiveAd event received with message: "
        + args.Message);
    }

    public void HandleOnAdOpenedBB(object sender, EventArgs args)
    {
        Debug.Log("Admob Big Banner HandleAdOpened event received");
    }

    public void HandleOnAdClosedBB(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Big Banner HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplicationBB(object sender, EventArgs args)
    {
        MonoBehaviour.print("Admob Big Banner HandleAdLeavingApplication event received");
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
        //this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        this.admobRewardAd = new RewardedAd(AdmobRewardedAdUnitId);


        //Called when an ad request has successfully loaded.
        this.admobRewardAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        this.admobRewardAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        this.admobRewardAd.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        this.admobRewardAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for watching a video.
        this.admobRewardAd.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        this.admobRewardAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        //this.admobRewardAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        //Load Ad
        //this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), AdmobRewardedAdUnitId);
        //create an empty ad request
        //AdRequest request = new AdRequest.Builder().Build();
        this.admobRewardAd.LoadAd(this.CreateAdRequest());
    }

    public void AdmobShowRewardVideoAd()
    {
        if (this.admobRewardAd.IsLoaded())
        {
            this.admobRewardAd.Show();
            Debug.Log("Admob rewarded ad shown");

        }
        else
        {
            Debug.Log("Admob ad is not loaded. Requested again");
            showError = true;
        }

    }
    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log(
                "HandleRewardBasedVideoFailedToLoad event received with message: "
                                 + args.Message);


    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoClosed event received");
        //this.admobRewardAd.LoadAd(this.CreateAdRequest(), AdmobRewardedAdUnitId);
        this.admobRewardAd.LoadAd(this.CreateAdRequest());
        //this.admobRewardAd.LoadAd(this.CreateAdRequest(), AdmobRewardedAdUnitId);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        OnVideoReward();
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log(
                "HandleRewardBasedVideoRewarded event received for "
                            + amount.ToString() + " " + type);
    }

    //public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    //{
    //    MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    //}
    public void Load_NextScene()
    {

        //Application.LoadLevel("AdViewScene");
        //		SceneMenagment.LoadScene("AdViewScene");

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


    // Firebase
    void FireBaseVersionCheck()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Firebase is ready");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


}
