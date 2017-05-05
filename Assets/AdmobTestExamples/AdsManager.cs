using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class AdsManager : MonoBehaviour {

	public static AdsManager Instance { set; get; }

	public string AdMob_BannerID = "ca-app-pub-8725679997372244/4435644010";
	public string AdMob_InterstitialID = "ca-app-pub-8725679997372244/8865843613";
	public string AdMob_RewardedVideoID = "ca-app-pub-8725679997372244/8865843613";


	// Use this for initialization
	void Start ()
	{
		Instance = this;
		DontDestroyOnLoad (gameObject);
			
		Admob.Instance ().initAdmob (AdMob_BannerID, AdMob_InterstitialID);
		Admob.Instance ().loadInterstitial ();
		Admob.Instance ().loadRewardedVideo (AdMob_RewardedVideoID);

		ShowBanner ();
	}

	public void ShowBanner()
	{
		Admob.Instance ().showBannerRelative (AdSize.SmartBanner, AdPosition.BOTTOM_CENTER, 5);
	}

	public void ShowInterstitial()
	{
		if (Admob.Instance ().isInterstitialReady ())
		{
			Admob.Instance ().showInterstitial ();
		}
		Admob.Instance ().loadInterstitial ();
	}

	public void ShowRewardedVideo()
	{
		if (Admob.Instance ().isRewardedVideoReady ())
		{
			Admob.Instance ().showRewardedVideo ();
		}
		Admob.Instance ().loadRewardedVideo (AdMob_RewardedVideoID);
	}

	public void RemoveBanner()
	{
		Admob.Instance ().removeBanner ();
	}




}
