using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
    [SerializeField] GameObject BombParticle;
    [SerializeField] float destroyTime;
    public int playerID { get; set; }
    private Coroutine coroutine;

	// Use this for initialization
	void Start () {
        coroutine = StartCoroutine(DestroyTimer());
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
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        ExecuteBomb();
    }

    public void ExecuteBomb()
    {
        AudioManager.Instance.PlaySEClipFromIndex(8, 0.25f);
        Instantiate(BombParticle,
            transform.position,
            transform.rotation).GetComponent<BombOption>().ThrowPlayerID = playerID;
        Destroy(gameObject);
    }
}
