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
	private bool grounded = false;
	private bool pink = true;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		groundCheck = transform.Find("groundCheck");
		var blueStuff = GameObject.FindGameObjectsWithTag("Blue");
		foreach (var obj in blueStuff) {
			Vector3 newPos = new Vector3(obj.transform.position.x, -obj.transform.position.y, obj.transform.position.z);
			obj.transform.position = newPos;
		}
	}

	// Update is called once per frame
	void Update () {
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if (Input.GetButtonDown("Switch"))
		{
			swap = true;
		}
		if (Input.GetButtonDown("Jump") && grounded)
		{
			jump = true;
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
