using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
	public void UnityAds()
	{
        //SpotLightSDK.instance.UnityAds ();
	}

	public void AdBeforeGP()
	{
//        SpotLightSDK.instance.AdBeforeGamePlay ();
	}

//	public void ShowBanner()
//	{
////        SpotLightSDK.instance.ShowBanner ();
	//}

	//public void HidBanner()
	//{
 //       SpotLightSDK.instance.HideBanner ();
	//}

	public void MoreGames()
	{
        SpotLightSDK.instance.MoreGamesUrl ();
	}

	public void FaceBook()
	{
        SpotLightSDK.instance.FaceBookUrl ();
	}

	public void Twitter()
	{
        SpotLightSDK.instance.Twitter ();
	}

	public void Gmail()
	{
//        SpotLightSDK.instance.SendEmail();
	}

	public void AdoOnLC()
	{
//        SpotLightSDK.instance.AdOnLevelComplete ();
	}

	public void ReloadScene()
	{
		SceneManager.LoadScene ("SpotLight SDK");
	}
}
