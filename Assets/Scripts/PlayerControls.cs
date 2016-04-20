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
	public float jumpForcePink = 450f;
	public float gravityPink = 2f;
	public float moveForceBlue = 200f;
	public float maxSpeedBlue = 3f;
	public float jumpForceBlue = 200f;
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
	private bool pink = true;
	private bool kick = false;
	private GameObject kickee;

    // variable to store lastCheckpoint object
    private int checkpointNum = 0; 
    private GameObject lastCheckpoint;
    private GameObject nextCheckpoint;
	private GameObject infoBox;
	private GameObject dialogueBox;

    // variables for SecondLevel
    private bool currentSceneIsSecondLevel;
    private bool boulderTriggered = false;

	// Use this for initialization
	void Start () {
        // Set Deathbed's alpha to 0
        Color colorPicker = new Color(0.5f, 0.5f, 0.5f);
        colorPicker.a = 0;
        //GameObject.Find("Deathbed").GetComponent<Renderer>().material.SetColor("_Color", colorPicker);

        rb2d = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		groundCheck = transform.Find("groundCheck");
		groundCheckTop = transform.Find("groundCheckTop");
		groundCheckLeft = transform.Find("groundCheckLeft");
		groundCheckRight = transform.Find("groundCheckRight");
		slopeCheck = transform.Find ("slopeCheck");
		slopeCheckBack = transform.Find ("slopeCheckBack");
		var blueStuff = GameObject.FindGameObjectsWithTag("Blue");
		foreach (var obj in blueStuff) {
			Vector3 newPos = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
			obj.transform.position = newPos;
		}
        // to iterate through the checkpoints: {checkpoint0, checkpoint1, ...}
        nextCheckpoint = GameObject.Find("checkpoint"+checkpointNum);
        currentSceneIsSecondLevel = SceneManager.GetActiveScene().name=="SecondLevel";
		//find the next info tutorial point
		infoBox = GameObject.Find("Info");
		dialogueBox = GameObject.Find ("DialogueBox");
		dialogueBox.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		sloped = Physics2D.Linecast(transform.position, slopeCheck.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, slopeCheckBack.position, 1 << LayerMask.NameToLayer("Slope"));
        /*
        bool deadTop = Physics2D.Linecast(transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer("Death"));
        bool deadBot = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Death"));
        bool deadLeft = Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Death"));
        bool deadRight = Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Death"));
        dead = deadBot || deadLeft || deadRight || deadTop;
        */

        tilted = Physics2D.Linecast(transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer("Ground"));
        tilted = tilted || Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Ground"));
        tilted = tilted || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Ground"));
        //sloped = sloped || Physics2D.Linecast(transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer("Slope")) || Physics2D.Linecast(transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer("Slope"));
		if (transform.position.y < -10)
			dead = true;
        if (Input.GetButtonDown("Switch"))
            swap = true;
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
        if (kick && Input.GetButtonDown("Kick"))
            StartCoroutine(kickIt());

        // Checkpoint logic
        if (transform.position.x > nextCheckpoint.transform.position.x) {
            lastCheckpoint = GameObject.Find("checkpoint" + checkpointNum);
            checkpointNum++;
            if (GameObject.Find("checkpoint" + checkpointNum) != null)
                nextCheckpoint = GameObject.Find("checkpoint" + checkpointNum);
            else {
                int i = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(i+1);
            }
        }

        // SecondLevel Methods
        if (currentSceneIsSecondLevel) { //These scripts are specific to SecondLevel
            if (transform.position.x > 14 && !boulderTriggered)
            {
                boulderTriggered = true;
                GameObject.Find("boulder").transform.position = new Vector3(39f,-2f,0f);
            }
        }

		//the tutorial info box shows up when collide
		if (transform.position.x > infoBox.transform.position.x) {
			dialogueBox.SetActive (true);
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
		if (sloped) {
			if (pink) {
				moveForce = 0f;
				maxSpeed = 1.5f;
			} else {
				moveForce /= 10;
			}
		}

		if (tilted || !pink) {
			if (rb2d.rotation != 0f) {
				rb2d.rotation = 0f;
			}
		}

        if (dead) {
            rb2d.transform.position = lastCheckpoint.transform.position;
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
		}
		if (swap) {
			pink = !pink;
			var box = GetComponent<BoxCollider2D>();
			var circle = GetComponent<CircleCollider2D>();
			if (pink) {
				rb2d.gravityScale = gravityPink;
				sr.sprite = Resources.Load<Sprite>("rectangle");
				box.enabled = true;
				circle.enabled = false;
				transform.localScale = new Vector3 (.2f,.2f,1);
			} else {
				rb2d.gravityScale = gravityBlue;
				sr.sprite = Resources.Load<Sprite>("circle");
				box.enabled = false;
				circle.enabled = true;
				transform.localScale = new Vector3 (.25f,.25f,1);
			}
			var pinkStuff = GameObject.FindGameObjectsWithTag("Pink");
			foreach (var obj in pinkStuff) {
				Vector3 newPos = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
				obj.transform.position = newPos;
			}
			var blueStuff = GameObject.FindGameObjectsWithTag("Blue");
			foreach (var obj in blueStuff) {
				Vector3 newPos = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
				obj.transform.position = newPos;
			}
			swap = false;
		}
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

	void OnCollisionStay2D(Collision2D col) {
		if(col.collider.bounds.Contains(transform.position) && (col.gameObject.tag=="Pink" || col.gameObject.tag=="Blue"))
		{
			Debug.Log ("inside");
		}
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == 10) { //int value of 'Death' in layer manager(User Defined starts at 10)
            dead = true;
        }
		if (col.transform.gameObject.name == "Kickable") {
			kick = true;
			kickee = col.transform.gameObject;
		}

        // SecondLevel Methods
        if (currentSceneIsSecondLevel)
        { //These scripts are specific to SecondLevel
            if (col.gameObject.name == "bridge" && !pink)
            { // Cheating hardcoded bridge method
                col.gameObject.GetComponent<Rigidbody2D>().mass = 1;
            }
            //if (col.gameObject.name == "boulder") {
            /*
            if (col.gameObject.layer == 10 && checkpointNum < 2)
            {
                GameObject boulder = GameObject.Find("boulder");
                boulder.transform.position = new Vector3(41f, -2f, 0f);
                boulder.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                boulder.GetComponent<Rigidbody2D>().angularVelocity = 0;
                boulderTriggered = false;
            }*/
        }
    }

	void OnCollisionExit2D(Collision2D col) {
		//Debug.Log("gO: "+col.gameObject.layer);
		//Debug.Log("collider: " + col.collider.gameObject.layer);
		if (col.transform.gameObject.name == "Kickable") {
			kick = false;
		}
	}
}
