using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class dialogBehaviour : MonoBehaviour {
	private GameObject infoBox;
	private GameObject dialogueBox;
	private GameObject player;
	private GameObject rect;
	private GameObject circle;
	private Text dialogueText;
	private PlayerControls playerScript;
	private string[] circleQuotes;
	private string[] rectQuotes;
	private float[,] infoPositions;
	private int X1;
	private int X2;
	private int Y1;
	private int Y2;
	private int counter;
	private bool showingQuote;
	private AudioSource source;
	public AudioClip textAudio;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		counter = 0;
		player = GameObject.Find ("Player");
		infoBox = GameObject.Find("Info");
		showingQuote = false;
		if (player != null) {
			playerScript = player.GetComponent<PlayerControls> ();

			dialogueBox = GameObject.Find ("DialogueBox");
			circle = GameObject.Find ("DialogueBox/circle");
			rect = GameObject.Find ("DialogueBox/rect");
			dialogueText = GameObject.Find ("DialogueBox/Canvas/DialogueText").GetComponent<Text> ();
			dialogueBox.SetActive (false);
			X1 = 0;
			X2 = 2;
			Y1 = 1;
			Y2 = 3;
			//[x1,y1,x2,y2] where x1,y1 is initial position and x2,y2 is where it goes after it teleports
			infoPositions = new float[,] {
				{ -5, -6, 5, -6 },
				{ 9, -4, 17, -4 },
				{ 47.86f, -4, 67, -4 },
				{ 78, -2.89f, 83, -2.89f },
				 {
					100,
					-5,
					199,
					-5
				}
			};

			rectQuotes = new string[5];
			circleQuotes = new string[5];
			rectQuotes [0] = "I bet I can get to the end of this world all on my own! Why do we need to be merged?";
			circleQuotes [0] = "You fire folk are so brash. You always rush things; I can't wait until we unmerge.";

			circleQuotes [1] = "Even if you can jump and run faster than me, that doesn't mean you can climb ramps as well as I can...";
			rectQuotes [1] = "Ice Maiden, this ramp is too steep even for me! I can't seem to rush past it...but maybe you can?";
			rectQuotes [2] = "What are those weird red bars? Why can't I get past them on my own?";
			circleQuotes [2] = "Hm, the red bars don't seem to block me. Let me take over for a little while.";
			rectQuotes [3] = "Man, I wish I could jump high enough to reach that white portal...but we might need to switch off to make it.";
			circleQuotes [3] = "Seems like there is no way for us to make it to the portal unless we switch off.";


			//setting things in place
			Vector3 newPos = new Vector3 (infoPositions [counter, X1], infoPositions [counter, Y1], infoBox.transform.position.z);
			infoBox.transform.position = newPos;
			this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}


	}
	
	// Update is called once per frame
	void Update () {
		if (showingQuote) {
			updateQuote ();
		}
	}
	void updateQuote()
	{
		if (playerScript.pink) {
			dialogueText.text = circleQuotes [counter];
			circle.SetActive (true);
			rect.SetActive (false);
		} else {
			dialogueText.text = rectQuotes [counter];
			rect.SetActive (true);
			circle.SetActive (false);
		}
	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		var sign = 1;
		if (!playerScript.pink && playerScript.flippingAnimation) {
			sign = -1;
		}
		if (coll.transform.gameObject.name == "Player") {
			if (showingQuote == false) {

				dialogueBox.SetActive (true);
				updateQuote ();
				Vector3 newPos = new Vector3 (infoPositions [counter, X2], sign*infoPositions [counter, Y2], infoBox.transform.position.z);
				infoBox.transform.position = newPos;
				this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

				showingQuote = true;
				source.clip = textAudio;
				source.Play ();
			} else {
				this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);

				counter++;

				Vector3 newPos = new Vector3 (infoPositions [counter, X1], sign*infoPositions [counter, Y1], infoBox.transform.position.z);
				infoBox.transform.position = newPos;
				dialogueBox.SetActive (false);
				showingQuote = false;
				source.Stop ();

			}

		}
	
	}
}
