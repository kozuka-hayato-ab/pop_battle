using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultSceneDirector : MonoBehaviour
{
    [SerializeField] Image[] playerImages;
    [SerializeField] Image[] playerRank;
    [SerializeField] Sprite[] charactors;
    [SerializeField] Sprite[] rankImages;
    [SerializeField] Text finishText;
    [SerializeField] Text[] playerKD;

    [SerializeField] float[] waitTimeAtRankAnnounce;
    private int[,] RankToPlayerIndexArray;
    // botu idea 
    /*
    private void PlayerTextInit()
    {

        for (int i = 0; i < playerRankText.Length; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                int playerRank = PlayerDataDirector.Instance.PlayerRank[i];
                playerRankText[i].text = playerRank + "位";
                if (playerRank == 1)
                {
                    finishText.text += ((i + 1) + "Player ");
                }
                playerKD[i].text = PlayerDataDirector.Instance.PlayerKills[i].ToString() + "Kill\n" +
                    PlayerDataDirector.Instance.PlayerDeaths[i].ToString() + "Death";
            }
            else
            {
                playerRankText[i].text = "None";
                playerKD[i].text = "";
                RankHideParticles[i].SetActive(false);
            }
        }
    }*/

    private void DisplayPlayerInfoFromRank(int rankIndex)
    {
        for (int i = 0; i < RankToPlayerIndexArray.GetLength(1); i++)
        {
            int playerIndex = RankToPlayerIndexArray[rankIndex, i];
            if (playerIndex != -1)
            {
                playerRank[playerIndex].sprite = rankImages[rankIndex];
                playerKD[playerIndex].text = PlayerDataDirector.Instance.PlayerKills[playerIndex].ToString() + "\n" +
                    PlayerDataDirector.Instance.PlayerDeaths[playerIndex].ToString();
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

    private readonly float textFlashSpeed = 5f;

    // Use this for initialization
    void Start()
    {
        PlayerDataDirector.Instance.PlayerRankDecided();
        tmp();
        RankToPlayerIndexArray = playerRankToPlayerIndex();
        PlayerImageInit();
        AudioManager.Instance.ChangeBGM(0);
        StartCoroutine(RankAnnounce());
    }

    // Update is called once per frame
    void Update()
    {
        if (rankAnnounceFinished)
        {
            var color = finishText.color;
            color.a = Mathf.Sin(Time.time * textFlashSpeed) / 2 + 0.6f;
            finishText.color = color;
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
        for (int i = RankToPlayerIndexArray.GetLength(0) - 1; i >= 0; i--)
        {
            yield return new WaitForSecondsRealtime(waitTimeAtRankAnnounce[i]);
            if (RankToPlayerIndexArray[i, 0] != -1)
                DisplayPlayerInfoFromRank(i);
        }
        yield return new WaitForSecondsRealtime(1f);
        finishText.gameObject.SetActive(true);
        rankAnnounceFinished = true;
        AudioManager.Instance.StopBGM();
    }

    // for debug
    
    [SerializeField] int[] rankNumberForDebug;
    [SerializeField] PlayerType[] playerType;
    private void tmp()
    {
        PlayerDataDirector.Instance.PlayerTypes[0] = playerType[0];
        PlayerDataDirector.Instance.PlayerTypes[1] = playerType[1];
        PlayerDataDirector.Instance.PlayerTypes[2] = playerType[2];
        PlayerDataDirector.Instance.PlayerTypes[3] = playerType[3];

        PlayerDataDirector.Instance.PlayerRank[0] = rankNumberForDebug[0];
        PlayerDataDirector.Instance.PlayerRank[1] = rankNumberForDebug[1];
        PlayerDataDirector.Instance.PlayerRank[2] = rankNumberForDebug[2];
        PlayerDataDirector.Instance.PlayerRank[3] = rankNumberForDebug[3];

        PlayerDataDirector.Instance.PlayerKills[0] = 12;
        PlayerDataDirector.Instance.PlayerKills[1] = 4;
        PlayerDataDirector.Instance.PlayerKills[2] = 6;
        PlayerDataDirector.Instance.PlayerKills[3] = 21;

        PlayerDataDirector.Instance.PlayerDeaths[0] = 2;
        PlayerDataDirector.Instance.PlayerDeaths[1] = 1;
        PlayerDataDirector.Instance.PlayerDeaths[2] = 5;
        PlayerDataDirector.Instance.PlayerDeaths[3] = 2;

    }
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