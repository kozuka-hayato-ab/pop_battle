using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureItem : MonoBehaviour {
    [SerializeField] private int cureItemValue = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.root.gameObject.SendMessage("PickUpCureItem", cureItemValue);
            Destroy(gameObject);
        }
    }
}
