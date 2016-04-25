using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuBahaviour : MonoBehaviour {

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
	
	}
	
	// Update is called once per frame
	void Update () {
	
	} 
}
