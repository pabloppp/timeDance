using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class myMenuManager : MonoBehaviour {

	public GameObject songController;
	public GameObject menu;
	public GameObject waitOnline;


	public GameObject jackson;
	public GameObject textP1, textP2;

	public TextAsset songData;

	public bool online = false, startOnline = false, backBool = false, skipped = false, startedSong = false;

	public GameObject croud;
	AudioSource croudSource;

	public GameObject socketioObject;
	teSocket socketio;

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		menu.transform.position = Vector3.zero;
		audioSource = GetComponent<AudioSource> ();

		croudSource = croud.GetComponent<AudioSource> ();
		socketio = socketioObject.GetComponent<teSocket>();

	}

	public void retry(){
		Application.LoadLevel(Application.loadedLevel);
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if (Input.GetKeyDown (KeyCode.W)) {
						
			audioSource.time = 250;
		}
		*/
			
		if (Input.GetKeyDown (KeyCode.Escape)) {
						Application.LoadLevel (Application.loadedLevel);
						socketio.disconnect();
				}

		if (!skipped && startedSong){
				if (!audioSource.isPlaying && Input.GetKeyDown (KeyCode.Space)) {
						StopCoroutine("startSongCoroutine");
						StartCoroutine("skipCoroutine");
						skipped = true;
				}
		}
	}

	public void quitGame(){
		Application.Quit();
	}

	public void startSong(){
		online = false;
		GameObject.Find ("songManager 1st").SendMessage("goOffline");
		GameObject.Find ("songManager 2nd").SendMessage("goOffline");
		startedSong = true;
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
				StartCoroutine ("startSongCoroutineOnline");
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
		//yield return new WaitForSeconds(2);
		yield return StartCoroutine ("conversation");
		if(menu != null)menu.SetActive(false);
		jackson.SendMessage ("startDancing");
		if (songController != null && songController.GetComponent<songManager> () != null) {
						audioSource.Play ();
			croudSource.Stop();
		}
	}

	IEnumerator skipCoroutine(){
		textP1.GetComponent<Text> ().text = "`LET'S DO THIS!´";
		textP2.GetComponent<Text> ().text = "¡A mover el exoesqueleto!";
		yield return new WaitForSeconds(2);
		textP1.GetComponent<Text> ().text = "";
		textP2.GetComponent<Text> ().text = "";
		if(menu != null)menu.SetActive(false);
		jackson.SendMessage ("startDancing");
		if (songController != null && songController.GetComponent<songManager> () != null) {
						audioSource.Play ();
			croudSource.Stop();
				}
	}
	
	IEnumerator startSongCoroutineOnline(){
		float posy = waitOnline.transform.position.y;
		while(posy < 10){
			yield return new WaitForSeconds(0.01f);
			posy += 15f*Time.deltaTime;
			waitOnline.transform.position = new Vector3(waitOnline.transform.position.x, posy, waitOnline.transform.position.z);
		}
		yield return new WaitForSeconds(2);
		if(waitOnline != null)waitOnline.SetActive(false);
		jackson.SendMessage ("startDancing");
		if (songController != null && songController.GetComponent<songManager> () != null) {
						audioSource.Play ();
			croudSource.Stop();
				}
	}

	IEnumerator conversation(){
		Text p1 = textP1.GetComponent<Text> ();
		Text p2 = textP2.GetComponent<Text> ();
		yield return new WaitForSeconds(1);
		p1.text = "¡Vaya! ¡Esto está a tope de power!";
		yield return new WaitForSeconds(3);
		p1.text = "";
		p2.text = "Jejejeje... ¡Maikel Yakson, ya eres mío!";
		yield return new WaitForSeconds(3);
		p2.text = "";
		yield return new WaitForSeconds(1f);
		p1.text = "...";
		p2.text = "...";
		yield return new WaitForSeconds(2);
		p1.text = "¿¡WOOOOT DEEEE FOOOOOOK!?";
		p2.text = "¡¿Qué #$·%## haces tú aqui?!";
		yield return new WaitForSeconds(3);
		p2.text = "";
		p1.text = "¿Yo? Yo he venido a... a algo... esto... ";
		yield return new WaitForSeconds(3);
		p1.text = "¡Ah si! He venido a ver a la estrella del pop y mi idolo personal... Maikel Yakson";
		yield return new WaitForSeconds(3.5f);
		p1.text = "Pero... ¿tú quién eres?";
		yield return new WaitForSeconds(3);
		p1.text = "";
		p2.text = "¡Já! Yo soy el Gran e Insuperable Emilio Rodrígues";
		yield return new WaitForSeconds(3);
		p2.text = "Vengo aquí a restaurar el equilibrio en el mundo...";
		yield return new WaitForSeconds(3.5f);
		p2.text = "A reparar los bugs habidos y por haber en las distintas lineas temporales";
		yield return new WaitForSeconds(3);
		p2.text = "¡Vengo a devolverle a Yakson su negrura! HAHAHAHA!";
		yield return new WaitForSeconds(3);
		p2.text = "";
		p1.text = "Qué... No... ¡¡No puedes hacer eso!!";
		yield return new WaitForSeconds(3);
		p1.text = "Si haces eso... Thriller nunca existirá! ¡Ni yo podré ir a este concierto nunca más!";
		yield return new WaitForSeconds(4);
		p1.text = "¡Jamás lo permitiré!";
		yield return new WaitForSeconds(2);
		p1.text = "";
		p2.text = "Hahaha... ¿Y cómo me lo vas a impedir?";
		yield return new WaitForSeconds(3);
		p1.text = "Uhh... esto... ¡Te reto a un duelo de baile!";
		p2.text = "";
		yield return new WaitForSeconds(2);
		p1.text = "";
		p2.text = "...";
		yield return new WaitForSeconds(1.5f);
		p2.text = "Vale... pero con una regla: si ganas tú, me iré para siempre y te dejaré en paz";
		yield return new WaitForSeconds(3.5f);
		p2.text = "";
		p1.text = "???";
		yield return new WaitForSeconds(1f);
		p1.text = "¡¡HECHO!!";
		yield return new WaitForSeconds(1f);
		p2.text = "Pues entonces...";
		p1.text = "";
		yield return new WaitForSeconds(2f);
		p1.text = "`LET'S DO THIS!´";
		p2.text = "¡A mover el exoesqueleto!";

		
		
		
		yield return new WaitForSeconds(3);

		p1.text = "";
		p2.text = "";

	}
	
}
