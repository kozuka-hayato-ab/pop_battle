using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameDirector : Singleton<GameDirector>
{
    public Vector3[] PlayerStartPosition;
    public Vector3[] PlayerLatterStartPosition;

    public bool GameIsLatter { get; set; }
    [SerializeField] StageDirector OutsideStage;

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
    private bool isPoseTime = false;
    [SerializeField] GameObject PoseMenu;
    [SerializeField] Text PoseText;
    [SerializeField] float textFlashSpeed;
    [SerializeField] GameObject firstSelectedButton;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] Text KillLogText;
    [SerializeField] GameObject KillLogPanel;
    private float killLogDisplayTime = 3f;
    private Coroutine killLogCoroutine;

    private bool exist_primary; //全員が二人対戦できるようにする変数

    // Use this for initialization
    void Start()
    {
        exist_primary = false;
        GameIsLatter = false;
        player = new PlayerController[PlayerDataDirector.Instance.MaxPlayerNumber];
        GenerateAllPlayer();
        preSeconds = 0f;
        GamePanel.gameObject.SetActive(true);
        CenterTimer.gameObject.SetActive(true);
        KillLogText.gameObject.SetActive(false);
        isGameStart = false;
        AudioManager.Instance.PlaySEClipFromIndex(2, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPoseTime)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Option"))
            {
                isPoseTime = true;
                PoseMenu.SetActive(true);
                GamePanel.gameObject.SetActive(true);
                eventSystem.SetSelectedGameObject(firstSelectedButton);
                Time.timeScale = 0f;
            }
        }
        else
        {
            var color = PoseText.color;
            color.a = Mathf.Sin(Time.realtimeSinceStartup * textFlashSpeed) / 2 + 0.6f;
            PoseText.color = color;
        }

        if (Time.timeScale == 0)
        {
            return;
        }

        if (isGameStart == true)
        {
            if (totalTime <= 0f) return;
            totalTime = limitTimeMinutes * 60 + limitTimeSeconds;
            totalTime -= Time.deltaTime;

            if (!GameIsLatter)
                if (totalTime <= latterTimeMinutes * 60 + latterTimeSeconds)
                {
                    GameIsLatter = true;
                    OutsideStage.StageFallStart();
                }

            limitTimeMinutes = (int)totalTime / 60;
            limitTimeSeconds = totalTime - limitTimeMinutes * 60;

            if ((int)limitTimeSeconds != (int)preSeconds)
            {
                GameTimer.text = limitTimeMinutes.ToString("00") + " : " +
                    ((int)limitTimeSeconds).ToString("00");
            }

            preSeconds = limitTimeSeconds;

            if (totalTime <= 0f)
            {
                AudioManager.Instance.PlaySEClipFromIndex(3, 1f);
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

    public void GameFinishFromPose()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        AudioManager.Instance.DestroySingleton();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
        DestroySingleton();
    }

    public void QuitGameFromPose()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        AudioManager.Instance.DestroySingleton();
        Time.timeScale = 1f;
        GameDirector.Instance.SingletonReset();
        Application.Quit();
    }

    public void BackFromPose()
    {
        PoseMenu.SetActive(false);
        if (isGameStart)
        {
            GamePanel.gameObject.SetActive(false);
        }
        Time.timeScale = 1f;
        isPoseTime = false;
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
        Vector3 respawnPlace;
        if (GameIsLatter)
            respawnPlace = PlayerLatterStartPosition[playerIndex];
        else
            respawnPlace = PlayerStartPosition[playerIndex];

        player[playerIndex] = Instantiate(
            charactors[(int)PlayerDataDirector.Instance.PlayerTypes[playerIndex] - 1],//enum型の先頭はNoneのため
            respawnPlace,
            Quaternion.identity).GetComponent<PlayerController>();

        player[playerIndex].SendMessage("ChangeGameController", playerIndex + 1);
        CameraViewSetting(playerIndex, PlayerDataDirector.Instance.participantsNumber());
    }

    private void CameraViewSetting(int playerIndex, int participantsNumber)
    {
        Camera camera = player[playerIndex].transform.Find("CameraPivot").Find("Camera").GetComponent<Camera>();
        bool isOnlyTwoPlayer = (participantsNumber == 2);
        switch (playerIndex + 1)
        {
            case 1://
                if (isOnlyTwoPlayer)
                {
                    camera.rect = new Rect(0.15f, 0.5f, 0.7f, 0.5f);
                    exist_primary = true;
                }
                else
                {
                    camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 2://
                if (isOnlyTwoPlayer && exist_primary)
                {
                    camera.rect = new Rect(0.15f, 0f, 0.7f, 0.5f);
                }
                else if(isOnlyTwoPlayer){
                    camera.rect = new Rect(0.15f, 0.5f, 0.7f, 0.5f);
                    exist_primary = true;
                }
                else
                {
                    camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                }
                break;
            case 3://
                if (isOnlyTwoPlayer && exist_primary)
                {
                    camera.rect = new Rect(0.15f, 0f, 0.7f, 0.5f);
                }
                else if (isOnlyTwoPlayer)
                {
                    camera.rect = new Rect(0.15f, 0.5f, 0.7f, 0.5f);
                    exist_primary = true;
                }
                else
                {
                    camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                }
                break;
            case 4://
                if (isOnlyTwoPlayer)
                {
                    camera.rect = new Rect(0.15f, 0f, 0.7f, 0.5f);
                }
                else
                {
                    camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                }
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

    public void DisplayKillLog(int killerID, int deathID)
    {
        if(killLogCoroutine != null)
        {
            StopCoroutine(killLogCoroutine);
        }
        KillLogPanel.SetActive(true);
        if(killerID != 0)
        {
            KillLogText.text = killerID + "P       " + deathID + "P";
        }
        else
        {
            KillLogText.text = deathID + "P       " + deathID + "P";
        }
        killLogCoroutine = StartCoroutine(WaitKillLogDisplayTime());
    }

    IEnumerator WaitKillLogDisplayTime()
    {
        yield return new WaitForSeconds(killLogDisplayTime);
        KillLogText.text = "";
        KillLogText.gameObject.SetActive(false);
    }
}