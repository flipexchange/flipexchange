﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InstructionsBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown("Submit"))
		{
			Debug.Log ("Load portal scene");
			SceneManager.LoadScene("DylanLevel", LoadSceneMode.Single);
		}
	}
}
