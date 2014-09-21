using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timeUpdate : MonoBehaviour {
	public Text time;
	int lenghtSong;
	int timeLeft;
	AudioSource clipSender;
	bool ended = false;

	public GameObject player1, player2;

	public GameObject retryMenu;
	public GameObject retryText;

	public GameObject croud;

	public GameObject socketioObject;
	teSocket socketio;
		
	// Use this for initialization
	void Start () {
		clipSender = GetComponent<AudioSource>();
		lenghtSong = (int)(clipSender.clip.length);
		time.text = ""+lenghtSong;
		timeLeft = lenghtSong;
		socketio = socketioObject.GetComponent<teSocket>();
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft = (int)(lenghtSong-clipSender.time);
		if (timeLeft <= 0) {
						time.text = "000";
						if (!ended) {
								retryMenu.transform.position = Vector3.zero;
								Debug.Log ("ENDED");
								ended = true;
								socketio.disconnect ();
								croud.GetComponent<AudioSource> ().Play ();
								if (player1.GetComponent<songManager> ().score > player2.GetComponent<songManager> ().score)
										retryText.GetComponent<Text> ().text = "¡El jugador 1 gana!";
								else if (player1.GetComponent<songManager> ().score < player2.GetComponent<songManager> ().score)
										retryText.GetComponent<Text> ().text = "¡El jugador 2 gana!";
								else
										retryText.GetComponent<Text> ().text = "¡Wow! ¡Es un empate!";
			
						}
				}
		else if(timeLeft >=100) time.text = ""+timeLeft;
		else if(timeLeft < 100 && timeLeft >= 10) time.text = "0"+timeLeft;
		else time.text = "00"+timeLeft;

 		
	}
}
