using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface PlayerControllerRecieveInterface : IEventSystemHandler
{
    void Damage(int damageValue, int playerNumber);
    void BombDamage(int damageValue, int throwPlayerID);
}

public class PlayerController : MonoBehaviour, PlayerControllerRecieveInterface
{
    [SerializeField] GameObject CameraPivot;
    [SerializeField] float cameraVerticalUnderLimit = -60f;
    [SerializeField] float cameraVerticalUpperLimit = 30f;
    private float cameraVerticalAngel;

    [SerializeField] Camera camera;
    private bool SwitchTPS;
    private Vector3 TPS_pos;//TPS視点の切り替え
    private Vector3 FPS_pos;
    private bool nowFPS;
    private float rate_switch = 0f;
    [SerializeField] float switch_speed;//[0, 1]の小数

    [SerializeField, Range(1, 4)] private int playerID;
    public int PlayerID
    {
        get
        {
            return playerID;
        }
    }

    private string mynameForInputmanager;
    private CharacterController characon;
    private Animator animcon;
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    const float halfNumber = 0.5f;
    private Vector3 playerMoveDirection = Vector3.zero;

    public int maxHealth
    {
        get
        {
            return 7;
        }
    }
    public int playerHealth { get; set; }
    [SerializeField] float playerSpeedValue = 5.0f;
    [SerializeField] float playerLookSpeed = 400f;
    [SerializeField] float cameraAngleSpeed = 400f;
    [SerializeField] float gravityStrength = 20f;
    [SerializeField] float playerJumpValue;
    [SerializeField] float flyingSpeed = 0.2f;
    private float fpsAngelSpeedRatio = 1f;
    [SerializeField] float fpsAngleSpeedValue = 0.6f;

    public int bulletNumber { get; set; }
    public int bombNumber { get; set; }
    public int balloonNumber { get; set; }

    [SerializeField] GameObject Balloon;

    [SerializeField] GameObject myGun;
    [SerializeField] float bulletShotInterval;
    private bool bulletShotPossible = true;
    [SerializeField] float bombShotInterval;
    private bool bombShotPossible = true;

    private bool bombDamageLimit = false;
    [SerializeField] float bombDamageLimitTime;

    public PlayerUI PlayerUI { get; set; }

    private bool isFlying;
    public bool EnableFly
    {
        get
        {
            bool enableFly;
            if (balloonNumber > 0)
            {
                enableFly = true;
            }
            else
            {
                enableFly = false;
            }
            return enableFly;
        }
    }

    [SerializeField] private float rayLength = 0.85f;
    private bool isGround = false;

    private void PlayerInfoInit()
    {
        playerHealth = maxHealth;
        bulletNumber = 30;
        bombNumber = 3;
        balloonNumber = 0;
    }
    private void Awake()
    {
        characon = GetComponent<CharacterController>();
        animcon = GetComponent<Animator>();
        MynameUpdate();
        PlayerInfoInit();
        TPS_pos = camera.transform.localPosition;//元のカメラの相対座標
        FPS_pos = new Vector3(-0.17f, 0.4f, 0f);//Player変えたら調節
        SwitchTPS = false;
        Balloon.SetActive(false);
        isFlying = false;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        //視点操作　水平はプレイヤーの向きを変える　垂直はcamerapivotを回転
        transform.Rotate(new Vector3(0, Input.GetAxis(mynameForInputmanager + "CameraX") * playerLookSpeed * fpsAngelSpeedRatio * Time.deltaTime, 0), Space.Self);
        cameraVerticalAngel = Mathf.Clamp(cameraVerticalAngel + Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * fpsAngelSpeedRatio * Time.deltaTime,
            cameraVerticalUnderLimit, cameraVerticalUpperLimit);
        CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalAngel, CameraPivot.transform.eulerAngles.y, CameraPivot.transform.eulerAngles.z);

        //プレイヤー基準で移動方向を決める。
        var playerForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        float right = Input.GetAxis(mynameForInputmanager + "X");
        float forward = Input.GetAxis(mynameForInputmanager + "Y");
        Vector3 direction = playerForward * forward + transform.right.normalized * right;

        if (!characon.isGrounded)
        {
            if (Physics.Linecast(transform.position, (transform.position - transform.up * rayLength)))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
            Debug.DrawLine(transform.position, (transform.position - transform.up * rayLength), Color.red);
        }

        if (characon.isGrounded)
        {
            playerMoveDirection.y = 0f;
            playerMoveDirection = direction * playerSpeedValue;

            if (Input.GetButtonDown(mynameForInputmanager + "Jump") && !isFlying)
            {
                playerMoveDirection.y = playerJumpValue;
            }
            else if (!isFlying)
            {
                playerMoveDirection.y -= gravityStrength * Time.deltaTime;
            }
        }
        else if (!isFlying)
        {
            playerMoveDirection.y -= gravityStrength * Time.deltaTime;
        }
        else //isFlying is true
        {
            playerMoveDirection = direction * playerSpeedValue;
            playerMoveDirection.y += gravityStrength * flyingSpeed;
        }

        if (Input.GetButtonDown(mynameForInputmanager + "Aim") || Input.GetKeyDown(KeyCode.Insert))
        {
            SwitchTPS = true;
            fpsAngelSpeedRatio = fpsAngleSpeedValue;
        }

        if (Input.GetButtonUp(mynameForInputmanager + "Aim") || Input.GetKeyUp(KeyCode.Insert))
        {
            SwitchTPS = false; //バグ排除
            rate_switch = 0.0f;
            camera.transform.localPosition = TPS_pos;
            fpsAngelSpeedRatio = 1f;
        }

        if ((Input.GetButton(mynameForInputmanager + "Shot2") || Input.GetKey(KeyCode.KeypadEnter)) && bulletShotPossible == true && bulletNumber > 0)
        {
            myGun.SendMessage("BulletShot");
            bulletNumber--;
            PlayerUI.UpdateBulletNumber();
            bulletShotPossible = false;
            StartCoroutine(WaitBulletShotInterval());
        }

        if ((Input.GetButton(mynameForInputmanager + "Shot1") || Input.GetKey(KeyCode.KeypadPlus)) && bombShotPossible == true && bombNumber > 0)
        {
            myGun.SendMessage("BombShot");
            bombNumber--;
            PlayerUI.UpdateBombNumber();
            bombShotPossible = false;
            StartCoroutine(WaitBombShotInterval());
        }

        if (Input.GetButtonDown(mynameForInputmanager + "Function2") || Input.GetKeyDown(KeyCode.H))
        {
            if (EnableFly && !isFlying)
            {
                balloonNumber--;
                PlayerUI.UpdateBalloonNumber();
                isFlying = true;
                Balloon.SetActive(true);
            }
            else
            {
                isFlying = false;
                Balloon.SetActive(false);
            }
        }

        characon.Move(playerMoveDirection * Time.deltaTime);

        if (!characon.isGrounded && !isGround)
        {
            animcon.SetBool("OnGround", false);
            animcon.SetFloat("Jump", playerMoveDirection.y);
        }
        else
        {
            animcon.SetBool("OnGround", true);
            float runCycle = Mathf.Repeat(
                animcon.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < halfNumber ? 1 : -1) * forward;
            animcon.SetFloat("JumpLeg", jumpLeg);
        }
        animcon.SetFloat("Forward", forward);
        animcon.SetFloat("Right", right);
    }

    private void FixedUpdate()
    {
        if (SwitchTPS)
        {
            if (rate_switch <= 1)
            {
                camera.transform.localPosition = Vector3.Lerp(TPS_pos, FPS_pos, rate_switch);
                rate_switch += switch_speed;
            }
            else
            {
                camera.transform.localPosition = FPS_pos;
                rate_switch = 0f;
                SwitchTPS = false;
            }

        }
    }

    IEnumerator WaitBulletShotInterval()
    {
        yield return new WaitForSeconds(bulletShotInterval);
        bulletShotPossible = true;
    }

    IEnumerator WaitBombShotInterval()
    {
        yield return new WaitForSeconds(bombShotInterval);
        bombShotPossible = true;
    }

    public void HealthCure()
    {
        if (playerHealth > 0 || playerHealth < maxHealth)
        {
            playerHealth = maxHealth;
        }
        PlayerUI.UpdateLife();
    }

    public void PickUpBullet(int bulletValue)
    {
        bulletNumber += bulletValue;
        PlayerUI.UpdateBulletNumber();
    }

    public void PickUpBomb(int bombValue)
    {
        bombNumber += bombValue;
        PlayerUI.UpdateBombNumber();
    }

    public void PickUpBalloon()
    {
        balloonNumber++;
        PlayerUI.UpdateBalloonNumber();
    }

    // shotPlayerNumber == 0 -> self damage
    public void Damage(int damageValue, int shotPlayerNumber)
    {
        playerHealth -= damageValue;
        PlayerUI.UpdateLife();
        if (playerHealth <= 0)
        {
            Death(shotPlayerNumber);
        }
    }

    public void BombDamage(int damageValue, int throwPlayerID)
    {
        Damage(damageValue, throwPlayerID);
    }

    private void Death(int killerNumber)
    {
        //playerIndexであることに注意
        if (killerNumber != 0) // 0 -> self death(stageout) not 0 -> other player kill
        {
            PlayerDataDirector.Instance.PlayerKills[killerNumber - 1] += 1;
            GameDirector.Instance.UpdateKillPlayerUI(killerNumber - 1);

        }
        PlayerDataDirector.Instance.PlayerDeaths[PlayerID - 1] += 1;
        GameDirector.Instance.GeneratePlayer(playerID - 1);
        Destroy(gameObject);
    }

    public void ChangeGameController(int id)
    {
        if (1 <= id && id <= 4) playerID = id;
        MynameUpdate();
    }

    public void MynameUpdate()
    {
        mynameForInputmanager = "Gamepad" + playerID + "_";
    }
}
