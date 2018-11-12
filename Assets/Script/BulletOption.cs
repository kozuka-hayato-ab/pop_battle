using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletOption : MonoBehaviour {
    private const int damageValue = 1;
    public int shotPlayerNumber
    {
        get;set;
    }

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
            ExecuteEvents.Execute<PlayerControllerRecieveInterface>(
                target: collision.transform.root.gameObject,
                eventData: null,
                functor: (reciever, y) => reciever.Damage(damageValue, shotPlayerNumber)
                );
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Stage")
        {
            Destroy(gameObject);
        }
    }
}
