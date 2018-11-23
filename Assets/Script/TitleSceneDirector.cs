using System.Collections;
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

    [SerializeField] GameObject Title;
	// Use this for initialization
	void Start () {
        AudioManager.Instance.ChangeBGM(1);
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void HowToUseOn()
    {
        HowToUse.SetActive(true);
        eventSystem.SetSelectedGameObject(BackButton);
        Title.SetActive(false);
        MainButtons.SetActive(false);
        AudioManager.Instance.PlaySEClipFromIndex(1, 1f);
    }

    public void BackButtonOn()
    {
        Title.SetActive(true);
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
