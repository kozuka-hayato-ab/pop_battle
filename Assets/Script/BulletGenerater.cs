using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerater : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject muzzle;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDestroyTime;
    [SerializeField] float bombSpeed;
    [SerializeField] float bombVerticalCompensation;
    [SerializeField] Camera playerCamera;
    private int userPlayerNumber;//プレイヤーが何Pなのか

    // Use this for initialization
    void Start()
    {
        userPlayerNumber = transform.root.GetComponent<PlayerController>().PlayerID;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BulletShot()
    {
        var bulletInstance = Instantiate(bullet,
            muzzle.transform.position,
            muzzle.transform.rotation
            ) as GameObject;
        bulletInstance.GetComponent<Rigidbody>().AddForce(
            playerCamera.transform.forward * bulletSpeed, ForceMode.VelocityChange);
        bulletInstance.GetComponent<BulletOption>().shotPlayerNumber = userPlayerNumber;
        Destroy(bulletInstance, bulletDestroyTime);
    }

    public void BombShot()
    {
        var bombInstance = Instantiate(bomb,
            muzzle.transform.position,
            muzzle.transform.rotation
            ) as GameObject;
        bombInstance.GetComponent<Rigidbody>().AddForce(
            (playerCamera.transform.forward + Vector3.up * bombVerticalCompensation) * bombSpeed, ForceMode.Impulse);
    }
}