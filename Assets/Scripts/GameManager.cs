using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int sceneIndex;
	private int nextSceneIndex;
	private string[] sceneNames;

	// Use this for initialization
	void Start () {
		sceneNames = new string[2];
		sceneNames[0] = "cutScene";
		sceneNames[1] = "cutScene2";
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit"))
		{
			nextSceneIndex = sceneIndex + 1;
			Debug.Log (nextSceneIndex);
			SceneManager.LoadScene(sceneNames[nextSceneIndex], LoadSceneMode.Single);
		}
	}
}
