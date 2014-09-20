using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timeUpdate : MonoBehaviour {
	public Text time;
	int lenghtSong;
	int timeLeft;
	AudioSource clipSender;

	// Use this for initialization
	void Start () {
		time = GetComponent<Text>();
		clipSender = GetComponent<AudioSource>();
		lenghtSong = (int)(clipSender.clip.length);
		time.text = ""+lenghtSong;
		timeLeft = lenghtSong;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft = (int)(lenghtSong-clipSender.time);
		if(timeLeft <= 0) time.text = "000";
		else if(timeLeft >=100) time.text = ""+timeLeft;
		else if(timeLeft < 100 && timeLeft >= 10) time.text = "0"+timeLeft;
		else{ 
			time.text = "00"+timeLeft;
		}
 		
	}
}
