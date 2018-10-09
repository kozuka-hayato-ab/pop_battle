using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : Singleton<GameDirector>
{
    [SerializeField] private Vector3[] playerStartPosition;
    [SerializeField] private GameObject[] charactors;
    private PlayerController[] player;
    public PlayerController[] Player
    {
        get
        {
            return player;
        }
    }

    // Use this for initialization
    void Start()
    {
        player = new PlayerController[PlayerDataDirector.Instance.MaxPlayerNumber];
        GenerateAllPlayer();
    }

    // Update is called once per frame
    void Update()
    {

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
            playerStartPosition[playerIndex],
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
                if (participantsNumber == 2 || participantsNumber == 3)
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
                else if (participantsNumber == 3)
                {
                    camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 3:
                if (participantsNumber == 3)
                {
                    camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                }
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
}