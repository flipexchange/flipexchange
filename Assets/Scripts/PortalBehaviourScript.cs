using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PortalBehaviourScript : MonoBehaviour {

	public int sceneIndex;
	public Text statusText;
	private string[] sceneNames;
	// Make the object float up and down
	// http://answers.unity3d.com/questions/59934/how-to-an-object-floating-up-and-down.html
	public float amplitude;          //Set in Inspector 
	public float speed;                  //Set in Inspector 
	private float tempVal;
	private Vector3 tempPos;

	// Use this for initialization
	void Start () {
		sceneNames = new string[5];
		sceneNames[0] = "DylanLevel";
		sceneNames[1] = "cutscene1";
		sceneNames[2] = "SecondLevel";
		sceneNames[3] = "ThirdLevel";
		sceneNames[4] = "cutscene2";
		statusText.text = "";
		tempVal = transform.position.y-.5f;
		tempPos.x = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		// Make the portal bounce up and down
		tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
		transform.position = tempPos;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log ("Collision detected");
		StartCoroutine("LoadNextLevel");
		if (sceneIndex == 3) {
			statusText.text = "Loading end scene...";
		} else {
			statusText.text = "Loading next level...";
		}
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene(sceneNames[sceneIndex + 1], LoadSceneMode.Single);
	}

}
