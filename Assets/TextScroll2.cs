using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextScroll2 : MonoBehaviour {
	public Text textBox;
	private bool finishedType;
	Sprite[] cutscenes;
	private int cutscenePos = 0;
	private GameObject fireIcon;
	private GameObject iceIcon;


	//Store all your text in this string array
	string[] cutscene1Text = new string[]{"Wizard: Being merged isn't that bad, wasn't it?","  x:    Hrm. It's still a pain to be forced to switch off all the time.", "  z:    Well it's not like I like letting you do all the work either!"};
	string[] cutscene2Text = new string[]{"Wizard: Now now, the trials haven't finished yet! Let's see if you can get past the city guards."};


	string[][] allCutsceneText = new string[2][];
	string[] goalText;

	string[] cutscene5Text = new string[]{"     "};

	int currentlyDisplayingText = 0;

	public AudioClip textScrollAudio;
	private AudioSource textScrollAudioSource;
	public AudioClip ancientAudio;
	public AudioClip magicAudio;
	private AudioSource ambientAudioSource;
	void Awake () {
		goalText = cutscene1Text;
		textScrollAudioSource =AddAudio(textScrollAudio, true, true, 1.0f);
		ambientAudioSource =AddAudio(magicAudio, true, false, 1.0f);
		StartCoroutine(AnimateText());


	}
	public AudioSource AddAudio (AudioClip clip, bool loop, bool playAwake, float vol) {

		AudioSource newAudio = gameObject.AddComponent<AudioSource>();

		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;

		return newAudio;

	}

	//This is a function for a button you press to skip to the next text
	public void SkipToNextText(){
		textScrollAudioSource.Stop ();
		StopAllCoroutines();

		//If we've reached the end of the array, do anything you want. I just restart the example text
		if (currentlyDisplayingText >= goalText.Length -1) {
			showNewCutscene ();
			StartCoroutine(AnimateText());
		} else {
			currentlyDisplayingText++;

			StartCoroutine(AnimateText());
		}
		if (goalText [currentlyDisplayingText] [2] == 'x') {
			fireIcon.SetActive (false);
			iceIcon.SetActive (true);
		} else if (goalText [currentlyDisplayingText] [2] == 'z' && goalText [currentlyDisplayingText] [0] != 'W') {
			iceIcon.SetActive (false);
			fireIcon.SetActive (true);
		} else {
			iceIcon.SetActive (false);
			fireIcon.SetActive (false);
		}

	}
	//Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
	IEnumerator AnimateText(){
		finishedType = false;


		for (int i = 0; i < (goalText[currentlyDisplayingText].Length+1); i++)
		{
			if (i == 3) {
				textScrollAudioSource.Play ();
			}
			textBox.text = goalText[currentlyDisplayingText].Substring(0, i);
			yield return new WaitForSeconds(.05f);
		}
		textScrollAudioSource.Stop ();
		finishedType = true;
	}
	// Use this for initialization
	void Start () {
		fireIcon = GameObject.Find ("Canvas/rect");
		iceIcon = GameObject.Find ("Canvas/circle");
		goalText = cutscene1Text;
		allCutsceneText [0] = cutscene1Text;
		allCutsceneText [1] = cutscene2Text;


		goalText = allCutsceneText [0];
		finishedType = false;
		fireIcon.SetActive (false);
		iceIcon.SetActive (false);
		cutscenes = new Sprite[]{Resources.Load<Sprite>("cutscene6")};
		ambientAudioSource.Play ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			if (finishedType) {
				SkipToNextText ();
				textScrollAudioSource.Stop ();
			} else {
				textScrollAudioSource.Stop ();
				StopAllCoroutines ();
				if (currentlyDisplayingText >= goalText.Length) {
					showNewCutscene ();
					StartCoroutine(AnimateText());
				}
				textBox.text = goalText [currentlyDisplayingText];
				finishedType = true;
			}
		}

	}
	void showNewCutscene()
	{
		currentlyDisplayingText = 0;

		//print (goalText [0]);

		SceneFadeInOut fader = GameObject.Find ("screenFader").GetComponent<SceneFadeInOut> ();

		if (cutscenePos >= cutscenes.Length) {
			fader.EndScene ();
			//print ("dONE W CUTSCENES");
			SceneManager.LoadScene ("SecondLevel", LoadSceneMode.Single);
		} else {
			SpriteRenderer showCutscene = GameObject.Find ("Cutscene").GetComponent<SpriteRenderer> ();
			showCutscene.sprite = cutscenes[cutscenePos++];
			goalText = allCutsceneText [cutscenePos];

			fader.EndScene ();

		}
	}
}
