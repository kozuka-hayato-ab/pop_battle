using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : Item {
    [SerializeField] private int bulletNumber;

    protected override void Awake()
    {
        base.Awake();
    }
    // Use this for initialization
    void Start () {
		
	}
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.root.gameObject.SendMessage("PickUpBullet", bulletNumber);
            Destroy(gameObject);
        }
    }
}
