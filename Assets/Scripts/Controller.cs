using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	[Header("Movement")]
	[Tooltip("Turning Angle for the Character")]
	[Range(1,90)]
	public int AngleInDegrees;
	[Tooltip("Desired forward speed of the character")]
	public float ForwardSpeed;
	[Tooltip("Time (in seconds) to accelerate from 0 to ForwardSpeed")]
	public float AccelerationTime = 2f;
	[Tooltip("How fast the arrow rotates towards its target angle")]
	public float TurnSpeed = 6f;
	[Tooltip("Initial position of the arrow when a run starts")]
	public Vector3 StartPosition = new Vector3(0f, -5f, 0f);

	private float CurrentForwardSpeed = 0f;
	private float SpeedingEffect;

	// Is the player currently holding/tapping the input?
	private bool isTurningRight = false;

	// Keeps track of active Tail Part Game Objects
	private int TailCount;
	public GameObject TheParticlesss;

	[Header("Tail / Trail")]
	// This holds the information on Tail Parts of the Arrow
	[HideInInspector]
	public List<Transform> tailParts = new List<Transform> ();
	// Maximum Tail pool count for visual representation
	public int TailPoolCount;
	[Tooltip("Initial spacing (in world units) between spawned tail segments")]
	public float TailInitialSpacing = 5f;

	// Useful data members for adding Tail objects to Character's Head (Arrow)
	private Vector3 currentPos;
	private GameObject theTailPart;
	public GameObject tailPrefab;
	public Transform tailHolder;

	private static Controller instance;
	public static Controller Instance{
		get {return instance;}
	}

	void Awake(){
		if (instance != null && instance != this) {
			Destroy(gameObject);
			return;
		}
		instance = this;
		// Calculate how much speed to add each FixedUpdate so we reach ForwardSpeed in AccelerationTime seconds
		float stepsToMaxSpeed = Mathf.Max(1f, AccelerationTime / Time.fixedDeltaTime);
		SpeedingEffect = ForwardSpeed / stepsToMaxSpeed;
		for (int i = 0; i < TailPoolCount; i++) {
			//Instantiating all the 15 tail parts in the begining, and then later setting them active and deactive
			//This optimization technique is called object pooling 
			InstantiateTail ();
			tailParts [i].transform.gameObject.SetActive (false);
		}
	}

	public void Init(){
		CurrentForwardSpeed = 0f;
		TailCount = 0;
		isTurningRight = false;
		transform.position = StartPosition;
		transform.eulerAngles = new Vector3 (0f, 0f, -AngleInDegrees);
		transform.gameObject.SetActive (true);
	}

	void Update(){
		if (EventsManager.Instance.CurrentScreen == GameState.Gameplay) {
			if (CurrentForwardSpeed < ForwardSpeed)
				DoSpeedingEffect();
			MoveForward ();
			ChangeDirection ();
		}
	}

	private void DoSpeedingEffect(){
		CurrentForwardSpeed += SpeedingEffect;
	}

	//Character always moves forward
	private void MoveForward(){
		transform.Translate (transform.up * CurrentForwardSpeed * Time.deltaTime, Space.World);
	}

	// Handles steering based on current input state
	private void ChangeDirection(){
		float targetAngle = isTurningRight ? AngleInDegrees : -AngleInDegrees;
		float currentAngle = FetchAngle(transform.rotation.eulerAngles.z);
		float newAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * TurnSpeed);
		transform.eulerAngles = new Vector3(0f, 0f, newAngle);
	}


	public void OnClickTapDown(){
		isTurningRight = true;
	}

	public void OnClickTapUp(){
		isTurningRight = false;
	}

	//Converting the second quadrant angle values into negative e.g 330 = -30
	public float FetchAngle(float Angle){
		//Don't really have to perform conversion for the first quadrant

		//Perform conversion if the Angle is in second quadrant
		if (Angle >= 270 && Angle < 360) {
			Angle -= 360;
		}
		//3rd and 4th Quadrant, this should NOT happen and this block of code is not supposed to execute since we're 
		//only dealing with first and second quadrant in this game
		else if (Angle > 90 && Angle < 270) {
			//Just for house-keeping
			Mathf.Clamp (Angle,-AngleInDegrees,AngleInDegrees);
		}
		return Angle;
	}

	// Instantiates a tail object and registers it with this controller
	void InstantiateTail(){
		if (tailParts.Count == 0)
			currentPos = transform.position;		
		else
			currentPos = tailParts [tailParts.Count - 1].position;

		theTailPart = Instantiate (tailPrefab, currentPos + new Vector3(TailInitialSpacing, 0f, 0f), Quaternion.Euler(0f,0f,180f)) as GameObject;
		theTailPart.transform.SetParent (tailHolder);

		// Keep track of the transform and inform the Tail script of its order
		var tailComponent = theTailPart.GetComponent<Tail>();
		if (tailComponent != null)
		{
			int newOrder = tailParts.Count;
			tailComponent.Initialize(this, newOrder);
		}

		tailParts.Add (theTailPart.transform);
	}

	void AddTail(){
		tailParts [TailCount].transform.position = (TailCount == 0) ? transform.position : tailParts [TailCount-1].transform.position;
		tailParts [TailCount].transform.gameObject.SetActive (true);
		TailCount++;
	}


	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Collectible")) {
			HandleCollectible(other);
		}
		else if (other.CompareTag("Gem")) {
			HandleGem(other);
		}
		else if (other.CompareTag("Wall")) {
			HandleWall();
		}
		else if (other.CompareTag("SpawnNew")) {
			HandleSpawnNew();
		}
	}

	private void HandleCollectible(Collider2D other){
		TheParticlesss.SetActive (false);
		TheParticlesss.transform.position = other.transform.position;
		TheParticlesss.SetActive (true);
		Gamedata.Instance.AddScore (1);
		if (TailCount < TailPoolCount)
			AddTail ();
		AudioManager.Instance.PlayCollectible();
		other.transform.gameObject.SetActive(false);
	}

	private void HandleGem(Collider2D other){
		Gamedata.Instance.AddGems (1);
		AudioManager.Instance.PlayGem ();
		EndlessScroller.Instance.TransitionColor ();
		other.transform.gameObject.SetActive(false);
	}

	private void HandleWall(){
		AudioManager.Instance.PlayGameover ();
		Die();
		EventsManager.Instance.DisplayGameover ();
	}

	private void HandleSpawnNew(){
		EndlessScroller.Instance.SpawnNextPrefab ();
	}

	public void Die(){
		//Kill the head
		transform.gameObject.SetActive (false);
		//Kill the rest of the tail parts
		for (int i = 0; i < tailParts.Count; i++) {
			tailParts [i].transform.gameObject.SetActive (false);
		}
	}
}
