using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    private PlayerController MyPlayer;
    [SerializeField] GameObject[] PlayerLifes;
    [SerializeField] Text BulletNumber;
    [SerializeField] Text BombNumber;
    [SerializeField] Text BalloonNumber;
    [SerializeField] Text KillNumber;
    [SerializeField] Text DeathNumber;


    // Use this for initialization
    void Start()
    {
        MyPlayer = transform.root.GetComponent<PlayerController>();
        MyPlayer.PlayerUI = this;
        AllUpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AllUpdatePlayerUI()
    {
        UpdateLife();
        UpdateBulletNumber();
        UpdateBombNumber();
        UpdateBalloonNumber();
        UpdateKillNumber();
        UpdateDeathNumber();
    }

    public void UpdateLife()
    {
        int playerLife = MyPlayer.playerHealth;
        for (int i = 0; i < PlayerLifes.Length; i++)
        {
            if (i < playerLife)
            {
                PlayerLifes[i].SetActive(true);

            }
            else
            {
                PlayerLifes[i].SetActive(false);
            }
        }
    }

    public void UpdateBulletNumber()
    {
        BulletNumber.text = MyPlayer.bulletNumber.ToString();
    }

    public void UpdateBombNumber()
    {
        BombNumber.text = MyPlayer.bombNumber.ToString();
    }

    public void UpdateBalloonNumber()
    {
        BalloonNumber.text = MyPlayer.balloonNumber.ToString();
    }

    public void UpdateKillNumber()
    {
        KillNumber.text = PlayerDataDirector.Instance.PlayerKills[MyPlayer.PlayerID - 1] + " Kill";
    }

    public void UpdateDeathNumber()
    {
        DeathNumber.text = PlayerDataDirector.Instance.PlayerDeaths[MyPlayer.PlayerID - 1] + " Death";
    }
}   