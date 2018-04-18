using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour {

	public AudioClip clip;

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.CompareTag("DrumStick")) {
			byte[] vib = { 255, 0, 255, 0, 255 };
			if (OVRInput.Get (OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch) > 0.0f) {
				OVRHaptics.Channels[0].Preempt (new OVRHapticsClip (vib, 1));
			} else {
				OVRHaptics.Channels[1].Preempt (new OVRHapticsClip (vib, 1));
			}
			audioSource.PlayOneShot(clip);	
		}
	}
}
