using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSceneDirector : MonoBehaviour {
    [SerializeField] Text[] playerRankText;
    [SerializeField] Text topPlayer;
    private void RankTextInit()
    {

        for(int i = 0;i < playerRankText.Length; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                int playerRank = PlayerDataDirector.Instance.PlayerRank[i];
                playerRankText[i].text = playerRank + "位";
                if(playerRank == 1)
                {
                    topPlayer.text += ((i + 1) + "Player ");
                }
            }
            else
                playerRankText[i].text = "None";
        }
    }
	// Use this for initialization
	void Start () {
        PlayerDataDirector.Instance.PlayerRankDecided();
        RankTextInit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
