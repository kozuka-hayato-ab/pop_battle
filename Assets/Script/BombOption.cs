using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BombOption : MonoBehaviour
{
    private const int bombBigDamageValue = 2;
    private const int bombSmallDamageValue = 1;
    private int bombDamage;
    public int ThrowPlayerID { get; set; }
    [SerializeField] float destroyTime = 1f;
    [SerializeField] float bigDamageRange = 0.3f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float range = Vector3.SqrMagnitude(other.transform.position - transform.position);
            if (range < bigDamageRange * bigDamageRange)
                bombDamage = bombBigDamageValue;
            else
                bombDamage = bombSmallDamageValue;
            ExecuteEvents.Execute<PlayerControllerRecieveInterface>(
                target: other.transform.root.gameObject,
                eventData: null,
                functor: (reciever, y) => reciever.Damage(bombDamage, ThrowPlayerID)
                );
        }
    }
}
