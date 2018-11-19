using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * rotateSpeed, 0f));
    }
}
