using UnityEngine;
using System.Collections;

public class gateController : MonoBehaviour {

	public GameObject Key;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.GetComponent<CircleCollider2D>().gameObject==Key){
			
			Debug.Log ("Collision detected");
			StartCoroutine("OpenGate");
			Destroy(Key);
		}
	}

	IEnumerator OpenGate()
	{
		// Move the Gate object by y
		var endPosition = gameObject.transform.position.y+2;
		while (gameObject.transform.position.y< endPosition) {
			Vector3 newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+0.05f, gameObject.transform.position.z);
			gameObject.transform.position = newPosition;
			yield return new WaitForSeconds (0.03f);
		}
	}


}
