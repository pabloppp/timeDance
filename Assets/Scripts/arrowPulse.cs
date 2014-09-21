using UnityEngine;
using System.Collections;

public class arrowPulse : MonoBehaviour {


	float MSPB = 431.654676f;
	float BPS = 0;
	bool successBool = false;
	float shakeAmmount = 0.1f;
	Vector3 originalPosition;
	public Material successMaterial;
	Material mainMaterial;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		BPS = 1000/MSPB;
		mainMaterial = this.GetComponent<SpriteRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if(!successBool){
			float opacity = Mathf.Abs(Mathf.Cos ((Time.time+0.5f)*Mathf.PI*BPS));
			//Debug.Log ("opacity " + opacity);
			if(opacity < 0.8f) opacity = 0.3f;
			else if(opacity >= 0.8f) opacity = ((opacity-0.8f)/0.2f)*0.7f+0.3f;
			this.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, opacity);
		}
		else{
			this.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 1f);
		}
	}

	void success(){
		StartCoroutine ("doSuccess");
	}

	void error(){
		StartCoroutine ("doError");
	}

	IEnumerator doSuccess(){
		successBool = true;
		this.GetComponent<SpriteRenderer> ().material = successMaterial;
		yield return new WaitForSeconds (0.2f);
		successBool = false;
		this.GetComponent<SpriteRenderer> ().material = mainMaterial;
	}

	IEnumerator doError(){
		for(int i=0;i<5;i++){
			transform.position = originalPosition + new Vector3(Random.Range(-shakeAmmount,shakeAmmount),Random.Range(-shakeAmmount,shakeAmmount),Random.Range(-shakeAmmount,shakeAmmount));
			yield return new WaitForSeconds (0.05f);
		}
		transform.position = originalPosition;
	}
}
