using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float gravityStrength;

    private bool onGround = false;

    [SerializeField] float rayLength;
    protected virtual void Awake()
    {
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        if (!onGround)
        {
            FallMove();
            RayForJudgeGround();
        }
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f, true);
    }

    private void FallMove()
    {
        transform.position -= gravityStrength * Vector3.up * Time.deltaTime;
    }

    private void RayForJudgeGround()
    {
        ray = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(ray, out hit, rayLength, layerMask))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            if(layerName == "Stage")
            {
                onGround = true;
            }
        }
    }
}
