using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpExit : MonoBehaviour
{
    private int playerCount = 0;
    public bool enableWarp = true;
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
            playerCount++;
            enableWarp = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerCount--;
            if (playerCount == 0) enableWarp = true;
        }
    }
}
