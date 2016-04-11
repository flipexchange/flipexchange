using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public Transform target;
	// Use this for initialization
	void Start () {
	}

	void Update() {
		transform.position = new Vector3(target.position.x,transform.position.y,transform.position.z);
	}
}