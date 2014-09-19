using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class songManager : MonoBehaviour {

	// Use this for initialization
	private List<string> notes = new List<string>();
	private List<int> offsetsPassed = new List<int>();
	private bool playIt = false;
	public float interval = 200;
	public float intervalPointer = 3000;
	private float timePassed = 0;

	//Note Objects
	public GameObject objectA;
	public GameObject objectB;
	public GameObject objectC;
	public GameObject objectD;
	public GameObject pointerA;

	void Start () {	
		for(int i=0; i<20; i++){
			string s = (int)(Random.Range((i+1)*1-0.5f, (i+1)*1+0.5f))*1000+","+(int)Random.Range(1,4.999f);
			notes.Add(s);
			Debug.Log(s);
		}
	}
	
	// Update is called once per frame
	void Update () {
		timePassed += Time.deltaTime;
		bool a = false; bool b = false; bool c = false; bool d = false;
		foreach(string s in notes){
			string[] data = s.Split(',');
			int offset = int.Parse(data[0]);
			int key = int.Parse(data[1]);
			if(timePassed*1000-offset > 0 && timePassed*1000-offset <= interval){
				switch(key){
				case 1: a = true;
					break;
				case 2: b = true;
					break;
				case 3: c = true;
					break;
				case 4: d = true;
					break;				
				}
			}
			if(timePassed*1000-offset > 0 && timePassed*1000-offset <= intervalPointer
			   && !offsetsPassed.Contains(offset)){
				offsetsPassed.Add(offset);
				if(key == 1){
					GameObject newObject = (GameObject) Instantiate(pointerA, objectA.transform.position+Vector3.down*3, 
				                  Quaternion.Euler(0,0,0));
					
				}
				if(key == 2){
					GameObject newObject = (GameObject) Instantiate(pointerA, objectB.transform.position+Vector3.down*3, 
					                                                Quaternion.Euler(0,0,0));					
				}
				if(key == 3){
					GameObject newObject = (GameObject) Instantiate(pointerA, objectC.transform.position+Vector3.down*3, 
					                                                Quaternion.Euler(0,0,0));					
				}
				if(key == 4){
					GameObject newObject = (GameObject) Instantiate(pointerA, objectD.transform.position+Vector3.down*3, 
					                                                Quaternion.Euler(0,0,0));					
				}
			}
			objectA.renderer.enabled = a;
			objectB.renderer.enabled = b;
			objectC.renderer.enabled = c;
			objectD.renderer.enabled = d;
		}		
	}
}
