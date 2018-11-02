using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageOutCollider : MonoBehaviour {
    [SerializeField] private int stageOutDamage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int playerID = other.GetComponent<PlayerController>().PlayerID;
            GameDirector.Instance.PlayerRespawn(playerID);
            ExecuteEvents.Execute<PlayerControllerRecieveInterface>(
                target: other.transform.root.gameObject,
                eventData: null,
                functor: (reciever, y) => reciever.Damage(stageOutDamage, 0)
                );
        }
        if (other.tag == "OutsideStage")
        {
            Destroy(other.gameObject);
        }
    }
}
