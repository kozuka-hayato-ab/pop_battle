using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : Singleton<GameDirector>
{
    [SerializeField] private Vector3[] playerStartPosition;
    [SerializeField] private GameObject[] charactors;
    private GameObject[] player;
    public GameObject[] Player
    {
        get
        {
            return player;
        }
    }

    // Use this for initialization
    void Start()
    {
        GeneratePlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GeneratePlayer()
    {
        int participantsNumber = PlayerDataDirector.Instance.participantsNumber();
        player = new GameObject[participantsNumber];
        int playerIndex = 0;
        for (int i = 0; i < PlayerDataDirector.Instance.MaxPlayerNumber; i++)
        {
            if (PlayerDataDirector.Instance.PlayerTypes[i] != PlayerType.None)
            {
                player[playerIndex] = Instantiate(
                    charactors[(int)PlayerDataDirector.Instance.PlayerTypes[i] - 1],
                    playerStartPosition[i],
                    Quaternion.identity);

                player[playerIndex].SendMessage("ChangeGameController", i + 1);
                CameraViewSetting(playerIndex, participantsNumber);
                playerIndex++;
            }
        }
    }

    private void CameraViewSetting(int playerIndex,int participantsNumber)
    {
        Camera camera = player[playerIndex].transform.Find("CameraPivot").Find("Camera").GetComponent<Camera>();
        switch (playerIndex + 1)
        {
            case 1:
                if(participantsNumber == 2 || participantsNumber == 3)
                {
                    camera.rect = new Rect(0f, 0.5f, 1f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 2:
                if(participantsNumber == 2)
                {
                    camera.rect = new Rect(0f, 0f, 1f, 0.5f);
                }
                else if(participantsNumber == 3)
                {
                    camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 3:
                if(participantsNumber == 3)
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
}