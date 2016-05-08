using UnityEngine;
using System.Collections;

public class bossController : MonoBehaviour {

    /*  PATROL LOGIC  */
    public float distance = 5f;
    public float speed = 1f;
    private float distFromStart;
    Vector3 velocity;
    bool isGoingLeft = true;
    Transform _transform;
    Vector3 _originalPosition;

    /*  BULLET LOGIC  */
    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    public float Bullet_Forward_Force;
    public int firingPeriod = 150;
    private int firingCounter;
	private AudioSource bulletAudioSource;
	public AudioClip bulletAudio;
	private AudioSource hitAudioSource;
	public AudioClip hitAudio;
	private gateController gate;

    /* HEALTHBAR */
    public GameObject healthbar;
    public GameObject healthbarRed;
    private float health;

	/* KEY */
	public GameObject Key; 
	public AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {

		AudioSource newAudio = gameObject.AddComponent<AudioSource>();

		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;

		return newAudio;

	}

	void Awake(){
		bulletAudioSource =AddAudio(bulletAudio, false, false, 1.0f);
		hitAudioSource =AddAudio(hitAudio, false, false, 1.0f);



	} 
    public void Start()
    {
        /*  PATROL LOGIC  */
        _originalPosition = gameObject.transform.position;
        _transform = GetComponent<Transform>();
        velocity = new Vector3(speed, 0, 0);
        _transform.Translate(velocity.x * Time.deltaTime, 0, 0);

        /*  BULLET LOGIC  */
        firingCounter = 0;

        /*  ALPHA RENDERER  */
        Color color = GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        GetComponent<Renderer>().material.SetColor("_Color", color);

        /* HEALTHBAR */
        healthbar.GetComponent<Renderer>().enabled = false;
        healthbarRed.GetComponent<Renderer>().enabled = false;
        health = 2f;

		/* SOUND */
		//source = GetComponent<AudioSource> ();
		gate = GameObject.Find("Gate").GetComponent<gateController>();

		//* IGNORE COLLISIONS */
        GameObject player = GameObject.Find("Player");
		// Let player run through the Boss
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(player.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
		// Let key exist on the Boss 
		Physics2D.IgnoreCollision(Key.GetComponent<CircleCollider2D>(), GetComponent<Collider2D>());
		// Ignore collisions between key and player 
		Physics2D.IgnoreCollision(Key.GetComponent<CircleCollider2D>(), player.GetComponent<BoxCollider2D>());
		Physics2D.IgnoreCollision(Key.GetComponent<CircleCollider2D>(), player.GetComponent<CircleCollider2D>());
    }

    void Update()
    {
        distFromStart = transform.position.x - _originalPosition.x;

        if (isGoingLeft) {
            // If gone too far, switch direction
            if (distFromStart < -distance)
                SwitchDirection();

            _transform.Translate(-velocity.x * Time.deltaTime, 0, 0);
        }
        else {
            // If gone too far, switch direction
            if (distFromStart > distance)
                SwitchDirection();

            _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
        }

        // Firing logic
        firingCounter++;
        if (firingCounter > firingPeriod)
        {
			bulletAudioSource.Play ();
            firingCounter = 0;

            //The Bullet instantiation happens here.
            GameObject Temporary_Bullet_Handler;
            Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
            Physics2D.IgnoreCollision(Temporary_Bullet_Handler.GetComponent<Collider2D>(), GetComponent<Collider2D>());

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody2D Temporary_RigidBody;
            Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody2D>();

            Vector3 playerPos = GameObject.Find("Player").transform.position;
            Vector3 bulletVec = playerPos - transform.position;
            bulletVec.Normalize();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(bulletVec * Bullet_Forward_Force);
            
            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(Temporary_Bullet_Handler, 2.0f);
        }
    }
    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
		// Check if layer is interacable 
		if (col.collider.gameObject.layer == 13) 
        {
            health--;
            if (health == 1) {
				hitAudioSource.Play ();
                healthbar.GetComponent<Renderer>().enabled = true;
                healthbarRed.GetComponent<Renderer>().enabled = true;
            }
            if (health == 0) { 
				gate.bossDeath ();
                gameObject.SetActive(false);
                healthbar.SetActive(false);
                healthbarRed.SetActive(false);
				Key.transform.parent = null; // Remove the Key from the Boss Parent Object
				Key.GetComponent<Rigidbody2D>().isKinematic=false;
            }
            healthbar.transform.localScale = new Vector3(health / 2 * healthbar.transform.localScale.x, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
            Destroy(col.collider.gameObject);
        }
    }
}
