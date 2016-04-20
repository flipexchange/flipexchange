using UnityEngine;
using System.Collections;

public class bulletController : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
        // Debug.Log(col.collider.gameObject.name);
        // if (col.collider.gameObject.layer == 11 || col.collider.gameObject.layer == 10) { Destroy(gameObject); }
        // else  Destroy(gameObject);
    }
}
