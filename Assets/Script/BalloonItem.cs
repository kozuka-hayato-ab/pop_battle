using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonItem : Item{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
    }

    // Use this for initialization
    void Start () {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.root.gameObject.SendMessage("PickUpBalloon");
            Destroy(gameObject);
        }
    }


}
