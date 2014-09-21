using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.UI;

public class songManager : MonoBehaviour {

	// Use this for initialization
	private List<string> notesPreSpawn = new List<string>();
	private List<string> notesPostSpawn = new List<string>();
	private List<List<GameObject>> spawnedNotes = new List<List<GameObject>>();
	public float beforeInterval = 1000;
	public float afterInterval = 500;
	public float zArrowSpawn = 3;
	public float zArrowSpeed = 1.5f; //in units/second
	private float currentTime = 0;
	private float spawnPreTime = 0;
	public float songStartDelay = 0;
	public float songStartDataDelay = -200;
	public int score = 0;
	private int contadorCombo = 0;
	public int iniciaCombo = 10;
	private bool comboOn = false;

	public int scoreOK = 500;
	public int scoreOKCombo = 500;
	public int scoreGOOD = 1000;
	public int scoreGOODCombo = 1000;
	public int scorePERFECT = 2000;
	public int scorePERFECTCombo = 2000;
	public int scoreMISS = 5000;
	public int scoreWRONG = 1000;

	public KeyCode UpKey = KeyCode.UpArrow;
	public KeyCode DownKey = KeyCode.DownArrow;
	public KeyCode LeftKey = KeyCode.LeftArrow;
	public KeyCode RightKey = KeyCode.RightArrow;

	//Note Objects
	
	public GameObject arrowUP;
	public GameObject arrowDOWN;
	public GameObject arrowLEFT;
	public GameObject arrowRIGHT;

	public Text ok;
	public Text good;
	public Text perfect;
	public Text missed;
	public Text wrong;
	public Text combo_10;

	private GameObject[] arrows = new GameObject[4];
	private GameObject[] arrowsMove = new GameObject[4];

	public GameObject arrowUP_Moves;
	public GameObject arrowDOWN_Moves;
	public GameObject arrowLEFT_Moves;
	public GameObject arrowRIGHT_Moves;

	public TextAsset songData;

	public AudioSource audioSource;

	bool pressLeft = false, 
	pressUp = false, 
	pressDown = false, 
	pressRight = false,
	startedDance = false;

	public GameObject player;

	public GameObject socketioObject;
	teSocket socketio;

	public bool isPlayer = true;
	bool multiplayer = true;

	void Start () {	

		//Random generator -- TO AVOID

		/*
		for(int i=0; i<50; i++){
			string s = ((int)(Random.Range((i+1)*0.5f-0.2f, (i+1)*0.5f+0.2f))*1000+2000)+","+(int)Random.Range(1,4.999f);
			notesPreSpawn.Add(s);
			Debug.Log(s);
		}
		*/
		/*
		int start = 2000;
		int[][] s = {
			new int[]{56, 1}, 
			new int[]{56, 4}, 
			new int[]{136, 2},
			new int[]{304, 3},
			new int[]{384, 2},
			new int[]{272, 2},
			new int[]{184, 1},
			new int[]{56, 4},
		};
		*/

		socketio = socketioObject.GetComponent<teSocket> ();

		string[] lines = songData.text.Split('\n');
		foreach (string s in lines) {
			if(s.Length > 0){
				//Debug.Log (s);
				string[] data = s.Split(',');
				int time = 0;
				if(int.TryParse(data[0], out time)){
					time += (int)songStartDataDelay;
					//int time = (int)(int.Parse(data[0])+songStartDataDelay);
					notesPreSpawn.Add(time+","+data[1]);
				}
				else{
					notesPreSpawn.Add(data[0]+","+data[1]);
				}
			}
		}
		arrows [0] = arrowLEFT;
		arrows [1] = arrowUP;
		arrows [2] = arrowDOWN;
		arrows [3] = arrowRIGHT;


		arrowsMove [0] = arrowLEFT_Moves;
		arrowsMove [1] = arrowUP_Moves;
		arrowsMove [2] = arrowDOWN_Moves;
		arrowsMove [3] = arrowRIGHT_Moves;


		spawnPreTime = 1000 * zArrowSpawn / zArrowSpeed;

		audioSource = GetComponent<AudioSource> ();

		ok.enabled = false;
		good.enabled = false;
		perfect.enabled = false;
		missed.enabled = false;
		wrong.enabled = false;
		combo_10.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		spawnPreTime = 1000 * zArrowSpawn / zArrowSpeed;	

		if(!audioSource.isPlaying) return;

		if (audioSource.isPlaying) currentTime = audioSource.time * 1000; // in milliseconds

		if(audioSource.isPlaying && !startedDance){
			player.SendMessage("danceAnimate");
			startedDance = true;
		}


		if(notesPreSpawn.Count > 0){
			string[] data = notesPreSpawn[0].Split(',');
			arrowSpawner(data);
		}


		for(int i = notesPostSpawn.Count-1; i >= 0; i--) arrowCheck(i);

		checkKeyPress ();
		checkCombo();

		
		
	}

	public void turnSongOn(){
		if(!audioSource.isPlaying && Time.time*1000 > songStartDelay){
			audioSource.Play ();
		}
	}

	void checkCombo(){
		if(contadorCombo >= iniciaCombo){
			comboOn = true;
		}
		else{
			comboOn = false;
		}
	}

	void arrowSpawner(string[] data){
		if (data [0] == "s") {
			zArrowSpeed = int.Parse(data[1]);
			notesPreSpawn.RemoveAt(0);
			return;
		}
		int offset = int.Parse(data[0]);
		if (currentTime >= offset - spawnPreTime) {
			string note = notesPreSpawn[0];
			List<GameObject> noteObjects = new List<GameObject>();
			for(int i = 1; i<data.Length; i++){
				int direction = int.Parse(data[i])-1;
				GameObject newObject = (GameObject) Instantiate(arrowsMove[direction], arrows[direction].transform.position+Vector3.down*zArrowSpawn, Quaternion.Euler(0,0,0));
				moveUp moveScript = (moveUp) newObject.GetComponent<moveUp>();
				moveScript.setSpeed(zArrowSpeed);
				noteObjects.Add(newObject);
			}
			spawnedNotes.Add(noteObjects);
			notesPostSpawn.Add(note);
			notesPreSpawn.RemoveAt(0);
		}
	}

	void arrowCheck(int key)	{

		string[] data = notesPostSpawn[key].Split(',');
		int offset = int.Parse(data[0]);


		if (currentTime >= offset - beforeInterval && currentTime < offset + afterInterval) {
			for(int i = 1; i<data.Length; i++){
				int direction = int.Parse(data[i])-1;

				if(direction == 0){
					pressLeft = true;
					if(Input.GetKeyDown(LeftKey)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
						player.SendMessage("leftAnimate");
						if(isPlayer && multiplayer) socketio.emit("keyPressed","0");
					}

				}
				if(direction == 1){
					pressUp = true;
					if(Input.GetKeyDown(UpKey)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
						player.SendMessage("upAnimate");
						if(isPlayer && multiplayer) socketio.emit("keyPressed","1");
					}
				}
				if(direction == 2){
					pressDown = true;
					if(Input.GetKeyDown(DownKey)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
						player.SendMessage("downAnimate");
						if(isPlayer && multiplayer) socketio.emit("keyPressed","2");
					}
				}
				if(direction == 3){
					pressRight = true;
					if(Input.GetKeyDown(RightKey)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
						player.SendMessage("rightAnimate");
						if(isPlayer && multiplayer) socketio.emit("keyPressed","3");
					}
				}
			}
		}
		else if (currentTime > offset + afterInterval) {
			destroySpawned(key, 1000, false);
			contadorCombo = 0;
			score -= scoreMISS;
			StartCoroutine("showText", missed);
		}

	}

	void checkKeyPress(){
		if(!pressLeft && Input.GetKeyDown(LeftKey)){
			contadorCombo = 0;
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			arrows[0].SendMessage("error");
			if(isPlayer && multiplayer) socketio.emit("keyPressed","0");

		}
		if(!pressUp && Input.GetKeyDown(UpKey)){
			contadorCombo = 0;
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			arrows[1].SendMessage("error");
			if(isPlayer && multiplayer) socketio.emit("keyPressed","1");
		}
		if(!pressDown && Input.GetKeyDown(DownKey)){
			contadorCombo = 0;
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			arrows[2].SendMessage("error");
			if(isPlayer && multiplayer) socketio.emit("keyPressed","2");
		}
		if(!pressRight && Input.GetKeyDown(RightKey)){
			contadorCombo = 0;
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			arrows[3].SendMessage("error");
			if(isPlayer && multiplayer) socketio.emit("keyPressed","3");
		}
		pressLeft = false;
		pressUp = false;
		pressDown = false;
		pressRight = false;

	}

	void destroySpawned(int key, int time, bool fall){
		notesPostSpawn.RemoveAt(key);
		for(int i = 0; i < spawnedNotes[key].Count; i++){
			spawnedNotes[key][i].GetComponent<moveUp>().kill(time, fall);
		}
		spawnedNotes.RemoveAt(key);
	}

	void calculatePoint(float exact, float hit){
		float difference = exact - hit;
		if(difference < beforeInterval && difference >= (beforeInterval/3)*2){
			contadorCombo++;
			if(comboOn){
				scoreOKCombo= scoreOK*2;
				score += scoreOKCombo;
				StartCoroutine("showText", ok);
				StartCoroutine("showCombo", combo_10);
			}
			else{
				score += scoreOK;				
				StartCoroutine("showText", ok);
			}				
			//Debug.Log("contador: "+contadorCombo);
		}
		else if((difference < (beforeInterval/3)*2 && difference >= (beforeInterval/3))
		        || (difference > -afterInterval && difference <= -afterInterval/2 )){
			contadorCombo++;
			if(comboOn){
				scoreGOODCombo= scoreGOOD*2;
				score += scoreGOODCombo;
				StartCoroutine("showText", good);
				StartCoroutine("showCombo", combo_10);
			}
			else{
				score += scoreGOOD;				
				StartCoroutine("showText", good);
			}				
			//Debug.Log("contador: "+contadorCombo);
		}
		else if(difference < beforeInterval/3 && difference >= 0
		        || difference > -afterInterval/2 && difference <= 0){
			contadorCombo++;
			if(comboOn){
				scorePERFECTCombo= scorePERFECT*2;
				score += scorePERFECTCombo;
				StartCoroutine("showText", perfect);
				StartCoroutine("showCombo", combo_10);
			}
			else{
				score += scorePERFECT;				
				StartCoroutine("showText", perfect);
			}				
			//Debug.Log("contador: "+contadorCombo);
		}

	}

	IEnumerator showText(Text t){
		ok.enabled = false;
		good.enabled = false;
		perfect.enabled = false;
		missed.enabled = false;
		wrong.enabled = false;
		t.enabled = true;
		yield return new WaitForSeconds(0.4f);
		t.enabled = false;
	}

	IEnumerator showCombo(Text t){
		combo_10.enabled = false;
		t.enabled = true;
		yield return new WaitForSeconds(0.4f);
		t.enabled = false;
	}
}
