using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using AudienceNetwork;
using UnityEngine.SceneManagement;

public class InterstitialAdScene : MonoBehaviour
{
    public string fbInterstitalID;
    private InterstitialAd interstitialAd;
    public bool isLoaded;
#pragma warning disable 0414
    private bool didClose;
#pragma warning restore 0414
    // UI elements in scene
    //public Text statusLabel;

    private void OnEnable()
    {
        LoadInterstitial();
    }

    // Load button
    public void LoadInterstitial()
    {
        //this.statusLabel.text = "Loading interstitial ad...";

        // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
        // Use different ID for each ad placement in your app.
        this.interstitialAd = new InterstitialAd(fbInterstitalID);
        
        this.interstitialAd.Register(this.gameObject);

        // Set delegates to get notified on changes or when the user interacts with the ad.
        this.interstitialAd.InterstitialAdDidLoad = (delegate() {
            Debug.Log("Interstitial ad loaded.");
            this.isLoaded = true;
            this.didClose = false;
            //this.statusLabel.text = "Ad loaded. Click show to present!";
        });
        interstitialAd.InterstitialAdDidFailWithError = (delegate(string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
            //this.statusLabel.text = "Interstitial ad failed to load. Check console for details.";
        });
        interstitialAd.InterstitialAdWillLogImpression = (delegate() {
            Debug.Log("Interstitial ad logged impression.");
        });
        interstitialAd.InterstitialAdDidClick = (delegate() {
            Debug.Log("Interstitial ad clicked.");
        });

        this.interstitialAd.InterstitialAdDidClose = (delegate() {
            Debug.Log("Interstitial ad did close.");
            this.didClose = true;
            LoadInterstitial();
            if (this.interstitialAd != null) {
                this.interstitialAd.Dispose();
            }
            LoadInterstitial();
        });

#if UNITY_ANDROID
        /*
         * Only relevant to Android.
         * This callback will only be triggered if the Interstitial activity has
         * been destroyed without being properly closed. This can happen if an
         * app with launchMode:singleTask (such as a Unity game) goes to
         * background and is then relaunched by tapping the icon.
         */
        this.interstitialAd.interstitialAdActivityDestroyed = (delegate() {
            if (!this.didClose) {
                Debug.Log("Interstitial activity destroyed without being closed first.");
                Debug.Log("Game should resume.");
            }
        });
#endif

        // Initiate the request to load the ad.
        this.interstitialAd.LoadAd();
    }

    // Show button
    public void ShowInterstitial()
    {
        if (this.isLoaded)
        {
            this.interstitialAd.Show();
            this.isLoaded = false;
            //this.statusLabel.text = "FB Int shown";
        }
        else
        {
            LoadInterstitial();
        }
        }



    public void FBIntDestroy()
    {
        // Dispose of interstitial ad when the scene is destroyed
        if (this.interstitialAd != null) {
            this.interstitialAd.Dispose();
        }
        Debug.Log("InterstitialAdTest was destroyed!");
    }


}
