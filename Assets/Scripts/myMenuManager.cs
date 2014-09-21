using UnityEngine;
using System.Collections;

public class myMenuManager : MonoBehaviour {

	public GameObject songController;
	public GameObject menu;
	public GameObject waitOnline;


	public GameObject jackson;

	public TextAsset songData;

	public bool online = false, startOnline = false, backBool = false;

	public GameObject socketioObject;
	teSocket socketio;

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		menu.transform.position = Vector3.zero;
		audioSource = GetComponent<AudioSource> ();
		socketio = socketioObject.GetComponent<teSocket>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void quitGame(){
		Application.Quit();
	}

	public void startSong(){
		online = false;
		GameObject.Find ("songManager 1st").SendMessage("goOffline");
		GameObject.Find ("songManager 2nd").SendMessage("goOffline");
		StartCoroutine("startSongCoroutine");
	}

	public void startSongOnline(){
		online = true;
		socketio.initialize();
		GameObject.Find ("songManager 1st").SendMessage("goOnline");
		GameObject.Find ("songManager 2nd").SendMessage("goOnline");
		StartCoroutine ("waitForOnline");
	}

	public void runOnline(){
		startOnline = true;
		//StartCoroutine("startSongCoroutine");
	}

	public void back(){
		backBool = true;
	}

	IEnumerator waitForOnline(){
		menu.transform.position = Vector3.up * 30;
		waitOnline.transform.position = Vector3.zero;
		while(!startOnline && !backBool) yield return new WaitForSeconds(0.001f);
		if (startOnline) {
				Debug.Log ("RUN");
				StartCoroutine ("startSongCoroutine");
		}
		else {
			waitOnline.transform.position = Vector3.up * 30;
			menu.transform.position = Vector3.zero;
			backBool = false;
			socketio.disconnect();
		}
	}

	IEnumerator startSongCoroutine(){
		float posy = menu.transform.position.y;
		while(posy < 10){
			yield return new WaitForSeconds(0.01f);
			posy += 15f*Time.deltaTime;
			menu.transform.position = new Vector3(menu.transform.position.x, posy, menu.transform.position.z);
		}
		yield return new WaitForSeconds(2);
		if(menu != null)menu.SetActive(false);
		jackson.SendMessage ("startDancing");
		if(songController != null && songController.GetComponent<songManager>() != null)
				audioSource.Play ();
		}
	
}
