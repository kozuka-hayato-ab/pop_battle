using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneDirector : MonoBehaviour
{
    [SerializeField] Text[] playerRankText;
    [SerializeField] Image[] playerImages;
    [SerializeField] Sprite[] charactors;
    [SerializeField] Text topPlayer;

    [SerializeField] float[] waitTimeAtRankAnnounce;
    [SerializeField] GameObject[] RankHideParticles;
    private int[,] RankToPlayerIndexArray;

    private void RankInit()
    {

        for (int i = 0; i < playerRankText.Length; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                int playerRank = PlayerDataDirector.Instance.PlayerRank[i];
                playerRankText[i].text = playerRank + "位";
                if (playerRank == 1)
                {
                    topPlayer.text += ((i + 1) + "Player ");
                }
            }
            else
            {
                playerRankText[i].text = "None";
                RankHideParticles[i].SetActive(false);
            }
        }
    }
    private void PlayerImageInit()
    {
        for (int i = 0; i < playerImages.Length; i++)
        {
            playerImages[i].sprite = charactors[(int)PlayerDataDirector.Instance.PlayerTypes[i]];
        }
    }

    private int[,] playerRankToPlayerIndex()
    {
        int participants = PlayerDataDirector.Instance.participantsNumber();
        int[,] array = new int[participants, participants];
        for (int i = 0; i < participants; i++)
        {
            for (int k = 0; k < participants; k++)
            {
                array[i, k] = -1;
            }
        }
        int maxPlayer = PlayerDataDirector.Instance.MaxPlayerNumber;
        for (int i = 0; i < maxPlayer; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                for (int k = 0; k < participants; k++)
                {
                    if (array[PlayerDataDirector.Instance.PlayerRank[i] - 1, k] == -1)
                    {
                        array[PlayerDataDirector.Instance.PlayerRank[i] - 1, k] = i;
                        break;
                    }
                }
            }
        }
        return array;
    }

    private bool rankAnnounceFinished = false;

    // Use this for initialization
    void Start()
    {
        PlayerDataDirector.Instance.PlayerRankDecided();
        //tmp();
        RankToPlayerIndexArray = playerRankToPlayerIndex();
        PlayerImageInit();
        StartCoroutine(RankAnnounce());
    }

    // Update is called once per frame
    void Update()
    {
        if (rankAnnounceFinished)
        {
            if (Input.anyKeyDown)
            {
                BackToTitle();
            }
        }
    }

    private void BackToTitle()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        AudioManager.Instance.DestroySingleton();
        SceneManager.LoadScene("Title");
    }

    IEnumerator RankAnnounce()
    {
        yield return new WaitForSecondsRealtime(3f);
        RankInit();
        for (int i = RankToPlayerIndexArray.GetLength(0) - 1; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(waitTimeAtRankAnnounce[i]);
            for(int k = 0; k < RankToPlayerIndexArray.GetLength(1); k++)
            {
                if (RankToPlayerIndexArray[i, k] != -1)
                    RankHideParticles[RankToPlayerIndexArray[i, k]].SetActive(false);
            }
        }
        yield return new WaitForSecondsRealtime(1f);
        topPlayer.gameObject.SetActive(true);
        rankAnnounceFinished = true;
    }

    // for debug
    /*
    private void tmp()
    {
        PlayerDataDirector.Instance.PlayerTypes[0] = PlayerType.Charactor3;
        PlayerDataDirector.Instance.PlayerTypes[1] = PlayerType.Charactor2;
        PlayerDataDirector.Instance.PlayerTypes[2] = PlayerType.Charactor1;
        PlayerDataDirector.Instance.PlayerTypes[3] = PlayerType.Charactor4;

        PlayerDataDirector.Instance.PlayerRank[0] = 4;
        PlayerDataDirector.Instance.PlayerRank[1] = 2;
        PlayerDataDirector.Instance.PlayerRank[2] = 3;
        PlayerDataDirector.Instance.PlayerRank[3] = 1;
    }*/
    /* for debug
    void debuglogArray()
    {
        string debug = "";
        for (int i = 0; i < RankToPlayerIndexArray.GetLength(0); i++)
        {
            for (int k = 0; k < RankToPlayerIndexArray.GetLength(1); k++)
            {
                debug += RankToPlayerIndexArray[i, k] + " ";
            }
            debug += "\n";
        }
        Debug.Log(debug);
    }*/
}