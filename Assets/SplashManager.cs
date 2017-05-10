using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour {

	AsyncOperation AO;

	// Use this for initialization
	void Start () {

		Handheld.PlayFullScreenMovie ("ShooterboySplash.mp4", Color.black, FullScreenMovieControlMode.Hidden, FullScreenMovieScalingMode.AspectFit);
		AO = SceneManager.LoadSceneAsync (1, LoadSceneMode.Single);
		AO.allowSceneActivation = false;
		Invoke ("ChangeScene", 2f);
	}

	void ChangeScene()
	{
		AO.allowSceneActivation = true;	
	}

}
