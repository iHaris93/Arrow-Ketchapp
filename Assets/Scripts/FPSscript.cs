using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//[AddComponentMenu("RVP/C#/Demo Scripts/Performance Stats", 1)]

//Class for displaying the framerate
public class FPSscript : MonoBehaviour
{
	public Text fpsText;
	float fpsUpdateTime;
	int frames;
	
	void Update()
	{
		fpsUpdateTime = Mathf.Max(0, fpsUpdateTime - Time.deltaTime);

		if (fpsUpdateTime == 0)
		{
			fpsText.text = "FPS: " + frames.ToString();
			fpsUpdateTime = 1;
			frames = 0;
		}
		else
		{
			frames ++;
		}
	}
}