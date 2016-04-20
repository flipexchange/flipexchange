using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	string sceneName;
	// Use this for initialization
	void Start () {
		Scene scene = SceneManager.GetActiveScene();
		Debug.Log (scene.name);
		sceneName = scene.name;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit"))
		{
			Debug.Log (sceneName);
			switch (sceneName)
			{
			case "cutscene1":
				SceneManager.LoadScene("SecondLevel", LoadSceneMode.Single);
				break;
			default:
				SceneManager.LoadScene("introScene", LoadSceneMode.Single);
				break;
			}
		}
	}
}
