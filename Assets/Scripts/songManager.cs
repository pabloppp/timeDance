using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

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

	//Note Objects
	
	public GameObject arrowUP;
	public GameObject arrowDOWN;
	public GameObject arrowLEFT;
	public GameObject arrowRIGHT;

	private GameObject[] arrows = new GameObject[4];
	private GameObject[] arrowsMove = new GameObject[4];

	public GameObject arrowUP_Moves;
	public GameObject arrowDOWN_Moves;
	public GameObject arrowLEFT_Moves;
	public GameObject arrowRIGHT_Moves;

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

	}
	
	// Update is called once per frame
	void Update () {

		spawnPreTime = 1000 * zArrowSpawn / zArrowSpeed;	

		if(!audioSource.isPlaying && Time.time*1000 > songStartDelay){
			audioSource.Play ();
		}

		if (audioSource.isPlaying)
						currentTime = audioSource.time * 1000; // in milliseconds
		//Debug.Log (currentTime);

		if(notesPreSpawn.Count > 0){
			string[] data = notesPreSpawn[0].Split(',');
			arrowSpawner(data);
		}


		for(int i = notesPostSpawn.Count-1; i >= 0; i--) arrowCheck(i);

		checkKeyPress ();

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
		/*for(int i = 0; i < 4; i++){
			arrows[i].renderer.enabled = true;
		}*/

		if (currentTime >= offset - beforeInterval && currentTime < offset + afterInterval) {
			for(int i = 1; i<data.Length; i++){
				int direction = int.Parse(data[i])-1;
				//arrows[direction].renderer.enabled = false;
				if(direction == 0){
					pressLeft = true;
					if(Input.GetKeyDown(KeyCode.LeftArrow)){
						Debug.Log("+1000 LEFT");
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
					}

				}
				if(direction == 1){
					pressUp = true;
					if(Input.GetKeyDown(KeyCode.UpArrow)){
						Debug.Log("+1000 UP");
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
					}
				}
				if(direction == 2){
					pressDown = true;
					if(Input.GetKeyDown(KeyCode.DownArrow)){
						Debug.Log("+1000 DOWN");
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
					}
				}
				if(direction == 3){
					pressRight = true;
					if(Input.GetKeyDown(KeyCode.RightArrow)){
						Debug.Log("+1000 RIGHT");
						destroySpawned(key, 0, true);
						arrows[direction].SendMessage("success");
					}
				}
			}
		}
		else if (currentTime > offset + afterInterval) {
			destroySpawned(key, 1000, false);
			Debug.Log ("MISSED");
		}

	}

	void checkKeyPress(){
		if(!pressLeft && Input.GetKeyDown(KeyCode.LeftArrow)){
			Debug.Log("-1000 LEFT");
			arrows[0].SendMessage("error");

		}
		if(!pressUp && Input.GetKeyDown(KeyCode.UpArrow)){
			Debug.Log("-1000 UP");
			arrows[1].SendMessage("error");
		}
		if(!pressDown && Input.GetKeyDown(KeyCode.DownArrow)){
			Debug.Log("-1000 DOWN");
			arrows[2].SendMessage("error");
		}
		if(!pressRight && Input.GetKeyDown(KeyCode.RightArrow)){
			Debug.Log("-1000 RIGHT");
			arrows[3].SendMessage("error");
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

}
