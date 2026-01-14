using UnityEngine;
using System.Collections;

public class Tail : MonoBehaviour {

	private int order;
	private Transform head;
	private Controller headController;
	private Vector3 movementVelocity;
	[Range(0.0f,1.0f)]
	public float followSmoothTime = 0.2f;
	private Vector3 lookPos;

	// Called from Controller when the tail segment is created
	public void Initialize(Controller controller, int order)
	{
		this.headController = controller;
		this.head = controller.transform;
		this.order = order;
	}

	void Start(){
		// Fallback for tails that were not initialized explicitly (e.g. placed manually in the scene)
		if (headController == null)
		{
			GameObject headObject = GameObject.FindGameObjectWithTag("Player");
			if (headObject != null)
			{
				headController = headObject.GetComponent<Controller>();
				head = headController != null ? headController.transform : null;
				if (headController != null)
				{
					for (int i = 0; i < headController.tailParts.Count; i++)
					{
						if (gameObject == headController.tailParts[i].gameObject)
						{
							order = i;
							break;
						}
					}
				}
			}
		}
	}

	void Update(){
		if (headController == null || head == null)
			return;

		// Determine which transform this segment should follow
		Transform target = (order == 0)
			? head
			: headController.tailParts[Mathf.Clamp(order - 1, 0, headController.tailParts.Count - 1)];

		lookPos = target.position - transform.position;
		float rotationZ = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
		transform.position = Vector3.SmoothDamp(transform.position, target.position, ref movementVelocity, followSmoothTime);
	}
}
