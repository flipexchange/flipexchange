﻿using UnityEngine;
using System.Collections;

public class SteamController : MonoBehaviour {
	private PlayerControls playerScript;
	private GameObject player;
	private int switchStatus;
	// Use this for initialization
	void Start () {
		switchStatus = 0;//haven't switched yet 
		player = GameObject.Find ("Player");
		if (player != null) {
			playerScript = player.GetComponent<PlayerControls> ();
		}
	}

	// Update is called once per frame
	void Update () {
		if (playerScript.pink && switchStatus != 1) {
			EllipsoidParticleEmitter emitter = GetComponent<EllipsoidParticleEmitter> ();
			emitter.ClearParticles ();
			emitter.transform.position = new Vector3 (5.59f, -9, 0);
			emitter.localVelocity = new Vector3 (0, 1.5f, 0);
			switchStatus = 1;
		} else if(playerScript.pink == false && switchStatus != 2){
			EllipsoidParticleEmitter emitter = GetComponent<EllipsoidParticleEmitter> ();
			emitter.ClearParticles ();
			emitter.transform.position = new Vector3 (5.59f, 8, 0);
			emitter.localVelocity = new Vector3 (0, -1.5f, 0);
			switchStatus = 2;
		}
	}

}
