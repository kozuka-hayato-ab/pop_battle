using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface PlayerControllerRecieveInterface : IEventSystemHandler
{
    void Damage(int damageValue, int playerNumber);
}

public class PlayerController : MonoBehaviour, PlayerControllerRecieveInterface
{
    [SerializeField] GameObject CameraPivot;
    [SerializeField] float cameraVerticalUnderLimit = -60f;
    [SerializeField] float cameraVerticalUpperLimit = 30f;
    private float cameraVerticalAngel;

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

    public int bulletNumber { get; set; }
    public int bombNumber { get; set; }

    [SerializeField] GameObject myGun;
    [SerializeField] float bulletShotInterval;
    private bool bulletShotPossible = true;
    [SerializeField] float bombShotInterval;
    private bool bombShotPossible = true;

    public PlayerUI PlayerUI { get; set; }

    private void PlayerInfoInit()
    {
        playerHealth = 5;
        bulletNumber = 30;
        bombNumber = 1;
    }
    private void Awake()
    {
        characon = GetComponent<CharacterController>();
        animcon = GetComponent<Animator>();
        MynameUpdate();
        PlayerInfoInit();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //視点操作　水平はプレイヤーの向きを変える　垂直はcamerapivotを回転
        transform.Rotate(new Vector3(0, Input.GetAxis(mynameForInputmanager + "CameraX") * playerLookSpeed * Time.deltaTime, 0), Space.Self);
        cameraVerticalAngel = Mathf.Clamp(cameraVerticalAngel + Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * Time.deltaTime,
            cameraVerticalUnderLimit, cameraVerticalUpperLimit);
        CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalAngel, CameraPivot.transform.eulerAngles.y, CameraPivot.transform.eulerAngles.z);

        //プレイヤー基準で移動方向を決める。
        var playerForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        float right = Input.GetAxis(mynameForInputmanager + "X");
        float forward = Input.GetAxis(mynameForInputmanager + "Y");
        Vector3 direction = playerForward * forward + transform.right.normalized * right;

        if (characon.isGrounded)
        {
            playerMoveDirection.y = 0f;
            playerMoveDirection = direction * playerSpeedValue;

            if (Input.GetButtonDown(mynameForInputmanager + "Jump"))
            {
                playerMoveDirection.y = playerJumpValue;
            }
            else
            {
                playerMoveDirection.y -= gravityStrength * Time.deltaTime;
            }
        }
        else
        {
            playerMoveDirection.y -= gravityStrength * Time.deltaTime;
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

        characon.Move(playerMoveDirection * Time.deltaTime);


        if (!characon.isGrounded)
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
