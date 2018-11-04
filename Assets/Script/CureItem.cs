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
            PlayerController playerController = other.transform.root.GetComponent<PlayerController>();
            if(playerController.playerHealth < playerController.maxHealth)
            {
                playerController.SendMessage("HealthCure");
                Destroy(gameObject);
            }
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
