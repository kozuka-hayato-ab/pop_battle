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

    [SerializeField] int playerHealth = 10;
    [SerializeField] float playerSpeedValue = 5.0f;
    [SerializeField] float playerLookSpeed = 400f;
    [SerializeField] float cameraAngleSpeed = 400f;
    [SerializeField] float gravityStrength = 20f;
    [SerializeField] float abilityOfItemGetValue = 1.5f;
    [SerializeField] float playerJumpValue;

    private void Awake()
    {
        characon = GetComponent<CharacterController>();
        animcon = GetComponent<Animator>();
    }
    // Use this for initialization
    void Start () {
        mynameForInputmanager = "Gamepad" + playerID + "_";
	}
	
	// Update is called once per frame
	void Update () {
        
        characon.Move(playerMoveDirection * Time.deltaTime);
        transform.Rotate(new Vector3(0, Input.GetAxis(mynameForInputmanager + "CameraX") * playerLookSpeed * Time.deltaTime, 0), Space.Self);
        /*
        float CampivRotationX = CameraPivot.transform.eulerAngles.x;
        if (CampivRotationX > cameraVerticalUpperLimit)
        {
            CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalUpperLimit,0, 0);
        }
        else if (CampivRotationX < cameraVerticalUnderLimit)
        {
            CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalUnderLimit,0, 0);
        }
        else
        {
            CameraPivot.transform.Rotate(new Vector3(Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * Time.deltaTime, 0, 0), Space.Self);
        }
        */
        
        cameraVerticalAngel = Mathf.Clamp(cameraVerticalAngel + Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * Time.deltaTime,
            cameraVerticalUnderLimit, cameraVerticalUpperLimit);
        CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalAngel, CameraPivot.transform.eulerAngles.y, CameraPivot.transform.eulerAngles.z);
        
        //プレイヤー基準で向きを決める。
        var playerForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 direction = playerForward * Input.GetAxis(mynameForInputmanager + "Y") +
            transform.right * Input.GetAxis(mynameForInputmanager + "X");

        if (Input.GetAxis(mynameForInputmanager + "X") == 0 && Input.GetAxis(mynameForInputmanager + "Y") == 0)
        {
            //animcon.SetBool("Run", false);
        }
        else
        {
            //animcon.SetBool("Run", true);
        }

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
	}

    private void FixedUpdate()
    {
    }
    /* 仮メソッド
    void PlayerRotateChange(Vector3 lookDirection)
    {
        Quaternion quaternion = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,quaternion, playerLookSpeed * Time.deltaTime);
    }*/
}
