using UnityEngine;
using System.Collections;

public class myMenuManager : MonoBehaviour {

	public GameObject songController;
	public GameObject menu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void quitGame(){
		Application.Quit();
	}

	public void startSong(){
		StartCoroutine("startSongCoroutine");
	}

	IEnumerator startSongCoroutine(){
		float posy = menu.transform.position.y;
		while(posy < 10){
			yield return new WaitForSeconds(0.01f);
			posy += 15f*Time.deltaTime;
			menu.transform.position = new Vector3(menu.transform.position.x, posy, menu.transform.position.z);
		}
		yield return new WaitForSeconds(2);
		if(menu != null)menu.SetActive(false);
		if(songController != null && songController.GetComponent<songManager>() != null)
			songController.GetComponent<songManager>().turnSongOn();

	}
}
