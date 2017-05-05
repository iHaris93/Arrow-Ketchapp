using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

	void Start()
	{
		Invoke ("LoadMainScene", 1);
	}

	void LoadMainScene()
	{
		SceneManager.LoadScene (1);
	}

}
