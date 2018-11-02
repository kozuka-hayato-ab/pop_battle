﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDirector : MonoBehaviour {
    [SerializeField] GameObject OutsideStage;
    [SerializeField] float fallSpeed;
    [SerializeField] float fallPrepareSpeed;
    [SerializeField] float verticalMoveValue;
    public bool isFallTime = false;
    public bool isFallPrepareTime = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isFallPrepareTime && !isFallTime)
        {
            FallPrepareMove();
        }
        else if(isFallTime)
        {
            FallMove();
        }
	}

    private void FallMove()
    {
        transform.position -= fallSpeed * Vector3.up * Time.deltaTime;
    }

    private void FallPrepareMove()
    {
        transform.position += Mathf.Sin(Time.time * fallPrepareSpeed) * verticalMoveValue * Vector3.up * Time.deltaTime;
    }
}