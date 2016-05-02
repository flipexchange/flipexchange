using UnityEngine;
using System.Collections;

public class gunnerController : MonoBehaviour {
    /*  BULLET LOGIC  */
    public GameObject Bullet_Emitter;
    public GameObject Bullet;
    
    public float Bullet_Forward_Force;
    public bool activated;

    public int FiringPeriod;
    public int LullPeriod;
    private int lullCounter;
    private int firingCounter;
    private bool isFiring;

    public int BobPeriod;
    private bool bobUp;
    private int bobCounter;
    private int bobState;

	private AudioSource source;
	public AudioClip bulletAudio;

    void Start () {
        /*  Audio Stuff  */
        source = GetComponent<AudioSource> ();

        activated = false;

        /*  Firing Logic  */
        isFiring = false;
        firingCounter = 0;
        lullCounter = 0;

        /*  Bob Logic  */
        bobUp = true;
        bobState = 0;
        bobCounter = 0;

        /*  SCALE  */
        //transform.localScale += new Vector3(0f, 0.2f, 0f);

        /*  ALPHA CODE  */
        Color color = GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        GetComponent<Renderer>().material.SetColor("_Color", color);
    }
	
	void Update () {
        if (activated)
        {
            /*  Firing Logic  */
            lullCounter++;
            if (lullCounter > LullPeriod)
            {
                lullCounter = 0;
                isFiring = !isFiring;
            }
            if (isFiring)
            {
                firingCounter++;
                if (firingCounter > FiringPeriod)
                {
                    source.clip = bulletAudio;
                    source.Play();
                    firingCounter = 0;
                    //The Bullet instantiation happens here.
                    GameObject Temporary_Bullet_Handler;
                    Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

                    //Retrieve the Rigidbody component from the instantiated Bullet and control it.
                    Rigidbody2D Temporary_RigidBody;
                    Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody2D>();

                    //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
                    Temporary_RigidBody.AddForce(Vector2.left * Bullet_Forward_Force);

                    //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
                    Destroy(Temporary_Bullet_Handler, 5.0f);
                }
            }

            /*  Bob Logic  */
            bobCounter++;
            if (bobCounter > BobPeriod)
            {
                /*  Reset  */
                bobCounter = 0;

                if (bobUp)
                {
                    bobState++;
                    transform.localScale += new Vector3(0f, 0.05f, 0f);
                }
                else
                {
                    bobState--;
                    transform.localScale -= new Vector3(0f, 0.05f, 0f);
                }
                if (bobState == 3 || bobState == 0)
                {
                    bobUp = !bobUp;
                }    
            }

            
        }
    }
}
