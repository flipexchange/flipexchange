using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBahaviour : MonoBehaviour {
	private GameObject bg;
	private GameObject bg2;
	public float _x;
	public float _y;
	public float _buttonHeight;
	public float _buttonWidth;
	public GUIStyle customButton;

	public void ClickPlay(){
		SceneManager.LoadScene ("introScene", LoadSceneMode.Single);
	}

	public void ClickCredits(){
		SceneManager.LoadScene ("Credits", LoadSceneMode.Single);
	}

	public void ClickMenu(){
		SceneManager.LoadScene ("MainMenu", LoadSceneMode.Single);
	}

	public void ClickQuit(){
		Application.Quit();
	}

	// Use this for initialization
	void Start () {
		bg = GameObject.Find ("BackgroundQuad");
		bg2 = GameObject.Find ("BackgroundQuad2");

		Vector3 newPos = new Vector3(-16.46f, bg.transform.position.y, bg.transform.position.z);
		bg.transform.position = newPos;
		Vector3 newPos2 = new Vector3(33.15f, bg2.transform.position.y, bg2.transform.position.z);
		bg2.transform.position = newPos2;

		// Set main menu position
		_x = (Screen.height-2*_buttonHeight) / 2 +50;
		_y = 30;
		_buttonHeight = 60;
		_buttonWidth = 300;

		// Apply faster animation speed to all Buttons on Canvas
		Button[] buttons = this.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].GetComponent<Animator> ().speed = .5f;	
		}
	}	

	// Update is called once per frame
	void Update () {
		//Create a nice scrolling animation
		Vector3 newPos = new Vector3 (bg.transform.position.x - 0.01f, bg.transform.position.y, bg.transform.position.z);
		bg.transform.position = newPos;
		Vector3 newPos2 = new Vector3 (bg2.transform.position.x - 0.01f, bg2.transform.position.y, bg2.transform.position.z);
		bg2.transform.position = newPos2;
		if (bg2.transform.position.x <= -18.1) {
			newPos = new Vector3 (18.4f, bg.transform.position.y, bg.transform.position.z);
			bg.transform.position = newPos;
			newPos2 = new Vector3 (68.2f, bg2.transform.position.y, bg2.transform.position.z);
			bg2.transform.position = newPos2;
		}
	} 

//  Please don't remove this just yet... 
//	void OnGUI () {
//
//		// Make the first button. If it is pressed, Play Game will be executed
//		if(GUI.Button(new Rect(_x,_y,_buttonWidth,_buttonHeight), "Play Game", customButton)) {
//			ClickPlay ();
//		}
//
//		// Make the second button.
//		if(GUI.Button(new Rect(_x,_y+_buttonHeight+10,_buttonWidth,_buttonHeight), "Credits", customButton)) {
//			ClickCredits();
//		}
//
//		// Make the third button.
//		if(GUI.Button(new Rect(_x,_y+2*_buttonHeight+20,_buttonWidth,_buttonHeight), "Quit", customButton)) {
//			Application.Quit();
//		}
//	}
}


