using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	public OVRInput.Controller controller;
	public bool isLeftHand;

	private float indexTriggerState = 0;
	private float handTriggerState = 0;
	private float oldIndexTriggerState = 0;

	private bool holdingStick = false;
	private GameObject stick = null;

	public Vector3 holdPosition = new Vector3(0, -0.025f, 0.03f);
	public Vector3 holdRotation = new Vector3(0, 180, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		oldIndexTriggerState = indexTriggerState;
		indexTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
		handTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
		if (holdingStick) {
			if (handTriggerState < 0.6f) {
				Release();
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.CompareTag("DrumStick")) {
			if (handTriggerState > 0.9f && !holdingStick) {
				Grab(other.gameObject);
			}
		}
	}

	void Grab(GameObject obj) {
		holdingStick = true;
		stick = obj;

		stick.transform.parent = transform;
		stick.transform.localPosition = holdPosition;
		stick.transform.localEulerAngles = holdRotation;
		stick.GetComponent<Rigidbody>().useGravity = false;
		stick.GetComponent<Drumstick> ().handController = isLeftHand ? Drumstick.HandType.left : Drumstick.HandType.right;
		//stick.GetComponent<Rigidbody>().isKinematic = true;
	}

	void Release() {
		stick.transform.parent = null;

		Rigidbody rigidbody = stick.GetComponent<Rigidbody>();

		rigidbody.useGravity = true;
		//rigidbody.isKinematic = false;
		rigidbody.velocity = OVRInput.GetLocalControllerVelocity(controller);

		holdingStick = false;
		stick = null;
	}
}
