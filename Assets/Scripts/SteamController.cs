using UnityEngine;
using System.Collections;

public class SteamController : MonoBehaviour {
	private PlayerControls playerScript;
	private GameObject player;
	private int switchStatus;
	ParticleEmitter emitter;
	// Use this for initialization
	void Start () {
		switchStatus = 0;//haven't switched yet 
		player = GameObject.Find ("Player");
		emitter = GetComponent<ParticleEmitter>();
		if (player != null) {
			playerScript = player.GetComponent<PlayerControls> ();
		}
	
	}

	// Update is called once per frame
	void Update () {
		if (playerScript.pink && switchStatus != 1) {
			EllipsoidParticleEmitter emitter = GetComponent<EllipsoidParticleEmitter> ();
			switchParticles ();
			emitter.transform.position = new Vector3 (5.59f, -9, 0);
			emitter.worldVelocity = new Vector3 (0, 1.0f, 0);
			switchStatus = 1;
		} else if(playerScript.pink == false && switchStatus != 2){
			EllipsoidParticleEmitter emitter = GetComponent<EllipsoidParticleEmitter> ();
			switchParticles ();
			emitter.transform.position = new Vector3 (5.59f, 9, 0);
			emitter.worldVelocity = new Vector3 (0, -1.0f, 0);
			switchStatus = 2;

		}
	}
	void switchParticles()
	{
		Particle[] particles = emitter.particles;
		int i = 0;
		while (i < particles.Length) {
			float yPosition = particles [i].position.y * -1;
			particles [i].position = new Vector3 (particles [i].position.x, yPosition, particles [i].position.z);
			particles [i].velocity *= -1;
			i++;
		}
		emitter.particles = particles;
	}

}
