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

    [SerializeField] Image CharactorImage;
    [SerializeField] Sprite[] charactors;
    
    private void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {
        charaChoiceStep = PlayerDataDirector.Instance.MaxPlayerNumber;
        PlayerNameChange(nowStep);
        CharactorImage.sprite = charactors[0];
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerCharaChoice(int charactorNumber)
    {
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
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
        CharactorImage.sprite = charactors[charactorNumber];
    }

    public void PlayerNameChange(int playerNumber)
    {
        playerName.text = playerNumber + "P Player";
    }

    public void NextStep()
    {
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
        nowStep++;
        if (nowStep == (charaChoiceStep + 1)) {
            nowStep--;
            if (PlayerDataDirector.Instance.participantsNumber() >= 2)
            {
                GameStart();
            }
        }
        PlayerNameChange(nowStep);
        CharactorImage.sprite =
            charactors[(int)PlayerDataDirector.Instance.PlayerTypes[nowStep -1]];
    }

    public void BackStep()
    {
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
        nowStep--;
        if(nowStep == 0)
        {
            nowStep++;
            BackToTitle();
            return;
        }
        PlayerNameChange(nowStep);
        CharactorImage.sprite =
           charactors[(int)PlayerDataDirector.Instance.PlayerTypes[nowStep - 1]];
    }

    public void BackToTitle()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        AudioManager.Instance.DestroySingleton();
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Main");
    }

}
