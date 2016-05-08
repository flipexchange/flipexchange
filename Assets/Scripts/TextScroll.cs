using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextScroll : MonoBehaviour {
	public Text textBox;
	private bool finishedType;
	Sprite[] cutscenes;
	private int cutscenePos = 0;
	private GameObject fireIcon;
	private GameObject iceIcon;


	//Store all your text in this string array
	string[] cutscene1Text = new string[]{"In the beginning, there were two nations, Lavaland and Iceworld.","Due to their differences, the two lands constantly argued and fought.","  x:    Lavalanders are too quick to jump into action. Never considering the consequences before acting. Your recklessness ruins everything.", "  z:    You Iceworlders move so slowly. It's a miracle that anything gets done!", "  x:    Watch your tongue hotheds, or we'll freeze you where you stand.", "  z:    Your threats are feeble like embers. If it's fight is what you want, we'll bring you an inferno!"};
	string[] cutscene2Text = new string[]{"A war erupted, with massive casualties on both sides.", "Two soldiers, one from Lavaland, and one from Iceworld continued to fight amongst the chaos."};
	string[] cutscene3Text = new string[]{ "Wizard: Enough!", "Wizard: I'm merging you two so that Lavaland and Iceworld can work together for once!"};
	string[] cutscene4Text = new string[]{"And so, the fire soldier and ice soldier were merged."};
	string[][] allCutsceneText = new string[4][];
	string[] goalText = new string[]{"Long ago, there were two nations, Lavaland and Iceworld.","The two lands were separated, and hated each other due to their massive differences.","  x  : Lavaland people never think before jumping into action! You guys are so reckless, it's ruining everything.", "  z  : Well you Iceworld people move so slowly, no wonder we don't like you guys! We would never work with you.", "  x  : I hate you!", "  x  : You guys are the worst, I never want to see you near our land!","  z  : Same here, you lame icicles!"};
	int currentlyDisplayingText = 0;

	public AudioClip textScrollAudio;
	private AudioSource textScrollAudioSource;
	public AudioClip ancientAudio;
	public AudioClip magicAudio;
	private AudioSource ambientAudioSource;
	void Awake () {
		textScrollAudioSource =AddAudio(textScrollAudio, true, false, 1.0f);
		ambientAudioSource =AddAudio(ancientAudio, true, true, 1.0f);
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
		allCutsceneText [0] = cutscene1Text;
		allCutsceneText [1] = cutscene2Text;
		allCutsceneText [2] = cutscene3Text;
		allCutsceneText [3] = cutscene4Text;
		goalText = allCutsceneText [0];
		finishedType = false;
		fireIcon.SetActive (false);
		iceIcon.SetActive (false);
		cutscenes = new Sprite[]{Resources.Load<Sprite>("cutscene2"),Resources.Load<Sprite>("cutscene3"),Resources.Load<Sprite>("cutscene4")};
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
			SceneManager.LoadScene ("instructionScene", LoadSceneMode.Single);
		} else {
			SpriteRenderer showCutscene = GameObject.Find ("Cutscene").GetComponent<SpriteRenderer> ();
			showCutscene.sprite = cutscenes[cutscenePos++];
			goalText = allCutsceneText [cutscenePos];

			if (cutscenePos == 2) {
				ambientAudioSource.clip = magicAudio;
				ambientAudioSource.Play ();
				fader.EndSceneFast ();
			} else {
				fader.EndScene ();
			}
		}
	}
}
