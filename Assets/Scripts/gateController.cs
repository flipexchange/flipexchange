using UnityEngine;
using System.Collections;

public class gateController : MonoBehaviour {

	public GameObject Key;
	private AudioSource deathAudioSource;
	public AudioClip deathAudio;

	// Use this for initialization
	void Awake(){
		deathAudioSource = GetComponent<AudioSource> ();
		deathAudioSource.clip = deathAudio;
	} 
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void bossDeath()
	{
		deathAudioSource.Play ();
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject==Key){			
			Debug.Log ("Collision detected");
			StartCoroutine("OpenGate");
			Destroy(Key);
		}
	}

	IEnumerator OpenGate()
	{
		// Move the Gate object by y
		var endPosition = gameObject.transform.position.y+3;
		while (gameObject.transform.position.y< endPosition) {
			Vector3 newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+0.05f, gameObject.transform.position.z);
			gameObject.transform.position = newPosition;
			yield return new WaitForSeconds (0.03f);
		}
	}


}
