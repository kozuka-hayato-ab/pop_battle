using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType
{
    None,
    Charactor1,
    Charactor2,
    Charactor3,
    Charactor4
}
public class PlayerDataDirector : Singleton<PlayerDataDirector> {

    private const int maxPlayerNumber = 4;
    public int MaxPlayerNumber
    {
        get
        {
            return maxPlayerNumber;
        }
    }

    public int participantsNumber()
    {
        int number = 0;
        for (int i = 0; i < PlayerTypes.Length; i++)
        {
            if (PlayerTypes[i] != PlayerType.None) number++;
        }
        return number;
    }

    public PlayerType[] PlayerTypes { get; private set; }

    public void PlayerTypeInit()
    {
        PlayerTypes = new PlayerType[MaxPlayerNumber];
        for(int i = 0;i < PlayerTypes.Length; i++)
        {
            PlayerTypes[i] = PlayerType.None;
        }
    }

    public int[] PlayerKills { get; set; }

    public void PlayerKillInit()
    {
        PlayerKills = new int[MaxPlayerNumber];
        for(int i = 0;i < PlayerKills.Length; i++)
        {
            PlayerKills[i] = 0;
        }
    }

    public int[] PlayerDeaths { get; set; }

    public void PlayerDeathInit()
    {
        PlayerDeaths = new int[MaxPlayerNumber];
        for(int i = 0; i < PlayerDeaths.Length; i++)
        {
            PlayerDeaths[i] = 0;
        }
    }

    public int[] PlayerRank { get; set; }

    public void PlayerRankInit()
    {
        PlayerRank = new int[MaxPlayerNumber];
        for(int i = 0;i < PlayerRank.Length; i++)
        {
            PlayerRank[i] = -1;
        }
    }

    public void PlayerRankDecided()
    {
        int[] playerScore = new int[MaxPlayerNumber];
        int[] rank = new int[MaxPlayerNumber];
        for(int i = 0;i < MaxPlayerNumber; i++)
        {
            playerScore[i] = PlayerKills[i] - PlayerDeaths[i];
            rank[i] = i;
        }
        for(int i = rank.Length - 1;i > 0; i--)
        {
            for(int k = 0;k < i; k++)
            {
                if(playerScore[rank[k]] < playerScore[rank[k + 1]])
                {
                    int tmp = rank[k];
                    rank[k] = rank[k + 1];
                    rank[k + 1] = tmp;
                }
            }
        }
        int tmpRank = 1;
        for(int i = 0;i < rank.Length; i++)
        {
            if(PlayerTypes[rank[i]] != PlayerType.None)
            {
                PlayerRank[rank[i]] = tmpRank;
                tmpRank++;
            }
        }
    }
    
    //PlayerRankDecidedのデバッグ用
    /*
    [SerializeField] int[] killAndDeath = new int[8];

    public void inputKillDeath()
    {
        for(int i = 0;i < MaxPlayerNumber; i++)
        {
            PlayerKills[i] = killAndDeath[i * 2];
            PlayerDeaths[i] = killAndDeath[i * 2 + 1];
        }
    }

    public void OutputPlayerRank()
    {
        for(int i = 0;i < PlayerRank.Length; i++)
        {
            Debug.Log((i + 1) + "PlayerRank is " + PlayerRank[i] +
                "\nPlayerkill" + PlayerKills[i] +
                "\nPlayerDeath" + PlayerDeaths[i]
                );
        }
    }
    */
    //until this

    private new void Awake()
    {
        base.Awake();
        PlayerTypeInit();
        PlayerKillInit();
        PlayerDeathInit();
        PlayerRankInit();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        /*
        for (int i = 0; i < PlayerTypes.Length; i++)
        {
            Debug.Log("player" + (i + 1) + PlayerKills[i]);
        }
        */
    }


}
