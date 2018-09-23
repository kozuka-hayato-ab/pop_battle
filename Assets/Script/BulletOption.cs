using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOption : MonoBehaviour {
    [SerializeField] private const int damageValue = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.transform.root.gameObject.SendMessage("Damage", damageValue);
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Stage")
        {
            Destroy(gameObject);
        }
    }
}
