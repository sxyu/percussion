using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour {

	public OVRInput.Controller leftController;
	public OVRInput.Controller rightController;
	public GameObject recordingText, playingText;
	public AudioSource audioSource;
	private AudioListener audioListener;
	private bool recording = false, recordingStart = false;
	private List<float> audio = new List<float>();
	private float lastRecordPressTime = -100.0f, lastPlayPressTime = -100.0f;
	private float eps = 0.001f;

	// Use this for initialization
	void Start () {
		audioListener = GetComponent<AudioListener>();
	}
	
	// Update is called once per frame
	void Update () {
		if (OVRInput.Get (OVRInput.Button.Two, rightController)) {
			if (Time.time - lastRecordPressTime > 1.0f){
				lastRecordPressTime = Time.time;
				if (!recording) {
					audioSource.Stop ();
					audio.Clear ();
					recordingStart = false;
				}

				recording = !recording;
				recordingText.SetActive(recording);
			}
		}
		if (!recording && OVRInput.Get (OVRInput.Button.Two, leftController)) {
			if (Time.time - lastPlayPressTime > 1.0f) {
				lastPlayPressTime = Time.time;
				if (!audioSource.isPlaying) {
					recording = false;
					recordingText.SetActive(false);
					if (audio.Count > 0) {
						for (int i = audio.Count - 1; i > 0; --i) {
							if (Mathf.Abs (audio [i]) > eps) {
								break;
							}
							audio.RemoveAt (i);
						}
						AudioClip clip = AudioClip.Create ("playback", audio.Count, 2, 48000, false);
						clip.SetData (audio.ToArray (), 0);
						audioSource.PlayOneShot (clip);
					}
				} else {
					audioSource.Stop ();
				}
			}
		}

		playingText.SetActive (audioSource.isPlaying);
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		if (recording)
		{
			if (!recordingStart) {
				foreach (float f in data) {
					if (Mathf.Abs (f) > eps) {
						recordingStart = true;
						break;
					}
				}
				if (!recordingStart) {
					return;
				}
			}
			audio.AddRange (data);
		}
	}
}
