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
        mynameForInputmanager = "Gamepad" + playerID + "_";
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, Input.GetAxis(mynameForInputmanager + "CameraX") * playerLookSpeed * Time.deltaTime, 0), Space.Self);
        cameraVerticalAngel = Mathf.Clamp(cameraVerticalAngel + Input.GetAxis(mynameForInputmanager + "CameraY") * cameraAngleSpeed * Time.deltaTime,
            cameraVerticalUnderLimit, cameraVerticalUpperLimit);
        CameraPivot.transform.eulerAngles = new Vector3(cameraVerticalAngel, CameraPivot.transform.eulerAngles.y, CameraPivot.transform.eulerAngles.z);
        
        //プレイヤー基準で向きを決める。
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

        characon.Move(playerMoveDirection * Time.deltaTime);
    }

    public void ChangeGameController(int id)
    {
        if(1 <= id && id <= 4) playerID = id;
    }
}
