using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, Time.deltaTime*10, 0f);
	}
}
