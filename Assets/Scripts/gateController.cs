using UnityEngine;
using System.Collections;

public class gateController : MonoBehaviour {

	public GameObject Key;
	private AudioSource deathAudioSource;
	public AudioClip deathAudio;
	private AudioSource prebattleAudioSource;
	public AudioClip prebattleAudio;
	public AudioClip battleAudio;
	private GameObject player;
	private bool playingBossSound;
	private AudioSource audio1;
	private AudioSource audio2;
	private bool playBossNow = false;



	// Use this for initialization
	void Awake(){
		deathAudioSource = GetComponent<AudioSource> ();
		deathAudioSource.clip = deathAudio;
		prebattleAudioSource = gameObject.AddComponent<AudioSource>();
		prebattleAudioSource.clip = prebattleAudio;
		prebattleAudioSource.loop = true;
	} 
	void Start () {
		//audio1 = gameObject.AddComponent(AudioSource);
		//audio1.clip = prebattleAudio;
		//audio2 = gameObject.AddComponent(AudioSource);
		//audio2.clip = battleAudio;
		prebattleAudioSource.Play ();
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.x > 23) {
			if (playingBossSound == false && !playBossNow) {
				if (prebattleAudioSource.volume > 0.01f) {
					prebattleAudioSource.volume -= 0.005f;
				} else {
					prebattleAudioSource.volume= 1;
					playBossNow = true;
					playingBossSound = true;
					prebattleAudioSource.clip = battleAudio;
					prebattleAudioSource.Play ();
				}

			}
		}
	}
	public void bossDeath()
	{
		prebattleAudioSource.clip = prebattleAudio;
		prebattleAudioSource.Play ();
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
