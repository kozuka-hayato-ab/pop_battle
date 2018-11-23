using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureItem : Item{
    public int myPopPlaceIndex;
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
                ItemController.enablePopSpecificPlaces[myPopPlaceIndex] = true;
                Destroy(gameObject);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        base.onGroundTrue();
    }

    protected override void Update()
    {
        base.Update();
    }
}
