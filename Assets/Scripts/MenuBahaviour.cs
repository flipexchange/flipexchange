using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBahaviour : MonoBehaviour {
	private GameObject bg;
	private GameObject bg2;
	public void ClickPlay(){
		SceneManager.LoadScene ("introScene", LoadSceneMode.Single);
	}

	public void ClickCredits(){
		SceneManager.LoadScene ("Credits", LoadSceneMode.Single);
	}

	public void ClickMenu(){
		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {
		bg = GameObject.Find ("BackgroundQuad");
		bg2 = GameObject.Find ("BackgroundQuad2");
		GameObject playBut = GameObject.Find ("PlayButton");
		Image butAlpha = playBut.GetComponent<Image> ();
		Color c = butAlpha.color;
		c.a = 0.8f;
		butAlpha.color = c;
		Vector3 newPos = new Vector3(-16.46f, bg.transform.position.y, bg.transform.position.z);
		bg.transform.position = newPos;
		Vector3 newPos2 = new Vector3(33.15f, bg2.transform.position.y, bg2.transform.position.z);
		bg2.transform.position = newPos2;
	}
	
	// Update is called once per frame
	void Update () {
		//Create a nice scrolling animation
		Vector3 newPos = new Vector3(bg.transform.position.x-0.01f, bg.transform.position.y, bg.transform.position.z);
		bg.transform.position = newPos;
		Vector3 newPos2 = new Vector3(bg2.transform.position.x-0.01f, bg2.transform.position.y, bg2.transform.position.z);
		bg2.transform.position = newPos2;
		if (bg2.transform.position.x <= -18.1) {
			newPos = new Vector3(18.4f, bg.transform.position.y, bg.transform.position.z);
			bg.transform.position = newPos;
			newPos2 = new Vector3(68.2f, bg2.transform.position.y, bg2.transform.position.z);
			bg2.transform.position = newPos2;
		}
	} 
}
