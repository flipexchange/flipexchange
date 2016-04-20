using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalBehaviourScript : MonoBehaviour {

	public int sceneIndex;
	private int nextSceneIndex;
	public Text statusText;
	private string[] sceneNames;

	// Use this for initialization
	void Start () {
		sceneNames = new string[2];
		sceneNames[0] = "DylanLevel";
		sceneNames[1] = "SecondLevel";
		statusText.text = "";
		Debug.Log (sceneIndex);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log ("Collision detected");
		StartCoroutine("LoadNextLevel");
		statusText.text = "Loading next level...";
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds(1.5f);
		nextSceneIndex = sceneIndex + 1;
		Debug.Log (nextSceneIndex);
		SceneManager.LoadScene(sceneNames[nextSceneIndex], LoadSceneMode.Single);
	}

}
