using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.EventSystems;

public class TitleSceneDirector : MonoBehaviour {
    [SerializeField] GameObject MainButtons;
    [SerializeField] GameObject HowToUse;
    
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HowToUseOn1()
    {
        HowToUse.SetActive(true);
    }

    public void HowToUseOn2()
    {
        MainButtons.SetActive(false);
    }

    public void BackButton1()
    {
        MainButtons.SetActive(true);
    }

    public void BackButton2()
    {
        HowToUse.SetActive(false);

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
