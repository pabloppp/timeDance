using UnityEngine;
using System.Collections;

public class moveUp : MonoBehaviour {

	public float speed = 1.5f;
	int killOffset = -1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.up*speed*Time.deltaTime;
		//if (killOffset > 0) Debug.Log (killOffset-Time.time*1000);
		if (killOffset > 0 && Time.time*1000 > killOffset)
			Destroy (this.gameObject);
	}

	public void setSpeed(float newSpeed){
		speed = newSpeed;
		//Debug.Log ("NEW SPEED: " + speed);
	}

	public void kill(int offset, bool fall){
		//Debug.Log ("kill in " + offset);
		killOffset = (int)(Time.time * 1000 + offset);
	}
}
