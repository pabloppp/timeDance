using UnityEngine;
using System.Collections;

public class animatePlayer : MonoBehaviour {

	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void leftAnimate(){
		animator.SetTrigger ("left");
	}
	void rightAnimate(){
		animator.SetTrigger ("right");
	}
	void upAnimate(){
		animator.SetTrigger ("up");
	}
	void downAnimate(){
		animator.SetTrigger ("down");
	}

	void danceAnimate(){
		animator.SetBool("dance", true);
	}
}
