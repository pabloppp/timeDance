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

	public int scoreOK = 500;
	public int scoreGOOD = 1000;
	public int scorePERFECT = 2000;
	public int scoreMISS = 5000;
	public int scoreWRONG = 1000;

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

	private GameObject[] arrows = new GameObject[4];

	public GameObject pointerA;

	public TextAsset songData;

	AudioSource audioSource;

	bool pressLeft = false, 
	pressUp = false, 
	pressDown = false, 
	pressRight = false;

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
		string[] lines = songData.text.Split('\n');
		foreach (string s in lines) {
			if(s.Length > 0){
				Debug.Log (s);
				string[] data = s.Split(',');
				int time = (int)(int.Parse(data[0])+songStartDataDelay);
				notesPreSpawn.Add(time+","+data[1]);
			}
		}
		arrows [0] = arrowLEFT;
		arrows [1] = arrowUP;
		arrows [2] = arrowDOWN;
		arrows [3] = arrowRIGHT;



		spawnPreTime = 1000 * zArrowSpawn / zArrowSpeed;

		audioSource = GetComponent<AudioSource> ();

		ok.enabled = false;
		good.enabled = false;
		perfect.enabled = false;
		missed.enabled = false;
		wrong.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {

		if(!audioSource.isPlaying && Time.time*1000 > songStartDelay){
			audioSource.Play ();
		}

		if (audioSource.isPlaying)
						currentTime = audioSource.time * 1000; // in milliseconds
		else
						currentTime = Time.time;
		//Debug.Log (currentTime);

		if(notesPreSpawn.Count > 0){
			string[] data = notesPreSpawn[0].Split(',');
			arrowSpawner(data);
		}


		for(int i = notesPostSpawn.Count-1; i >= 0; i--) arrowCheck(i);

		checkKeyPress ();

	}

	void arrowSpawner(string[] data){
		int offset = int.Parse(data[0]);
		if (currentTime >= offset - spawnPreTime) {
			string note = notesPreSpawn[0];
			List<GameObject> noteObjects = new List<GameObject>();
			for(int i = 1; i<data.Length; i++){
				int direction = int.Parse(data[i])-1;
				GameObject newObject = (GameObject) Instantiate(pointerA, arrows[direction].transform.position+Vector3.down*zArrowSpawn, Quaternion.Euler(0,0,0));
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
		for(int i = 0; i < 4; i++){
			arrows[i].renderer.enabled = true;
		}

		if (currentTime >= offset - beforeInterval && currentTime < offset + afterInterval) {
			for(int i = 1; i<data.Length; i++){
				int direction = int.Parse(data[i])-1;
				arrows[direction].renderer.enabled = false;
				if(direction == 0){
					pressLeft = true;
					if(Input.GetKeyDown(KeyCode.LeftArrow)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
					}

				}
				if(direction == 1){
					pressUp = true;
					if(Input.GetKeyDown(KeyCode.UpArrow)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
					}
				}
				if(direction == 2){
					pressDown = true;
					if(Input.GetKeyDown(KeyCode.DownArrow)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
					}
				}
				if(direction == 3){
					pressRight = true;
					if(Input.GetKeyDown(KeyCode.RightArrow)){
						calculatePoint(offset, currentTime);
						destroySpawned(key, 0, true);
					}
				}
			}
		}
		else if (currentTime > offset + afterInterval) {
			destroySpawned(key, 1000, false);
			score -= scoreMISS;
			StartCoroutine("showText", missed);
			Debug.Log ("MISSED");
		}

	}

	void checkKeyPress(){
		if(!pressLeft && Input.GetKeyDown(KeyCode.LeftArrow)){
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			Debug.Log("-1000 LEFT");

		}
		if(!pressUp && Input.GetKeyDown(KeyCode.UpArrow)){
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			Debug.Log("-1000 UP");
		}
		if(!pressDown && Input.GetKeyDown(KeyCode.DownArrow)){
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			Debug.Log("-1000 DOWN");
		}
		if(!pressRight && Input.GetKeyDown(KeyCode.RightArrow)){
			score -= scoreWRONG;
			StartCoroutine("showText", wrong);
			Debug.Log("-1000 RIGHT");
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
			score += scoreOK;
			StartCoroutine("showText", ok);
			Debug.Log(scoreOK);
		}
		else if((difference < (beforeInterval/3)*2 && difference >= (beforeInterval/3))
		        || (difference > -afterInterval && difference <= -afterInterval/2 )){
			score += scoreGOOD;
			StartCoroutine("showText", good);
			Debug.Log(scoreGOOD);
		}
		else if(difference < beforeInterval/3 && difference >= 0
		        || difference > -afterInterval/2 && difference <= 0){
			score += scorePERFECT;
			StartCoroutine("showText", perfect);
			Debug.Log(scorePERFECT);
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
}
