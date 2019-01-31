//using GoogleMobileAds.Api;
using UnityEngine;
using System.Collections;
using System;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
	//private RewardBasedVideoAd rewardBasedVideo;

	//public void Start()
	//{
	//	// Get singleton reward based video ad reference.
	//	rewardBasedVideo = RewardBasedVideoAd.Instance;

	//	// Called when an ad request has successfully loaded.
	//	rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
	//	// Called when an ad request failed to load.
	//	rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
	//	// Called when an ad is shown.
	//	rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
	//	// Called when the ad starts to play.
	//	rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
	//	// Called when the user should be rewarded for watching a video.
	//	rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
	//	// Called when the ad is closed.
	//	rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
	//	// Called when the ad click caused the user to leave the application.
	//	rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

	//	RequestRewardedVideo();
	//}

	//void Update(){
	//	if (rewardBasedVideo.IsLoaded ())
	//		rewardBasedVideo.Show ();
	//}

	//private void RequestRewardedVideo()
	//{
		//#if UNITY_ANDROID
		////string adUnitId = "ca-app-pub-3940256099942544/5224354917";
		//string adUnitId = "ca-app-pub-3940256099942544/7325402514";

		//#elif UNITY_IPHONE
		//string adUnitId = "ca-app-pub-3940256099942544/1712485313";
		//#else
		//string adUnitId = "unexpected_platform";
		//#endif


		//// Create an empty ad request.
		//AdRequest request = new AdRequest.Builder().Build();
		//// Load the rewarded video ad with the request.
		//rewardBasedVideo.LoadAd(request, adUnitId);

		//}

		//public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
		//{
		//MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		//}

		//public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		//{
		//MonoBehaviour.print(
		//"HandleRewardBasedVideoFailedToLoad event received with message: "
		//+ args.Message);
		//}

		//public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
		//{
		//MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		//}

		//public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
		//{
		//MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		//}

		//public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
		//{
		//MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
		//}

		//public void HandleRewardBasedVideoRewarded(object sender, Reward args)
		//{
		//string type = args.Type;
		//double amount = args.Amount;
		//MonoBehaviour.print(
		//"HandleRewardBasedVideoRewarded event received for "
		//+ amount.ToString() + " " + type);
		//}

		//public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
		//{
		//MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		//}
		}