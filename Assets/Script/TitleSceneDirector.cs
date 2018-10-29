using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneDirector : MonoBehaviour {
    [SerializeField] GameObject MainButtons;
    [SerializeField] GameObject HowToUse;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HowToUseOn()
    {
        HowToUse.SetActive(true);
        MainButtons.SetActive(false);
    }

    public void BackButton()
    {
        HowToUse.SetActive(false);
        MainButtons.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("CharactorChoice");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
