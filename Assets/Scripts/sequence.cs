using UnityEngine;
using System.Collections;

public class sequence : MonoBehaviour {

	public Texture2D[] shots;
	private Material material;
	private Texture mainTexture;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("Play"); 

	}

	IEnumerator Play(){  
		for(int i=0; i<shots.Length; i++){
			renderer.material.mainTexture = shots[1];
			yield return new WaitForSeconds(0.5f);
		}
		StopCoroutine("Play");   
	}   
}
