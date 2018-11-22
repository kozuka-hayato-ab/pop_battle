using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    private float myDestroyTime = 1f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, myDestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
