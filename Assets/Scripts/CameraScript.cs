using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
	public Transform Player;
	//How much far should the camera be from Play on Y-Axis?
	public float Offset;
	private static CameraScript instance;
	[HideInInspector]
	public Camera TheCamera;
	public static CameraScript Instance{
		get { return instance; }
	}
	void Awake(){
		instance = this;
		TheCamera = GetComponent<Camera> ();
	}
	void Update(){
		if (EventsManager.Instance.CurrentScreen == (int)GameState.Gameplay)
			transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x,Player.transform.position.y+Offset,transform.position.z), 0.05f);
	}
	public void Init(){
		transform.position = new Vector3 (transform.position.x, Player.transform.position.y + Offset, transform.position.z);
	}
}