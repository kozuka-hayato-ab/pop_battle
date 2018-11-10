using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private const int Max = 100;

    [SerializeField] private Vector3 stageCenterPoint;
    [SerializeField] private GameObject[] itemPopSpecificPlaces;
    private float popItemUpValue = 6;
    [SerializeField] private int SpecialItemPopProbability = 30;

    [SerializeField] private float[] upperPartRadius;// [0] -> start [1] -> end
    [SerializeField] private float[] underPartRadius;// this is same system ↑
    [SerializeField] private int upperPopPercent; // percent of Pop Item Under or Upper 

    private Dictionary<GameObject, int> ItemDict;
    [SerializeField] private GameObject[] Items;
    [SerializeField] private GameObject SpecialItem;
    [SerializeField] private int[] ItemProbability;

    [SerializeField] int startItemPopValue;
    [SerializeField] private int repopIntervalSeconds;
    [SerializeField] private int repopItemValue;

    private float timer;

    private void Awake()
    {
        ItemDict = new Dictionary<GameObject, int>();
        for (int i = 0; i < Items.Length; i++)
        {
            ItemDict.Add(Items[i], ItemProbability[i]);
        }
        GameStartItemPop();
        timer = repopIntervalSeconds;
    }
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        Mathf.Clamp(timer -= Time.deltaTime, 0, repopIntervalSeconds);
        if (timer <= 0)
        {
            RepopItem();
            timer = repopIntervalSeconds;
        }
    }

    private float randomRadian()
    {
        return Random.Range(0, 2 * Mathf.PI);
    }

    private GameObject DecideItem()
    {
        int totalPercent = 0;
        foreach (int per in ItemDict.Values)
        {
            totalPercent += per;
        }

        int randomValue = Random.Range(0, totalPercent);

        foreach (KeyValuePair<GameObject, int> pair in ItemDict)
        {
            randomValue -= pair.Value;

            if (randomValue <= 0)
            {
                return pair.Key;
            }
        }

        Debug.LogError("method DecideItem is failed");
        return new List<GameObject>(ItemDict.Keys)[0];
    }

    private void GenerateItem(float radius, float radian, GameObject item)
    {
        float x = Mathf.Cos(radian);
        float y = Mathf.Sin(radian);
        Vector3 direction = new Vector3(x, 0f, y);
        Vector3 generatePos = stageCenterPoint + direction * radius;
        Instantiate(item, generatePos, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }

    private void GenerateItem(Vector3 generatePos, GameObject item)
    {
        Instantiate(item, generatePos, Quaternion.Euler(new Vector3(-90, 0, 0)));
    }

    private bool boolFromPercent(int percent)
    {
        return Random.Range(0, Max) < percent;
    }

    private void PopItem()
    {
        float radius = 0;
        if (boolFromPercent(upperPopPercent))
        {
            radius = Random.Range(upperPartRadius[0], upperPartRadius[1]);
        }
        else
        {
            radius = Random.Range(underPartRadius[0], underPartRadius[1]);
        }
        GenerateItem(radius, randomRadian(), DecideItem());
    }

    private void PopItemOnSpecificPlace()
    {
        int length = itemPopSpecificPlaces.Length;
        for (int i = 0; i < length; i++)
        {
            GenerateItem(itemPopSpecificPlaces[i].transform.position + Vector3.up * popItemUpValue, SpecialItem);
        }
    }

    private void GameStartItemPop()
    {
        for (int i = 0; i < startItemPopValue; i++)
        {
            PopItem();
        }
        PopItemOnSpecificPlace();
    }

    private void RepopItem()
    {
        for (int i = 0; i < repopItemValue; i++)
        {
            PopItem();
        }
        if (boolFromPercent(SpecialItemPopProbability))
            PopItemOnSpecificPlace();
    }
}
