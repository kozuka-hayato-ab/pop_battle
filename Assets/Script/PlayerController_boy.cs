using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface PlayerControllerRecieveInterface_boy : IEventSystemHandler
{
    void Damage(int damageValue, int playerNumber);
}

public class PlayerController_boy : MonoBehaviour {
    
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

    public int bulletNumber { get; set; }
    public int bombNumber { get; set; }
    [SerializeField] float healthCureInterval;
    private bool healthCurePossible = true;
    private int healthCureValue = 2;//回復量

    [SerializeField] GameObject balloon;

    [SerializeField] GameObject myGun;
    [SerializeField] float bulletShotInterval;
    private bool bulletShotPossible = true;
    [SerializeField] float bombShotInterval;
    private bool bombShotPossible = true;

    public PlayerUI PlayerUI { get; set; }

    private GameObject WarpA;
    private GameObject WarpB;

    private bool isFlying;
    private bool enableFly;

    private void PlayerInfoInit()
    {
        playerHealth = maxHealth;
        bulletNumber = 30;
        bombNumber = 1;
    }

    private void Awake()
    {
        WarpB = GameObject.Find("Stage/warpB");
        nowFPS = false;
        characon = GetComponent<CharacterController>();
        animcon = GetComponent<Animator>();
        MynameUpdate();
        PlayerInfoInit();

        balloon.SetActive(false);

        TPS_pos = camera.transform.localPosition;//元のカメラの相対座標
        FPS_pos = new Vector3(0, 0.2f, -0.25f);//Player変えたら調節
        SwitchTPS = false;
        isFlying = false;
        enableFly = true; //飛行可能
    }


    //sec per frame が不安定
    void Update()
    {
        //視点操作　水平はプレイヤーの向きを変える　垂直はcamerapivotを回転
        transform.Rotate(new Vector3(0, Input.GetAxis(mynameForInputmanager + "CameraX") * playerLookSpeed * Time.deltaTime, 0), Space.Self);
        cameraVerticalAngel = Mathf.Clamp(cameraVerticalAngel + Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * Time.deltaTime, cameraVerticalUnderLimit, cameraVerticalUpperLimit);
        CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalAngel, CameraPivot.transform.eulerAngles.y, CameraPivot.transform.eulerAngles.z);

        //プレイヤー基準で移動方向を決める。
        var playerForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        float right = Input.GetAxis(mynameForInputmanager + "X");
        float forward = Input.GetAxis(mynameForInputmanager + "Y");
        Vector3 direction = playerForward * forward + transform.right.normalized * right;

        //animcon.SetFloat("Right", right);
        //animcon.SetFloat("Forward", forward);

        //playerが接地しているとき
        if (characon.isGrounded)
        {
            playerMoveDirection.y = 0f;
            playerMoveDirection = direction * playerSpeedValue;

            //3ボタンでJump
            if (Input.GetButtonDown(mynameForInputmanager + "Jump") && !isFlying)
            {
                playerMoveDirection.y = playerJumpValue;
            }
            else if(!isFlying)
            {
                playerMoveDirection.y -= gravityStrength * Time.deltaTime;
            }

            if(Input.GetButtonDown(mynameForInputmanager + "Aim"))
            {
                SwitchTPS = true;
            }

            if(Input.GetButtonUp(mynameForInputmanager + "Aim")){
                SwitchTPS = false; //バグ排除
                rate_switch = 0.0f;
                camera.transform.localPosition = TPS_pos;
            }
        }
        else if(!isFlying)
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

        if (Input.GetButtonDown(mynameForInputmanager + "Function2") || Input.GetKeyDown(KeyCode.H))
        {
            if(enableFly){
                enableFly = false;
                isFlying = true;
                balloon.SetActive(true);
            }
            else if(isFlying){
                isFlying = false;
                balloon.SetActive(false);
            }
        }

        if(isFlying)
        {
            Debug.Log("飛びます飛びます");
            playerMoveDirection.y += gravityStrength * 0.0001f;
            float now_y = playerMoveDirection.y;
            playerMoveDirection = direction * playerSpeedValue;
            playerMoveDirection.y = now_y;
        }

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

    //sec per Frameが固定
    private void FixedUpdate()
    {
        if(SwitchTPS){
            if(rate_switch <= 1){
                camera.transform.localPosition = Vector3.Lerp(TPS_pos, FPS_pos, rate_switch);
                rate_switch += switch_speed;
            }
            else{
                camera.transform.localPosition = FPS_pos;
                rate_switch = 0f;
                SwitchTPS = false;
            }

        }
        else{
            characon.Move(playerMoveDirection * Time.deltaTime);
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

    IEnumerator WaitCureHealthInterval()
    {
        yield return new WaitForSeconds(healthCureInterval);
        healthCurePossible = true;
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

    public void Damage(int damageValue)
    {
        playerHealth -= damageValue;
    }

    private void Death(int killerNumber)
    {
        //playerIndexであることに注意
        PlayerDataDirector.Instance.PlayerKills[killerNumber - 1] += 1;
        GameDirector.Instance.UpdateKillPlayerUI(killerNumber - 1);
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

    //当たり判定
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ワープゾーンとの当たり判定
        if (hit.gameObject.tag == "Warp")
        {
            Vector3 pos = WarpB.transform.position;
            pos.y += 0.5f;
            transform.position = pos;
        }
    }
}
