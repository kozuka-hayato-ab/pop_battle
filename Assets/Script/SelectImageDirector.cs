using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectImageDirector : MonoBehaviour
{
    private EventSystem eventSystem;

    [SerializeField] GameObject[] TargetButtons;
    private Image[] SelectImage;

    private GameObject PreSelectedButton;

    private void Awake()
    {
        eventSystem = GameObject.FindObjectOfType<EventSystem>();
        SelectImage = new Image[TargetButtons.Length];
        for (int i = 0; i < TargetButtons.Length; i++)
        {
            SelectImage[i] = TargetButtons[i].transform.Find("SelectImage").GetComponent<Image>();
            SelectImage[i].gameObject.SetActive(false);
        }
        PreSelectedButton = eventSystem.firstSelectedGameObject;
        SelectImage[0].gameObject.SetActive(true);
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject NowSelectedButton = eventSystem.currentSelectedGameObject;
        if(PreSelectedButton != NowSelectedButton)
        {
            UpdateSelectImage();
        }
        PreSelectedButton = NowSelectedButton;
    }

    public void UpdateSelectImage()
    {
        for (int i = 0; i < TargetButtons.Length; i++)
        {
            if (TargetButtons[i] == eventSystem.currentSelectedGameObject)
            {
                SelectImage[i].gameObject.SetActive(true);
            }
            else
            {
                SelectImage[i].gameObject.SetActive(false);
            }
        }
    }
}
