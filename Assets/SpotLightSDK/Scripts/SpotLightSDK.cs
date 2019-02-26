using System;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
//using System.Security.Policy;
using UnityEngine.UI;
//using AudienceNetwork;

public enum BannerSize 
{
	Banner,
	MediumRectangle,
	IABBanner,
	Leaderboard,
	SmartBanner
}

public class SpotLightSDK : MonoBehaviour 
{
    public static SpotLightSDK instance;
    public bool autoInatializeSdk;
    public string bundleIdentifier;
    [Header("App Store")]
    public string admobBannerIdIOS;
	public string interestitialIdIOS;
	public string admobRewardedIOS;
	public string unityIdIos;
    public string rateUsIOS;
    string moreGamesIOS = "https://itunes.apple.com/us/developer/asif-nadeem/id1441754142";

    [Header("Play Store")]
    public string bannerIDAndriod;
    public string interestitialIDAndriod;
    public string admobRewardedAndriod;
    public string unityIdAndroid;
    //public string rateUsPlayStore;
    public string moreGamesPlayStore;

    //public bool banner = false;
    public BannerSize bannerSize = BannerSize.SmartBanner;
    public AdPosition bannerPosition = AdPosition.Top;


    private InterstitialAd interstitial;
    bool isGoogleIntLoaded;
    public bool isGoogleRewardedVideoLoad;
    //bool isUnityRewardedLoad;
    //public bool isUnityRewardedAvailable;
    public bool isLifeReward;
    //public bool lifeReward;
	//private BannerView bannerView;
	private AdRequest request;   // Admob Interstitial Request call

    public RewardBasedVideoAd rewardBasedVideo;
    private int interstitialCount = 0;
    private int rewardedVideoCount = 0;
    //Unity IDs
    string placementIdRewarded = "rewardedVideo";
    string placementIdUnity = "video";

    [Header("Facebook Ads")]
    InterstitialAdScene fbInterstitial;
    AdViewScene fbBanner;





    void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

	void Start () 
	{
		if (Debug.isDebugBuild) 
		{
			//Admob test ads ids for IOS
            admobBannerIdIOS = "ca-app-pub-3940256099942544/2934735716";
            interestitialIdIOS = "ca-app-pub-3940256099942544/4411468910";
            admobRewardedIOS = "ca-app-pub-3940256099942544/1712485313";

            bannerIDAndriod = "ca-app-pub-3940256099942544/6300978111";
            interestitialIDAndriod = "ca-app-pub-3940256099942544/1033173712";
            admobRewardedAndriod = "ca-app-pub-3940256099942544/5224354917";
            

		}

        fbInterstitial = GetComponent<InterstitialAdScene>(); //Getting facebook interstitial ad object
        fbBanner = GetComponent<AdViewScene>();
        MobileAds.SetiOSAppPauseOnBackground(true);
		DontDestroyOnLoad(gameObject);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AdmobRewardedInstance(); // Getting the instance of Admob rewarded

        if (autoInatializeSdk)
        {
            InitializeUnityAds();
            LoadAdmobInterstitial();
            LoadAdmobRewardedVideo();
            LoadFacebookInterstitial();
            //ShowFacebookBanner();
        }


    }

	void InitializeUnityAds()
	{
		bool unityTestAds = false;
		if (Debug.isDebugBuild) 
		{
			unityTestAds = true;
		} 
		else 
		{
			unityTestAds = false;
		}

		#if UNITY_IOS
		if(Advertisement.isSupported)
			Advertisement.Initialize (unityIDiOS, unityTestAds);
		#elif UNITY_ANDROID
		if(Advertisement.isSupported)
			Advertisement.Initialize (unityIdAndroid, unityTestAds);
		#endif
	}
	//Admob Ads
	//private void RequestBanner()
	//{
	//	#if UNITY_ANDROID
	//	string adUnitId = bannerIDAndriod;
	//	#elif UNITY_IPHONE
	//	string adUnitId = admobBannerIdIOS;
	//	#else
	//	string adUnitId = "unexpected_platform";
	//	#endif

 //       if(this.bannerView !=null){
 //           this.bannerView.Destroy();
 //       }

	//	switch(bannerSize)
	//	{
	//		case BannerSize.Banner:
	//			bannerView = new BannerView (adUnitId, AdSize.Banner, bannerPosition);
	//			break;
	//		case BannerSize.IABBanner:
	//			bannerView = new BannerView (adUnitId, AdSize.IABBanner, bannerPosition);
	//			break;
	//		case BannerSize.Leaderboard:
	//			bannerView = new BannerView (adUnitId, AdSize.Leaderboard, bannerPosition);
	//			break;
	//		case BannerSize.MediumRectangle:
	//			bannerView = new BannerView (adUnitId, AdSize.MediumRectangle, bannerPosition);
	//			break;
	//		case BannerSize.SmartBanner:
	//			bannerView = new BannerView (adUnitId, AdSize.SmartBanner, bannerPosition);
	//			break;
	//	}
	//	// Create an empty ad request.
	//	AdRequest request = new AdRequest.Builder().Build();
	//	// Load the banner with the request.
	//	bannerView.LoadAd(request);
	//}

	//public void AmobHideBanner()      **** Not Using Admob Banner in Alaska Mountain ****
	//{
 //       bannerView.Hide ();
 //       //print("Working in hide banner");
 //       //this.bannerView.Destroy();
	//}
	
	//public void AdmobShowBanner()
	//{
	//	bannerView.Show ();
	//}

	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = interestitialIDAndriod;
		#elif UNITY_IPHONE
		string adUnitId = interestitialIdIOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoadedInt;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoadInt;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpenedInt;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosedInt;
        // Called when the ad click caused the user to leave the application.
        this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;



		// Create an empty ad request.
		request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
		//interstitial.Show ();  //Why showing here???
	}
    void HandleOnAdLoadedInt(object sender, EventArgs args){
        isGoogleIntLoaded = true;
    }

    void HandleOnAdFailedToLoadInt(object sender, AdFailedToLoadEventArgs args){
        isGoogleIntLoaded = false;
    }

    void HandleOnAdOpenedInt(object sender, EventArgs args){
        //Time.timeScale = 0;
        AudioListener.volume = 0f;
        
    }

    void HandleOnAdClosedInt(object sender, EventArgs args){
        //Time.timeScale = 1f;
        AudioListener.volume = 1f;
        isGoogleIntLoaded = false;
        RequestInterstitial();

    }

    void HandleOnAdLeavingApplication(object sender, EventArgs args){
        
    }

    ////////////////////////// UNITY ADS///////////////////////

    public void ShowUnityRewardedAd ()
	{
		var options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		if(Advertisement.IsReady(placementIdRewarded))
			Advertisement.Show(placementIdRewarded, options);
	}

	public void ShowUnityVideoAd()
	{
		var options = new ShowOptions();
        options.resultCallback = HandleShowResultVideo;

		if(Advertisement.IsReady(placementIdUnity))
			Advertisement.Show(placementIdUnity, options);
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

	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");
            CashReward();
        }
        else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}
///////////////////////////END REGIION UNITY ADS///////////////////////


    public void LoadAdmobInterstitial()
    {
        RequestInterstitial();
    }
    public void ShowAdmobInterstitial()
    {
        interstitial.Show();
    }

    public void LoadFbInterstitial()
    {
        fbInterstitial.LoadInterstitial();
    }

    public void ShowFbInterstitial()
    {
        fbInterstitial.ShowInterstitial();
    }

    public void ShowAdmobRewarded()
    {
        rewardBasedVideo.Show();
    }



    public void ShowAllInterstitialAds() // Admob & Facebook Interstitial
    {
        if(interstitialCount % 2== 0)
        {
            interstitialCount += 1;
            if (isGoogleIntLoaded)
            {
                ShowAdmobInterstitial();
            }
            else if(fbInterstitial.isLoaded)
            {
                ShowFbInterstitial();
            }
            else
            {
                Debug.Log("Interstitial Failed for 1");
            }
        }
        else if(interstitialCount % 2 == 1)
        {
            interstitialCount += 1;
            if (fbInterstitial.isLoaded)
            {
                ShowFbInterstitial();
            }
            else if(isGoogleIntLoaded)
            {
                LoadFbInterstitial();
                ShowAdmobInterstitial();
            }
            else
            {
                Debug.Log("Interstitial Failed for 2");
            }

        }
    }

    public void ShowAllRewardedVideo() // Admob & Unity Rewarded Video
    {
        if(rewardedVideoCount %2 == 0) // First iteration
        {
            rewardedVideoCount += 1;
            if (isGoogleRewardedVideoLoad)
            {
                ShowAdmobRewarded();
            }
            else if (Advertisement.IsReady(placementIdRewarded))
            {
                ShowUnityRewardedAd();
            }
            else
            {
                Debug.Log("No Rewarded Video is Avaiable");
            }

        }else if(rewardedVideoCount % 2 == 1) // Second iteration
        {
            rewardedVideoCount += 1;
            if (Advertisement.IsReady(placementIdRewarded))
            {
                ShowUnityRewardedAd();
            }else if (isGoogleRewardedVideoLoad)
            {
                ShowAdmobRewarded();
            }
            else
            {
                Debug.Log("No Rewarded Video is Avaiable in video count ++");
            }

        }


    }
    

    /////////////////////ADMOB REWARDED VIDEO ADD/////////



    private void AdmobRewardedInstance() // Getting Instance & Event Subscriber
	{
		// Get singleton reward based video ad reference.
		rewardBasedVideo = RewardBasedVideoAd.Instance;

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

	}






	public void LoadAdmobRewardedVideo()
	{
        isGoogleRewardedVideoLoad = false;  //Set False before Another request
		#if UNITY_ANDROID
		//string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		string adUnitId = admobRewardedAndriod;

		#elif UNITY_IPHONE
		string adUnitId = admobRewardedIOS;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded video ad with the request.
		rewardBasedVideo.LoadAd(request, adUnitId);

	}



	public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
	{
        Debug.Log("Inside Handle Reward Based Video Load");
		MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        isGoogleRewardedVideoLoad = true;
	}

	public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
        Debug.Log("Inside Handle Reward Based Video fail to Load");
		MonoBehaviour.print(
			"HandleRewardBasedVideoFailedToLoad event received with message: "
			+ args.Message);
        isGoogleRewardedVideoLoad = false;
	}

	public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
	{
        AudioListener.volume = 0f;
        Debug.Log("Inside Handle Reward Based Video Opened");
		MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
	}

	public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
	{
        Debug.Log("Inside Handle Reward Based Video Start");
		MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
	}

	public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
	{
        AudioListener.volume = 1f;
        LoadAdmobRewardedVideo();

        Debug.Log("Inside Handle Reward Based Video Closed");
		MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
	}

	public void HandleRewardBasedVideoRewarded(object sender, Reward args)
	{
        Debug.Log("Inside Handle Reward Based Video reward awarding");
        
        CashReward();

    }

	public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
	}




    /// <summary>
    /// Facebook Audionce network Inatialization
    /// </summary>
    public void LoadFacebookInterstitial()
    {
        GetComponent<InterstitialAdScene>().LoadInterstitial();
    }
    public void ShowFacebookInterstital()
    {
        GetComponent<InterstitialAdScene>().ShowInterstitial();
    }

    public void ShowFacebookBanner()
    {
       fbBanner.LoadBanner();
    }

    public void HideFacebookBanner()
    {
        fbBanner.OnDestroFbBanner();
    }

    public void CashReward()
    {
        if (isLifeReward)
        {
//            GameManager.Instance.PlayerRevive();
            isLifeReward = false;
        }
        else
        {
            PlayerPrefs.SetInt("Currency", PlayerPrefs.GetInt("Currency") + 500);
        }

    }


    //  //Social Media Links
    public void MoreGamesUrl()
    {
#if UNITY_ANDROID
        Application.OpenURL(moreGamesPlayStore);
        //Debug.Log("Visited more fun link Andriod");
#elif UNITY_IPHONE
                Application.OpenURL(moreGamesIOS);
                Debug.Log("Visited more fun link IOS");
#else
                string adUnitId = "unexpected_platform";
#endif
    }


    public void RateUsUrl()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + bundleIdentifier);
#elif UNITY_IPHONE
        Application.OpenURL (rateUsIOS);
#endif
        Debug.Log("Visited rate us link");
    }

    public void FaceBookUrl()
    {
        Application.OpenURL("https://www.facebook.com/SpotLightGamesStudio/");
        Debug.Log("Visited Facebook link");
    }

    public void Twitter()
    {
        Debug.Log("Visited Twitter link");
    }

    public void Gmail()
    {
        Application.OpenURL("https://plus.google.com/");
        Debug.Log("Visited Gamil link");
    }



}