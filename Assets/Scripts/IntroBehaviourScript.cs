using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneFadeInOut fader = GameObject.Find ("screenFader").GetComponent<SceneFadeInOut> ();
		fader.EndScene ();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit"))
		{
			//SceneManager.LoadScene("instructionScene", LoadSceneMode.Single);
		}
	}
}
