﻿using UnityEngine;
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
        Debug.Log(col.collider.gameObject.layer);
        if (col.collider.gameObject.layer != 11) {
            Debug.Log("COLLIDE");
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
