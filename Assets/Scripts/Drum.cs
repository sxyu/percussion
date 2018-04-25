using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drum : MonoBehaviour {

	public AudioClip clip;

	private AudioSource audioSource;
	private static int lastID = 0;
	private static int id;
	private static Dictionary<int, int> dict = new Dictionary<int, int>();

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();	
		id = lastID++;
		dict [id * 2] = 0;
		dict [id * 2 + 1] = 0;
	}

	void OnCollisionEnter(Collision collision) {
		if (Time.time < 2.0) {
			return;
		}

		if (collision.gameObject.CompareTag("DrumStick")) {
			byte[] vib = { 255, 0, 255, 0, 255 };

			Drumstick stick = collision.gameObject.GetComponent<Drumstick>();

			if (stick.handController == Drumstick.HandType.left) {
				if (dict [id * 2] == 0)
					OVRHaptics.Channels[0].Preempt (new OVRHapticsClip(vib, 1));
				dict [id * 2]++;
			} else {
				if (dict [id * 2 + 1] == 0)
					OVRHaptics.Channels[1].Preempt (new OVRHapticsClip(vib, 1));
				dict [id * 2 + 1]++;
			}
			audioSource.PlayOneShot(clip);	
		}
	}

	void OnCollisionExit (Collision collision) {
		if (collision.gameObject.CompareTag ("DrumStick")) {
			byte[] vib = { 255, 0, 255, 0, 255 };

			Drumstick stick = collision.gameObject.GetComponent<Drumstick> ();

			if (stick.handController == Drumstick.HandType.left) {
				dict [id * 2]--;
			} else {
				dict [id * 2 + 1]--;
			}
		}
	}
}
