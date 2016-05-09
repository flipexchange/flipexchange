﻿using UnityEngine;
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
	public AudioClip text2Audio;
	public int level;
	private int playedSound;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		counter = 0;
		playedSound = 0;
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
			rectQuotes = new string[6];
			circleQuotes = new string[6];
			//[x1,y1,x2,y2] where x1,y1 is initial position and x2,y2 is where it goes after it teleports
			if (level == 1) {
				infoPositions = new float[,] {
					{ -6, -6, 7, -6 },
					{ 7, -6, 26, -5 },
					{ 26, -5, 50, -5 },
					{ 50, -5, 60, -5 },
                    { 71, -5, 90, -4 },{
						100,
						-6,
						199,
						-6
					}
				};

                circleQuotes[0] = "I can't believe I'm trapped in a body with you...";
                rectQuotes[0] = "When I find this wizard, I'm going to burn him alive!";
				circleQuotes[1] = "I can climb that ramp. Give me a turn!\n <color=#24ACE2>(Press 'z' to swap characters)</color>";
                rectQuotes[1] = "This slidy thing must be a Iceworld invention.";
				circleQuotes[2] = "I'm so nervous. I don't jump much in Iceworld... <color=#24ACE2>\n(Press 'spacebar' to jump)</color>";
				rectQuotes[2] = "A chasm! Let me jump over it! <color=#ff0000ff>\n(Press 'spacebar' to jump)</color>";
                circleQuotes[3] = "These red walls don't block me. I can pass them no problem.";
                rectQuotes[3] = "But I can pass through these blue walls no problem.";
				circleQuotes[4] = "I know what to do! Try  <color=#24ACE2>switching</color> to me in midair!";
                rectQuotes[4] = "Snowball! We need to work together for this puzzle.";

                /*
				rectQuotes [0] = "I bet I can get to the end of this world all on my own! Why do we need to be merged?";
				circleQuotes [0] = "You fire folk are so brash. You always rush things; I can't wait until we unmerge.";
				circleQuotes [1] = "Even if you can jump and run faster than me, that doesn't mean you can climb ramps as well as I can...";
				rectQuotes [1] = "Ice Maiden, this ramp is too steep even for me! I can't seem to rush past it...but maybe you can?";
				rectQuotes [2] = "Seems like I can't get past red bars on my own?";
				circleQuotes [2] = "Hm, the red bars don't seem to block me. Let me take over for a little while.";
				rectQuotes [3] = "Man, I wish I could jump high enough to reach that white portal...but we might need to switch off to make it.";
				circleQuotes [3] = "Seems like there is no way for us to make it to the portal unless we switch off.";
                */
            } else if (level == 2) {
				infoPositions = new float[,] {
					{ -8, -5, 2, -5 },
					{ 2, -5, 12, -5 },
					{ 12, -5, 40, -5 },
					{ 40, -6, 73, -6 },
                    { 73, -2, 116, -7 },
                    { 116, -7, 125, -7 },{
						150,
						-6,
						199,
						-6
					}
				};

                circleQuotes[0] = "Looks like I'm getting deeper into the city.";
                rectQuotes[0] = "Good! I can't stand another agonizing moment with you.";
                circleQuotes[1] = "You don't have the mass to push it down. Let me try.";
                rectQuotes[1] = "Only proves how fat you are...";
				circleQuotes[2] = "Something's coming. We should  <color=#24ACE2>hide</color> in this crystal cave.";
                rectQuotes[2] = "Something's rumbling like a volcano. Think fast!";
                circleQuotes[3] = "We need to stay calm and collected for these puzzles.";
                rectQuotes[3] = "Puzzles are all about speed and skill.";
				circleQuotes[4] = "Gunshots? Chill out red, <color=#24ACE2>look for openings</color> to progress.";
                rectQuotes[4] = "I hear gunshots. Someone is trying to extinguish us!";
                circleQuotes[5] = "We really showed him!";
                rectQuotes[5] = "We did it!";
                /*
				rectQuotes [0] = "What's this rectangular thing and why can't I push it over myself?";
				circleQuotes [0] = "You're way too light to get that over. We ice folk are much more competent at this type of thing.";
				circleQuotes [1] = "Pay attention! That rock's going to kill us if we don't find a way to switch off and avoid it.";
				rectQuotes [1] = "I don't think I can jump high enough to get over that rock...but I can still try?";
				rectQuotes [2] = "We might have gotten past that rock but just sayin', I did most of the work there.";
				circleQuotes [2] = "I definitely helped us get under that platform so I'd say that I did more of the work there...";
				rectQuotes [3] = "Eek seems like someone's not happy about us being here together, let's try to somehow hide from the bullets?";
				circleQuotes [3] = "That guy really doesn't want the Fire and Ice nations to merge. But we have to get past him.";
                */
            } else  {
				infoPositions = new float[,] {
					{ -10, -8, 15, -4 },
					{ 15, -4, 24, -8 },
					{ 33, -8, 76, -8 },
					/*{ 40, -2, 44, -2 },
                    { 57, -2, 53, -2 },*/
                    { 76, -8, 83, -8 },{
						150,
						-5,
						199,
						-5
					}
				};

                circleQuotes[0] = "Against all odds, we've arrived at the city center!";
                rectQuotes[0] = "Against what odds? We're blazing through the levels!";
                circleQuotes[1] = "There's a city guard blocking our exit.";
                rectQuotes[1] = "There's someone in our way. Let's get fired up!";
				circleQuotes[2] = " <color=#24ACE2>Get above</color> him, then <color=#24ACE2>roll</color> fire and ice boulders onto him.";
                rectQuotes[2] = "We need to take the key from him.";
                /*
                circleQuotes[3] = "This rock will hurt him!";
                rectQuotes[3] = "Let's roast this fellow!";
                circleQuotes[4] = "This will slow him down.";
                rectQuotes[4] = "Burn!";
                */
                circleQuotes[3] = "That was quite the journey! I could've done it without you.";
                rectQuotes[3] = "We did it! You're not so bad, blue.";
                /*
				rectQuotes [0] = "Is this the end? I've think I've gotten the hang of this by now.";
				circleQuotes [0] = "Come on. Let's not slack off now. There's still one last obstacle for us.";
				circleQuotes [1] = "There's no way for us to beat this monster unless we find a way to push those rocks on his head.";
				rectQuotes [1] = "Jeez this guy won't stop shooting bullets, I think we might be able to jump above him if we switch off?";
				rectQuotes [2] = "Time to put an end to this guy.";
				circleQuotes [2] = "This rock will probably hurt him and slow him down.";
				rectQuotes [3] = "So we did it. I can't believe we really did it. You aren't half bad.";
				circleQuotes [3] = "That was quite the journey. Truthfully, I don't know if  I could've done it without you.";
                */
            }
			//setting things in place
			Vector3 newPos = new Vector3 (infoPositions [counter, X1], infoPositions [counter, Y1], infoBox.transform.position.z);
			infoBox.transform.position = newPos;
			this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
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
			if (playedSound ==-1 || playedSound == 2) {
				source.clip = text2Audio;
				source.Play ();
				playedSound--;
				

			}
		} else {
			dialogueText.text = rectQuotes [counter];
			rect.SetActive (true);
			circle.SetActive (false);
			if (playedSound == 1 || playedSound == -2) {
				source.clip = textAudio;
				source.Play ();

				playedSound++;
			}
		}

	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.transform.gameObject.name == "Player") {
			if (showingQuote == false) {

				dialogueBox.SetActive (true);
				updateQuote ();
				if (!playerScript.pink)
					playedSound = 1;
				else
					playedSound = -1;
				Vector3 newPos = new Vector3 (infoPositions [counter, X2], infoPositions [counter, Y2], infoBox.transform.position.z);
				infoBox.transform.position = newPos;
				this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

				showingQuote = true;

			} else {
				this.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);

				counter++;
				playedSound = 0;
				Vector3 newPos = new Vector3 (infoPositions [counter, X1], infoPositions [counter, Y1], infoBox.transform.position.z);
				infoBox.transform.position = newPos;
				dialogueBox.SetActive (false);
				showingQuote = false;
				source.Stop ();

			}

		}
	
	}
}
