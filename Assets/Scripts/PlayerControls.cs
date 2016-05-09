using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour {

	private Rigidbody2D rb2d;
	private SpriteRenderer sr;

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	[HideInInspector] public bool swap = false;
	public float moveForcePink = 500f;
	public float maxSpeedPink = 4f;
	public float jumpForcePink = 400f;
	public float gravityPink = 2f;
	public float moveForceBlue = 250f;
	public float maxSpeedBlue = 3.5f;
	public float jumpForceBlue = 250f;
	public float gravityBlue = 1f;
	public Transform groundCheck;
	public Transform groundCheckTop;
	public Transform groundCheckLeft;
	public Transform groundCheckRight;
	public Transform slopeCheck;
	public Transform slopeCheckBack;
	private bool grounded = false;
	private bool sloped = false;
	private bool tilted = false;
    private bool dead = false;
	public bool pink = true;
	private bool kick = false;
	private GameObject kickee;
	private Transform mainCamera;
	private bool swapping = false;
	private IEnumerator animating = null;
	private bool interruptable = false;

    // variable to store lastCheckpoint object
    private int checkpointNum = -1; 
    private GameObject lastCheckpoint;
    private GameObject nextCheckpoint;

    // variables for SecondLevel
    private bool currentSceneIsSecondLevel;
    private bool boulderTriggered = false;
    private bool gunnerStarted = false;
    GameObject gunner;
    float gunnerX;
    public Sprite surprised;

    // for sounds
    
	public AudioClip fireAudio;
	public AudioClip iceAudio;
	public AudioClip jumpAudio;
	public AudioClip dieAudio;
	public AudioClip rollAudio;

	private AudioSource fireAudioSource;
	private AudioSource iceAudioSource;
	private AudioSource jumpAudioSource;
	private AudioSource dieAudioSource;
	private AudioSource rollAudioSource;


	// animation
	private bool wasZero = true;
	Sprite[] pinkFrames;
	Sprite[] blueFrames;
	Sprite[] fireAnimation;
	Sprite[] iceAnimationStill;
	Sprite[] iceAnimationFwd;
	Sprite[] iceAnimationBwd;

	// Use this for initialization
	void Start () {
        // Set Deathbed's alpha to 0
        Color colorPicker = new Color(0.5f, 0.5f, 0.5f);
        colorPicker.a = 0;
        //GameObject.Find("Deathbed").GetComponent<Renderer>().material.SetColor("_Color", colorPicker);
		//allAudio = GetComponent<AudioSource>();


        rb2d = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		mainCamera = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform;
		groundCheck = transform.Find("groundCheck");
		groundCheckTop = transform.Find("groundCheckTop");
		groundCheckLeft = transform.Find("groundCheckLeft");
		groundCheckRight = transform.Find("groundCheckRight");
		slopeCheck = transform.Find ("slopeCheck");
		slopeCheckBack = transform.Find ("slopeCheckBack");
		var blueStuff = GameObject.FindGameObjectsWithTag("Blue");
		pinkFrames = new Sprite[5];
		for (int i = 1; i <= 5; i++) {
			pinkFrames.SetValue(Resources.Load<Sprite> ("firem" + i),i-1);
		}
		blueFrames = new Sprite[5];
		for (int i = 1; i <= 5; i++) {
			blueFrames.SetValue(Resources.Load<Sprite> ("icem" + i),i-1);
		}
		fireAnimation = new Sprite[]{Resources.Load<Sprite>("firem3"),Resources.Load<Sprite>("firem3"),Resources.Load<Sprite>("firem2"),Resources.Load<Sprite>("firem1"),Resources.Load<Sprite>("firem2"),Resources.Load<Sprite>("firem3"),Resources.Load<Sprite>("firem4"),Resources.Load<Sprite>("firem5"),Resources.Load<Sprite>("firem4"),Resources.Load<Sprite>("firem3")};
		iceAnimationStill = new Sprite[7];
		iceAnimationFwd = new Sprite[7];
		iceAnimationBwd = new Sprite[7];
		for (int i = 1; i <= 7; i++) {
			iceAnimationStill.SetValue (Resources.Load<Sprite> ("shine/stillshine-" + i), i - 1);
			iceAnimationFwd.SetValue (Resources.Load<Sprite> ("shine/fwdshine-" + i), i - 1);
			iceAnimationBwd.SetValue (Resources.Load<Sprite> ("shine/bwdshine-" + i), i - 1);
		}
		foreach (var obj in blueStuff) {
			if (obj.name != "BackgroundQuad (2)" && obj.name != "BackgroundQuad" && obj.name != "BackgroundQuad (1)") {
				Vector3 newPos = new Vector3 (obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
				obj.transform.position = newPos;
				obj.transform.Rotate (0, 0, 180);
				Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
				if (rigidbody != null) {
					if (!pink) {
						rigidbody.gravityScale = 1;
					} else {
						rigidbody.gravityScale = 0;
					}
				}
			}
		}
        // to iterate through the checkpoints: {checkpoint0, checkpoint1, ...}
		nextCheckpoint = GameObject.Find("checkpoint"+checkpointNum);
		currentSceneIsSecondLevel = SceneManager.GetActiveScene().name=="SecondLevel";
        if (currentSceneIsSecondLevel) {
            gunner = GameObject.Find("gunner");
            gunnerX = 99999f;
        }
    }
	public AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {

		AudioSource newAudio = gameObject.AddComponent<AudioSource>();

		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;

		return newAudio;

	}

	void Awake(){
		fireAudioSource =AddAudio(fireAudio, false, false, 1.0f);
		iceAudioSource =AddAudio(iceAudio, false, false, 1.0f);
		jumpAudioSource =AddAudio(jumpAudio, false,false, 1.0f);
		dieAudioSource =AddAudio(dieAudio, false,false, 1.0f);
		rollAudioSource =AddAudio(rollAudio, false,false, 1.5f);



	} 
    // Update is called once per frame
    void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		sloped = Physics2D.Linecast(transform.position, slopeCheck.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, slopeCheckBack.position, 1 << LayerMask.NameToLayer("Slope"));
		sloped = sloped && !grounded;
        tilted = Physics2D.Linecast(transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer("Ground"));
        tilted = tilted || Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"));
        tilted = tilted || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));
        //sloped = sloped || Physics2D.Linecast(transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Slope"));
		if (transform.position.y < -10)
			dead = true;
        if (Input.GetButtonDown("Switch") && !swapping)
            swap = true;
		if (Input.GetButtonDown ("Jump") && grounded && Mathf.Abs(rb2d.velocity.y)<.02) {
			jump = true;
			jumpAudioSource.Play ();
		}
        if (kick && Input.GetButtonDown("Kick"))
            StartCoroutine(kickIt());

        // Checkpoint logic
        if (transform.position.x > nextCheckpoint.transform.position.x) {
            lastCheckpoint = GameObject.Find("checkpoint" + checkpointNum);
            checkpointNum++;
			if (GameObject.Find ("checkpoint" + checkpointNum) != null) {
				// Set next checkpoint
				nextCheckpoint = GameObject.Find ("checkpoint" + checkpointNum);
			} 
            /*else {
                // Jump to next level when there's no more checkpoint.
                ended = true;   
            }*/
        }

        /*if (ended) {
            levelCounter++;
            if (levelCounter > 100) {
                int i = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(i + 1);
            }
        }*/

        // SecondLevel Methods
        if (currentSceneIsSecondLevel) { //These scripts are specific to SecondLevel
            if (transform.position.x > 14 && !boulderTriggered)
            {
                boulderTriggered = true;
                GameObject.Find("boulder").transform.position = new Vector3(39f,-2f,0f);
            }
            
            if (transform.position.x > 65 && !gunnerStarted)
            {
                gunnerStarted = true;
                gunner.GetComponent<gunnerController>().activated = true;
                gunnerX = gunner.transform.position.x;
            }
            if (transform.position.x > gunnerX+1) {
                gunner.GetComponent<gunnerController>().activated = false;
                gunner.GetComponent<SpriteRenderer>().sprite = surprised;
            }
        }
    }

	void FixedUpdate()
	{
		float moveForce = moveForcePink;
		float maxSpeed = maxSpeedPink;
		float jumpForce = jumpForcePink;
		if (!pink) {
			moveForce = moveForceBlue;
			maxSpeed = maxSpeedBlue;
			jumpForce = jumpForceBlue;
		}
		if (animating==null) {
			if ((int)(Time.time*100) % 600 == 0) {
				if (pink) {
					if (wasZero) {
						animating = fireFlick ();
					}
				} else {
					animating = iceFlick ();
				}
				if (!pink || wasZero) {
					StartCoroutine (animating);
				}
				wasZero = false;
			}
			else if (rb2d.velocity.x < -0.5) {
				if (pink) {
					if (rb2d.velocity.x < -maxSpeed+2.5f) {
						sr.sprite = pinkFrames [0];
					} else {
						sr.sprite = pinkFrames [1];
					}
				} else {
					if (rb2d.velocity.x < -maxSpeed+2f) {
						sr.sprite = blueFrames [0];
					} else {
						sr.sprite = blueFrames [1];
					}
				}
				wasZero = false;
			} else if (rb2d.velocity.x > 0.5) {
				if (pink) {
					if (rb2d.velocity.x > maxSpeed-2.5f) {
						sr.sprite = pinkFrames [4];
					} else {
						sr.sprite = pinkFrames [3];
					}
				} else {
					if (rb2d.velocity.x > maxSpeed-2f) {
						sr.sprite = blueFrames [4];
					} else {
						sr.sprite = blueFrames [3];
					}
				}
				wasZero = true;
			} else {
				if (wasZero) {
					if (pink) {
						sr.sprite = pinkFrames [2];
					} else {
						sr.sprite = blueFrames [2];
					}
				}
				wasZero = true;
			}
		}
		if (sloped) {
			if (pink) {
				moveForce = 0f;
				maxSpeed = 1.5f;
			} else {
				moveForce /= 10;
				if(rollAudioSource.isPlaying == false)
					rollAudioSource.Play ();
			}

		}
		if (swapping) {
			moveForce = 0;
			jumpForce = 0;
			maxSpeed = 0;
		}
		if (tilted || !pink) {
			if (rb2d.rotation != 0f) {
				rb2d.rotation = 0f;
			}
		}

		if (dead) {
			rb2d.transform.position = new Vector3(lastCheckpoint.transform.position.x,lastCheckpoint.transform.position.y,lastCheckpoint.transform.position.z);
            dead = false;
            if (currentSceneIsSecondLevel && checkpointNum < 2) {
                GameObject boulder = GameObject.Find("boulder");
                boulder.transform.position = new Vector3(41f, -2f, 0f);
                boulder.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                boulder.GetComponent<Rigidbody2D>().angularVelocity = 0;
                boulderTriggered = false;
            }
        }

		float h = Input.GetAxis("Horizontal");

		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);

		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (jump)
		{
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
			if (animating == null) {
				animating = jumpAnimation ();
				StartCoroutine (animating);
			} else if (animating != null && interruptable) {
				StopCoroutine (animating);
				animating = jumpAnimation ();
				StartCoroutine (animating);
			}
		}
		if (swap) {
			pink = !pink;
			// SecondLevel Methods
			if (currentSceneIsSecondLevel)
			{ //These scripts are specific to SecondLevel
				GameObject col = GameObject.Find("bridge");
				if (pink) {
					col.gameObject.GetComponent<Rigidbody2D>().mass = 1000000;
				}
				else
				{ // Cheating hardcoded bridge method
					col.gameObject.GetComponent<Rigidbody2D>().mass = 10;
				}
			}
			var box = GetComponent<BoxCollider2D>();
			var circle = GetComponent<CircleCollider2D>();
			if (pink) {
				//GetComponent<Animation>().CrossFade("RedToBlue", 0.5f, PlayMode.StopAll);;
				rb2d.gravityScale = gravityPink;
				if (animating!=null) {
					StopCoroutine (animating);
				}
				animating = blueToRed();
				StartCoroutine(animating);
				//sr.sprite = Resources.Load<Sprite>("firem");
				box.enabled = true;
				circle.enabled = false;
				if (transform.parent == null) {
					transform.localScale = new Vector3 (1.65f,1.65f,1);
				} else {
					transform.localScale = new Vector3 (0.4125f,1.65f,1);
				}
				fireAudioSource.Play ();
			} else {
				//GetComponent<Animation>().CrossFade("BlueToRed", 0.5f, PlayMode.StopAll);
				rb2d.gravityScale = gravityBlue;

				//sr.sprite = Resources.Load<Sprite>("icem");
				if (animating!=null) {
					StopCoroutine (animating);
				}
				animating = redToBlue();
				StartCoroutine(animating);
				box.enabled = false;
				circle.enabled = true;
				if (transform.parent == null) {
					transform.localScale = new Vector3 (1.65f,1.65f,1);
				} else {
					transform.localScale = new Vector3 (0.4125f,1.65f,1);
				}
				iceAudioSource.Play ();
			}
			var pinkStuff = GameObject.FindGameObjectsWithTag ("Pink");
			foreach (var obj in pinkStuff) {
				Vector3 newPos = new Vector3 (obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);

				obj.transform.position = newPos;
				obj.transform.Rotate (0, 0, 180);
				Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
				if (rigidbody != null) {
					if (pink) {
						rigidbody.gravityScale = 1;
					} else {
						rigidbody.gravityScale = 0;
					}
				}
			}
			var blueStuff = GameObject.FindGameObjectsWithTag ("Blue");
			foreach (var obj in blueStuff) {
				if (obj.name != "BackgroundQuad (2)" && obj.name != "BackgroundQuad" && obj.name != "BackgroundQuad (1)") {
					Vector3 newPos = new Vector3 (obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
					obj.transform.position = newPos;
				}
				obj.transform.Rotate (0, 0, 180);
				Rigidbody2D rigidbody = obj.GetComponent<Rigidbody2D>();
				if (rigidbody != null) {
					if (!pink) {
						rigidbody.gravityScale = 1;
					} else {
						rigidbody.gravityScale = 0;
					}
				}
			}
            if (currentSceneIsSecondLevel)
            {
                if (pink)
                {
                    GameObject.Find("bridge").GetComponent<Rigidbody2D>().gravityScale = 0;
                }
                else
                {
                    GameObject.Find("bridge").GetComponent<Rigidbody2D>().gravityScale = 1;
                }
            }
			swap = false;
		}
	}

	public void SetAllCollidersStatus (GameObject obj, bool active) {
		foreach(Collider2D c in obj.GetComponents<Collider2D> ()) {
			c.enabled = active;
		}
	}

	IEnumerator rotate() {
		swapping = true;
		var old = mainCamera.rotation.z;
		if (old == 0) {
			for (int x = 0; x<18; x++) {
				//mainCamera.rotation *= Quaternion.Euler(0, 0, 10);
				transform.rotation *= Quaternion.Euler(0, 0, -10);
				yield return new WaitForSeconds (0.003f);
			}
			//mainCamera.rotation = Quaternion.Euler(0,0,180);
			transform.rotation = Quaternion.Euler(0,0,0);
		} else {
			for (int x  = 0; x<18; x++) {
				//mainCamera.rotation *= Quaternion.Euler(0, 0, -10);
				transform.rotation *= Quaternion.Euler(0, 0, 10);
				yield return new WaitForSeconds (0.003f);
			}
			//mainCamera.rotation = Quaternion.Euler(0,0,0);
			transform.rotation = Quaternion.Euler(0,0,180);
		}
		transform.Rotate (0,0,180);
		swapping = false;
	}

	IEnumerator fireFlick() {
		interruptable = true;
		for (var i = 0; i < fireAnimation.Length; i++) {
			sr.sprite = fireAnimation[i];
			yield return new WaitForSeconds (0.1f);
		}
		if (rb2d.velocity.x < -0.5) {
			if (rb2d.velocity.x < -maxSpeedPink+2.5f) {
				sr.sprite = pinkFrames [0];
			} else {
				sr.sprite = pinkFrames [1];
			}
		} else if (rb2d.velocity.x > 0.5) {
			if (rb2d.velocity.x > maxSpeedPink-2.5f) {
				sr.sprite = pinkFrames [4];
			} else {
				sr.sprite = pinkFrames [3];
			}
		}
		animating = null;
		interruptable = false;
	}

	IEnumerator iceFlick() {
		for (var i = 0; i < iceAnimationStill.Length; i++) {
			if (rb2d.velocity.x < -0.5) {
				sr.sprite = iceAnimationBwd [i];
			} else if (rb2d.velocity.x > 0.5) {
				sr.sprite = iceAnimationFwd [i];
			} else {
				sr.sprite = iceAnimationStill [i];
			}
			yield return new WaitForSeconds (0.015f);
		}
		if (rb2d.velocity.x < -0.5) {
			if (rb2d.velocity.x < -maxSpeedBlue+2f) {
				sr.sprite = blueFrames [0];
			} else {
				sr.sprite = blueFrames [1];
			}
		} else if (rb2d.velocity.x > 0.5) {
			if (rb2d.velocity.x > maxSpeedBlue-2f) {
				sr.sprite = blueFrames [4];
			} else {
				sr.sprite = blueFrames [3];
			}
		}
		animating = null;
	}

	IEnumerator kickIt() {
		var old = kickee.transform.position.y;
		while (kickee.transform.position.y<-old) {
			Vector3 mwah = new Vector3(kickee.transform.position.x, kickee.transform.position.y+2, kickee.transform.position.z);
			kickee.transform.position = mwah;
			yield return new WaitForSeconds (0.03f);
		}
		Vector3 mwa = new Vector3(kickee.transform.position.x, -old, kickee.transform.position.z);
		kickee.transform.position = mwa;
	}

	IEnumerator blueToRed() {
		for (var i = 7; i < 10; i++) {
			sr.sprite = Resources.Load<Sprite> ("switch/Animation" + i);
			yield return new WaitForSeconds (0.015f);
		}
		for (var i = 10; i <= 13; i++) {
			sr.sprite = Resources.Load<Sprite> ("switch/Animation" + i);
			yield return new WaitForSeconds (0.005f);
		}
		sr.sprite = Resources.Load<Sprite> ("firem");
		animating = null;
	}

	IEnumerator jumpAnimation() {
		if (pink) {
			sr.sprite = Resources.Load<Sprite> ("firemjump");
		}
		for (int i = 0; i<25; i++) {
			if (!pink) {
				if (rb2d.velocity.x < -0.5) {
					if (rb2d.velocity.x < -maxSpeedBlue + 2f) {
						sr.sprite = blueFrames [0];
					} else {
						sr.sprite = blueFrames [1];
					}
					wasZero = false;
				} else if (rb2d.velocity.x > 0.5) {
					if (rb2d.velocity.x > maxSpeedBlue - 2f) {
						sr.sprite = blueFrames [4];
					} else {
						sr.sprite = blueFrames [3];
					}
					wasZero = true;
				} else {
					if (wasZero) {
						sr.sprite = blueFrames [2];
					}
					wasZero = true;
				}
			}
			yield return new WaitForSeconds (0.01f);
		}
		if (pink) {
			sr.sprite = Resources.Load<Sprite> ("firemjumpdown");
		} else {
			GetComponent<CircleCollider2D> ().radius = 0.48f;
		}
		while (!grounded) {
			if (!pink) {
				if (rb2d.velocity.x < -0.5) {
					if (rb2d.velocity.x < -maxSpeedBlue + 2f) {
						sr.sprite = blueFrames [0];
					} else {
						sr.sprite = blueFrames [1];
					}
					wasZero = false;
				} else if (rb2d.velocity.x > 0.5) {
					if (rb2d.velocity.x > maxSpeedBlue - 2f) {
						sr.sprite = blueFrames [4];
					} else {
						sr.sprite = blueFrames [3];
					}
					wasZero = true;
				} else {
					if (wasZero) {
						sr.sprite = blueFrames [2];
					}
					wasZero = true;
				}
			}
			yield return new WaitForSeconds (0.01f);
		}
		if (!pink) {
			for (int i = 0; i<3; i++) {
				yield return new WaitForSeconds (0.05f);
			}
			for (int i = 0; i<5; i++) {
				GetComponent<CircleCollider2D> ().radius = 0.48f+((float)i)/100;
				yield return new WaitForSeconds (0.001f);
			}
		}
		animating = null;
	}

	IEnumerator redToBlue() {
		for (var i = 1; i < 4; i++) {
			sr.sprite = Resources.Load<Sprite> ("switch/Animation" + i);
			yield return new WaitForSeconds (0.015f);
		}
		for (var i = 4; i <= 7; i++) {
			sr.sprite = Resources.Load<Sprite> ("switch/Animation" + i);
			yield return new WaitForSeconds (0.005f);
		}
		sr.sprite = Resources.Load<Sprite> ("icem");
		animating = null;
	}

	void OnCollisionStay2D(Collision2D col) {
		if(col.collider.bounds.Contains(transform.position) && (col.gameObject.tag=="Pink" || col.gameObject.tag=="Blue"))
		{
			Debug.Log ("inside");
		}
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == 10) { //int value of 'Death' in layer manager(User Defined starts at 10)
            dead = true;
			dieAudioSource.Play ();
        }
		if (col.transform.gameObject.name == "Kickable") {
			kick = true;
			kickee = col.transform.gameObject;
		}

    }

	void OnCollisionExit2D(Collision2D col) {
		if (col.transform.gameObject.name == "Kickable") {
			kick = false;
		}
    }
}
