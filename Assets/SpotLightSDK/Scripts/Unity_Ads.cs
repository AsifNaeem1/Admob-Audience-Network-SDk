using UnityEngine;
using UnityEngine.Advertisements;

public class Unity_Ads : MonoBehaviour
{
	string placementIdRewarded = "rewardedVideo";
	string placementIdUnity = "video";
	public string unityAdsId;
	public static Unity_Ads instance;
	void Awake()
	{
		instance = this;
	}

	void Start ()
	{  	
		if (Advertisement.isSupported) 
		{
			Advertisement.Initialize (unityAdsId, false);
			Debug.Log ("Advertiesment Supported");
		} 
		else 
		{
			Debug.Log ("Advertiesment NOT Supported");
		}
	}

	public void ShowUnityRewardedAd ()
	{
		var options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		if(Advertisement.IsReady(placementIdRewarded))
			Advertisement.Show(placementIdRewarded, options);
	}


	public void ShowUnityAd()
	{
		var options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		if(Advertisement.IsReady(placementIdUnity))
			Advertisement.Show(placementIdUnity, options);
	}

	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");

		}else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}
}