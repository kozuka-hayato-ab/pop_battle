using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharaChoiceDirector : MonoBehaviour
{
    [SerializeField] Text playerName;
    private int charaChoiceStep;
    private int nowStep = 1;
    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        charaChoiceStep = PlayerDataDirector.Instance.MaxPlayerNumber;
        PlayerNameChange(nowStep);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerCharaChoice(int charactorNumber)
    {
        switch (charactorNumber) {
            case (int)PlayerType.None:
                PlayerDataDirector.Instance.PlayerTypes[nowStep - 1] = PlayerType.None;
                break;
            case (int)PlayerType.Charactor1:
                PlayerDataDirector.Instance.PlayerTypes[nowStep - 1] = PlayerType.Charactor1;
                break;
            case (int)PlayerType.Charactor2:
                PlayerDataDirector.Instance.PlayerTypes[nowStep - 1] = PlayerType.Charactor2;
                break;
            case (int)PlayerType.Charactor3:
                PlayerDataDirector.Instance.PlayerTypes[nowStep - 1] = PlayerType.Charactor3;
                break;
            case (int)PlayerType.Charactor4:
                PlayerDataDirector.Instance.PlayerTypes[nowStep - 1] = PlayerType.Charactor4;
                break;
        }
    }

    public void PlayerNameChange(int playerNumber)
    {
        playerName.text = playerNumber + "P Player";
    }

    public void NextStep()
    {
        nowStep++;
        if(nowStep == (charaChoiceStep + 1)){
            if (PlayerDataDirector.Instance.participantsNumber() >= 2) GameStart();
            else nowStep--;
        }
        PlayerNameChange(nowStep);
    }

    public void BackStep()
    {
        nowStep--;
        if(nowStep == 0)
        {
            BackToTitle();
        }
        PlayerNameChange(nowStep);
    }

    public void BackToTitle()
    {
        PlayerDataDirector.Instance.PlayerTypeInit();
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Main");
    }
}
