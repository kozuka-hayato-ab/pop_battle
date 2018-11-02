using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] GameObject WarpExit;
    [SerializeField] float heightValue;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 pos = WarpExit.transform.position;
            pos.y += heightValue;
            other.transform.position = pos;
        }
    }
}
