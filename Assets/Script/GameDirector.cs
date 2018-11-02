using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : Singleton<GameDirector>
{
    public Vector3[] PlayerStartPosition;
    public Vector3[] PlayerLatterStartPosition;
    public bool GameIsLatter { get; set; }

    [SerializeField] private GameObject[] charactors;
    private PlayerController[] player;
    public PlayerController[] Player
    {
        get
        {
            return player;
        }
    }

    private float totalTime;
    [SerializeField] private int limitTimeMinutes = 3;
    [SerializeField] private float limitTimeSeconds = 0;
    private float preSeconds;
    [SerializeField] private float startTime = 3;
    [SerializeField] private int latterTimeMinutes;
    [SerializeField] private int latterTimeSeconds;
    private bool isGameStart;

    [SerializeField] Image GamePanel;
    [SerializeField] Text GameTimer;
    [SerializeField] Text CenterTimer;

    // Use this for initialization
    void Start()
    {
        player = new PlayerController[PlayerDataDirector.Instance.MaxPlayerNumber];
        GenerateAllPlayer();

        preSeconds = 0f;
        GamePanel.gameObject.SetActive(true);
        CenterTimer.gameObject.SetActive(true);
        isGameStart = false;
        GameIsLatter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStart == true)
        {
            if (totalTime <= 0f) return;
            totalTime = limitTimeMinutes * 60 + limitTimeSeconds;
            totalTime -= Time.deltaTime;

            limitTimeMinutes = (int)totalTime / 60;
            limitTimeSeconds = totalTime - limitTimeMinutes * 60;

            if ((int)limitTimeSeconds != (int)preSeconds)
            {
                GameTimer.text = limitTimeMinutes.ToString("00") + " : " +
                    ((int)limitTimeSeconds).ToString("00");
            }

            preSeconds = limitTimeSeconds;

            if (totalTime <= latterTimeMinutes * 60 + latterTimeSeconds)
                GameIsLatter = true;

            if (totalTime <= 0f)
            {
                GameFinish();
            }
        }
        else
        {
            if (startTime <= 0f) return;

            startTime -= Time.deltaTime;

            if ((int)startTime != (int)preSeconds)
            {
                CenterTimer.text = ((int)startTime).ToString();
            }

            preSeconds = startTime;

            if (startTime <= 0f)
            {
                GameStart();
            }
        }
    }

    private void GameStart()
    {
        totalTime = limitTimeMinutes * 60 + limitTimeSeconds;
        preSeconds = 0f;
        GamePanel.gameObject.SetActive(false);
        CenterTimer.gameObject.SetActive(false);
        isGameStart = true;
    }

    private void GameFinish()
    {
        SceneManager.LoadScene("Result");
        DestroySingleton();
    }

    private void GenerateAllPlayer()
    {
        for (int i = 0; i < PlayerDataDirector.Instance.MaxPlayerNumber; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                GeneratePlayer(i);
            }
        }
    }

    //プレイヤー番号、何PなどはPlayerNumber
    //PlayerIndexとはプレイヤーのデータの配列のインデックス
    public void GeneratePlayer(int playerIndex)
    {
        player[playerIndex] = Instantiate(
            charactors[(int)PlayerDataDirector.Instance.PlayerTypes[playerIndex] - 1],//enum型の先頭はNoneのため
            PlayerStartPosition[playerIndex],
            Quaternion.identity).GetComponent<PlayerController>();

        player[playerIndex].SendMessage("ChangeGameController", playerIndex + 1);
        CameraViewSetting(playerIndex, PlayerDataDirector.Instance.participantsNumber());
    }

    private void CameraViewSetting(int playerIndex, int participantsNumber)
    {
        Camera camera = player[playerIndex].transform.Find("CameraPivot").Find("Camera").GetComponent<Camera>();
        switch (playerIndex + 1)
        {
            case 1:
                if (participantsNumber == 2)
                {
                    camera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 2:
                if (participantsNumber == 2)
                {
                    camera.rect = new Rect(0f, 0f, 1f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 3:
                camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                break;
            case 4:
                camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                break;
        }
    }

    public void UpdateKillPlayerUI(int playerIndex)
    {
        Player[playerIndex].PlayerUI.UpdateKillNumber();
    }

    public void PlayerRespawn(int playerID)
    {
        if (GameIsLatter)
        {
            Player[playerID - 1].transform.position = PlayerLatterStartPosition[playerID - 1];
        }
        else
        {
            Player[playerID - 1].transform.position = PlayerStartPosition[playerID - 1];
        }
    }
}