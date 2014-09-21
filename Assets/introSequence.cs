using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class introSequence : MonoBehaviour {

	/*
	private Texture texture;  
	private Material goMaterial;  
	private int frameCounter = 0;
	private string baseName; 

	public string folderName;  
	public string imageSequenceName;  
	public int numberOfFrames;  */
	/*
	void Awake() {  
		this.goMaterial = this.renderer.material;
		this.baseName = this.folderName + "/" + this.imageSequenceName;  
	}  
	*/

	public GameObject slide1;
	public GameObject slide2;
	public GameObject slide3;
	public GameObject slide4;
	public GameObject slide5;
	public GameObject slide6;


	public Text text1Text;
	public Text text2Text;
	public Text text3Text;

	public GameObject text1;
	public GameObject text2;
	public GameObject text3;





	// Use this for initialization
	void Start () {
		slide1.SetActive(false);
		slide2.SetActive(false);
		slide3.SetActive(false);
		slide4.SetActive(false);
		slide5.SetActive(false);
		slide6.SetActive(false);

		text1Text = text1.GetComponent<Text> ();
		text2Text = text2.GetComponent<Text> ();
		text3Text = text3.GetComponent<Text> ();
		text1Text.enabled = false ;
		text2Text.enabled = false ;
		text3Text.enabled = false ;



	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("Play");  
		if (Input.GetKey (KeyCode.Space))
						Time.timeScale = 10;
		else Time.timeScale = 1;
	}


	IEnumerator Play(){  	
		slide1.SetActive(true);
		slide2.SetActive(true);
		slide3.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		text1Text.enabled = true ;
		yield return new WaitForSeconds(5.2f);
		text1Text.transform.position = Vector3.up * 30;
		yield return new WaitForSeconds(0.5f);
		slide4.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		text2Text.enabled = true ;
		yield return new WaitForSeconds(5.5f);
		text2Text.transform.position = Vector3.up * 30;
		yield return new WaitForSeconds(0.5f);
		slide5.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		slide6.SetActive(true);
		yield return new WaitForSeconds(1.5f);
		text3Text.enabled = true ;
		yield return new WaitForSeconds(8.5f);
		Time.timeScale = 1;
		Application.LoadLevel("principal");






	}   
}
