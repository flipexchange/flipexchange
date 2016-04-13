using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	private Rigidbody2D rb2d;

	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	[HideInInspector] public bool swap = false;
	public float moveForcePink = 500f;
	public float maxSpeedPink = 7f;
	public float jumpForcePink = 400f;
	public float gravityPink = 2.5f;
	public float moveForceBlue = 250f;
	public float maxSpeedBlue = 5f;
	public float jumpForceBlue = 650f;
	public float gravityBlue = 1f;
	public Transform groundCheck;
	public Transform groundCheckTop;
	public Transform groundCheckLeft;
	public Transform groundCheckRight;
	private bool grounded = false;
	private bool sloped = false;
	private bool tilted = false;
    private bool dead = false;
	private bool pink = true;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		groundCheck = transform.Find("groundCheck");
		groundCheckTop = transform.Find("groundCheckTop");
		groundCheckLeft = transform.Find("groundCheckLeft");
		groundCheckRight = transform.Find("groundCheckRight");
		var blueStuff = GameObject.FindGameObjectsWithTag("Blue");
		foreach (var obj in blueStuff) {
			Vector3 newPos = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
			obj.transform.position = newPos;
		}
	}
    
    // Update is called once per frame
    void Update () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		sloped = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Slope"));
        dead = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Death"));

        tilted = Physics2D.Linecast (transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer ("Ground"));
		tilted = tilted || Physics2D.Linecast (transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer ("Ground"));
		tilted = tilted || Physics2D.Linecast (transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer ("Ground"));

		sloped = sloped || Physics2D.Linecast (transform.position, groundCheckTop.position, 1 << LayerMask.NameToLayer ("Slope")) || Physics2D.Linecast (transform.position, groundCheckLeft.position, 1 << LayerMask.NameToLayer ("Slope")) || Physics2D.Linecast (transform.position, groundCheckRight.position, 1 << LayerMask.NameToLayer ("Slope"));
		if (Input.GetButtonDown("Switch"))
		{
			swap = true;
		}
		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
		}
        //Debug.Log(transform.position.x);
        Debug.Log(transform.Find("checkpoint1").position);
        //if (transform.position.x > 10) {}
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
				maxSpeed = 3f;
			} else {
				moveForce /= 20;
			}
		}

		if (tilted) {
			rb2d.rotation = 0f;
		}

        if (dead) {
            Vector3 newpos = new Vector3(-7, -6, 0);
            rb2d.transform.position = newpos;
            dead = false;
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
			if (pink) {
				rb2d.gravityScale = gravityPink;
				rb2d.transform.localScale = new Vector3 (1, 1, 1);
			} else {
				rb2d.gravityScale = gravityBlue;
				rb2d.transform.localScale = new Vector3 (1, 2, 1);
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
}
