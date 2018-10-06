using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BulletOption : MonoBehaviour {
    [SerializeField] private const int damageValue = 1;
    [Range(1,4)] private int shotPlayerNumber;
    public void ChangeShotPlayerNumber(int playerNumber)
    {
        shotPlayerNumber = playerNumber;
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
