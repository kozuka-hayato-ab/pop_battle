using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureItem : Item{
	// Use this for initialization
	void Start () {
		
	}
	
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.root.gameObject.SendMessage("HealthCure");
            Destroy(gameObject);
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }
}
