using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class pointsUpdate : MonoBehaviour {
	public GameObject songManager;
	songManager songManagerScript;
	Text inicial;
	// Use this for initialization
	void Start () {
		inicial = GetComponent<Text>();
		songManagerScript = songManager.GetComponent<songManager>();
	}
	
	// Update is called once per frame
	void Update () {
		string scoreText = ""+Mathf.Abs(songManagerScript.score);
		int textsize = scoreText.Length;
		for(int i = 0; i<9-textsize;i++){
			scoreText = "0"+scoreText;
		}
		if(songManagerScript.score >= 0) scoreText = "+"+scoreText;
		else scoreText = "-"+scoreText;
		inicial.text = scoreText;
	}
}
