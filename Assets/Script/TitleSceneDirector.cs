﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleSceneDirector : MonoBehaviour {
    [SerializeField] GameObject MainButtons;
    [SerializeField] GameObject HowToUse;

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject HowToUseButton;
    [SerializeField] GameObject BackButton;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void HowToUseOn()
    {
        HowToUse.SetActive(true);
        eventSystem.SetSelectedGameObject(BackButton);
        MainButtons.SetActive(false);
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
    }

    public void BackButtonOn()
    {
        MainButtons.SetActive(true);
        eventSystem.SetSelectedGameObject(HowToUseButton);
        HowToUse.SetActive(false);
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
    }

    public void NextScene()
    {
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
        SceneManager.LoadScene("CharactorChoice");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
        Application.Quit();
    }
}
