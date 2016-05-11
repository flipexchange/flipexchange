using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour {

	public Transform target;
	public GameObject move;
	// Use this for initialization
	void Start () {
	}

	void Update() {
		var diff = target.position.x-transform.position.x;
		if (!(SceneManager.GetActiveScene ().name == "ThirdLevel")) {
			transform.position = new Vector3 (target.position.x + 5, transform.position.y, transform.position.z);
		} else {
			transform.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
		}

		if (move != null) {
			move.transform.position = new Vector3 (move.transform.position.x + diff, move.transform.position.y, move.transform.position.z);
		}
	}
}