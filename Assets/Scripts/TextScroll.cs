using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextScroll : MonoBehaviour {
	public Text textBox;
	private bool finishedType;
	Sprite[] cutscenes;
	private int cutscenePos = 0;
	//Store all your text in this string array
	string[] cutscene1Text = new string[]{"Long ago, there were two nations, Lavaland and Iceland.","The two lands were separated, and did  not like each other due to their massive differences.","Ice:Lavaland people never think before jumping into action! You guys are so reckless, it's ruining everything.", "Lava:Well you Iceland people move so slowly, no wonder we don't like you guys! We would never work with you.", "Lava:I hate you guys!", "Ice:You guys are the worst, I never want to see you guys near our land!","Lava:Same here, you lame icicles!"};
	string[] cutscene2Text = new string[]{"A long battle ensued.", "Both nations would not stand down.","Soon, only two remained."};
	string[] cutscene3Text = new string[]{"Wizard: Enough!","Wizard:I'm merging you two so that LavaLand and IceLand can work together for once!"};
	string[] cutscene4Text = new string[]{"And so, Fire and Ice were merged."};
	string[][] allCutsceneText = new string[4][];
	string[] goalText = new string[]{"Long ago, there were two nations, Lavaland and Iceland.","The two lands were separated, and did  not like each other due to their massive differences.","Ice:Lavaland people never think before jumping into action! You guys are so reckless, it's ruining everything.", "Lava:Well you Iceland people move so slowly, no wonder we don't like you guys! We would never work with you.", "Lava:I hate you guys!", "Ice:You guys are the worst, I never want to see you guys near our land!","Lava:Same here, you lame icicles!"};
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
		allCutsceneText [0] = cutscene1Text;
		allCutsceneText [1] = cutscene2Text;
		allCutsceneText [2] = cutscene3Text;
		allCutsceneText [3] = cutscene4Text;
		goalText = allCutsceneText [0];
		finishedType = false;
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

		print (goalText [0]);

		SceneFadeInOut fader = GameObject.Find ("screenFader").GetComponent<SceneFadeInOut> ();
		fader.EndScene ();
		if (cutscenePos >= cutscenes.Length) {
			print ("dONE W CUTSCENES");
			SceneManager.LoadScene ("instructionScene", LoadSceneMode.Single);
		} else {
			SpriteRenderer showCutscene = GameObject.Find ("Cutscene").GetComponent<SpriteRenderer> ();
			showCutscene.sprite = cutscenes[cutscenePos++];
			goalText = allCutsceneText [cutscenePos];
			if (cutscenePos == 2) {
				ambientAudioSource.clip = magicAudio;
				ambientAudioSource.Play ();
			}
		}
	}
}
