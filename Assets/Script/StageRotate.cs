using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(0f, Time.deltaTime * 10, 0f);
        Vector3 stageUpVector = transform.up;
    }
}
