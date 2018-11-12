using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    [SerializeField] GameObject WarpExit;
    [SerializeField] float heightValue;
    private WarpExit WarpExitScript;

    List<PlayerController> playerList;

    private void Awake()
    {
        playerList = new List<PlayerController>();
    }
    // Use this for initialization
    void Start()
    {
        WarpExitScript = WarpExit.GetComponent<WarpExit>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerList.Count > 0 && WarpExitScript.enableWarp)
        {
            foreach(var player in playerList)
            {
                if (Input.GetButtonDown(player.MynameForInputmanager + "Function1") || Input.GetKeyDown(KeyCode.L))
                {
                    WarpPlayer(player);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerList.Add(other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            playerList.RemoveAt(playerList.FindIndex(player => player.gameObject == other.gameObject));
        }
    }

    private void WarpPlayer(PlayerController player)
    {
        Vector3 pos = WarpExit.transform.position;
        pos.y += heightValue;
        player.transform.position = pos;
    }
}
