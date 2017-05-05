using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAdsScript : MonoBehaviour {

	public void ShowBanner_Admob()
	{
		AdsManager.Instance.ShowBanner ();
	}

	public void ShowInterstitial_Admob()
	{
		AdsManager.Instance.ShowInterstitial ();
	}

	public void ShowRewardedVideo_Admob()
	{
		AdsManager.Instance.ShowRewardedVideo ();
	}

	public void RemoveBanner_AdMob()
	{
		AdsManager.Instance.RemoveBanner ();
	}
}
