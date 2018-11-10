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
    [SerializeField] Image DamagePanel;
    private float alphaLimitValue;
    private const float maxAlphaValue = 0.4f;
    private float damageTime = 1.5f;
    private float colorChangeSpeed = 10f;
    public bool isDamaged { get; set; }
    private Coroutine damageCoroutine;

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
        if (isDamaged)
        {
            var color = DamagePanel.color;
            color.a = ((Mathf.Sin(Time.time * colorChangeSpeed) / 2f) + 0.5f) * alphaLimitValue;
            DamagePanel.color = color;
            Debug.Log(DamagePanel.color);
        }
    }

    public void onDamage()
    {
        DamagePanel.gameObject.SetActive(true);
        float playerHealthRatio = (float)(MyPlayer.maxHealth - MyPlayer.playerHealth) / MyPlayer.maxHealth;
        alphaLimitValue = Mathf.Clamp(playerHealthRatio * maxAlphaValue, 0.1f, 0.4f);
        isDamaged = true;
        UpdateLife();
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(WaitDamageTime());
    }

    IEnumerator WaitDamageTime()
    {
        yield return new WaitForSeconds(damageTime);
        isDamaged = false;
        var color = DamagePanel.color;
        color.a = 0f;
        DamagePanel.color = color;
        DamagePanel.gameObject.SetActive(false);
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