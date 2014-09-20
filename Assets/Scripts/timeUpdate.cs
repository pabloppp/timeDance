using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timeUpdate : MonoBehaviour {
	Text time;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		time = GetComponent<Text>();
		float lenghtSong = audioSource.clip.length;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
