using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject CameraPivot;
    [SerializeField] float cameraVerticalUnderLimit = -60f;
    [SerializeField] float cameraVerticalUpperLimit = 30f;
    private float cameraVerticalAngel;

    [SerializeField, Range(1,4)] private int playerID;
    private string mynameForInputmanager;
    private CharacterController characon;
    private Animator animcon;
    private Vector3 playerMoveDirection = Vector3.zero;

    private const int maxHealth = 10;
    [SerializeField] int playerHealth = maxHealth;
    [SerializeField] float playerSpeedValue = 5.0f;
    [SerializeField] float playerLookSpeed = 400f;
    [SerializeField] float cameraAngleSpeed = 400f;
    [SerializeField] float gravityStrength = 20f;
    [SerializeField] float abilityOfItemGetValue = 1.5f;
    [SerializeField] float playerJumpValue;

    [SerializeField] int cureItemNumber = 1;
    [SerializeField] int bulletNumber = 15;
    [SerializeField] int bombNumber = 3;
    [SerializeField] float healthCureInterval;
    private bool healthCurePossible = true;
    private int healthCureValue = 2;//回復量
    
    [SerializeField] GameObject myGun;
    [SerializeField] float bulletShotInterval;
    private bool bulletShotPossible = true;
    [SerializeField] float bombShotInterval;
    private bool bombShotPossible = true;

    private void Awake()
    {
        characon = GetComponent<CharacterController>();
        animcon = GetComponent<Animator>();
        mynameForInputmanager = "Gamepad" + playerID + "_";
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
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

        //animcon.SetFloat("Right", right);
        animcon.SetFloat("Forward", forward);

        if (characon.isGrounded)
        {
            playerMoveDirection.y = 0f;
            playerMoveDirection = direction * playerSpeedValue;

            if(Input.GetButtonDown(mynameForInputmanager + "Jump"))
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


        if((Input.GetButton(mynameForInputmanager + "Shot2") || Input.GetKey(KeyCode.KeypadEnter)) && bulletShotPossible == true && bulletNumber > 0)
        {
            myGun.SendMessage("BulletShot");
            bulletNumber--;
            bulletShotPossible = false;
            StartCoroutine(WaitBulletShotInterval());
        }

        if((Input.GetButton(mynameForInputmanager + "Shot1") || Input.GetKey(KeyCode.KeypadPlus)) && bombShotPossible == true && bombNumber > 0)
        {
            myGun.SendMessage("BombShot");
            bombNumber--;
            bombShotPossible = false;
            StartCoroutine(WaitBombShotInterval());
        }

        if ((Input.GetButtonDown(mynameForInputmanager + "Function2") || Input.GetKeyDown(KeyCode.H)) && healthCurePossible == true && cureItemNumber > 0)
        {
            cureItemNumber--;
            HealthCure(healthCureValue);
            healthCurePossible = false;
            StartCoroutine(WaitCureHealthInterval());
        }

        characon.Move(playerMoveDirection * Time.deltaTime);
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

    public void HealthCure(int healthCureValue)
    {
        if(playerHealth > 0 || playerHealth < 10)
        {
            playerHealth += healthCureValue;
            if (playerHealth > 10) playerHealth = 10;
        }
    }

    public void PickUpBullet(int bulletValue)
    {
        bulletNumber += bulletValue;
    }

    public void PickUpBomb(int bombValue)
    {
        bombNumber += bombValue;
    }

    public void PickUpCureItem(int cureItemValue)
    {
        cureItemNumber += cureItemValue;
    }

    public void Damage(int damageValue)
    {
        playerHealth -= damageValue;
    }

    public void ChangeGameController(int id)
    {
        if(1 <= id && id <= 4) playerID = id;
    }

}
