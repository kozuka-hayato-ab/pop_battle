using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    [SerializeField] GameObject BombParticle;
    [SerializeField] float destroyTime;
    public int playerID { get; set; }

	// Use this for initialization
	void Start () {
		
	}
	
    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        ExecuteBomb();
    }

	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        StopCoroutine(DestroyTimer());
        ExecuteBomb();
    }

    public void ExecuteBomb()
    {
        Instantiate(BombParticle,
            transform.position,
            transform.rotation).GetComponent<BombOption>().ThrowPlayerID = playerID;
        Destroy(gameObject);
    }
}
