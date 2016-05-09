using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	string sceneName;
	// Use this for initialization
	void Start () {
		Scene scene = SceneManager.GetActiveScene();
		sceneName = scene.name;
	}

	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit"))
		{
			switch (sceneName)
			{
				case "MainMenu":
					SceneManager.LoadScene("introScene", LoadSceneMode.Single);
					break;
				case "cutscene1":
					//SceneManager.LoadScene("SecondLevel", LoadSceneMode.Single);
					break;
				case "cutscene2":
					//SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
					break;
				case "Credits":
					SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
					break;
			}
		}
	}
}
