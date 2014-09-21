using UnityEngine;
using System.Collections;

public class JacksonDance : MonoBehaviour {

	public bool onStart = false;
	bool playing = true;
	Animator animator;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator> ();

		if (onStart) {
			StartCoroutine ("dance");
			animator.SetTrigger("start");
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	IEnumerator dance(){
		while (playing) {
			float seconds = Random.Range (1, 6);
			yield return new WaitForSeconds (seconds);
			int action = (int)(Random.Range(0,2.99f));
			switch(action){
			case 0:
				animator.SetTrigger("woah1");
				break;
			case 1:
				animator.SetBool("change", true);
				animator.SetBool("change2", false);
				break;
			case 2:
				animator.SetBool("change2", true);
				animator.SetBool("change", false);
				break;
			case 3:
				//animator.SetBool("change2", false);
				//animator.SetBool("change", false);
				//animator.SetTrigger("relax");
				break;
			}
		}
	}
}
