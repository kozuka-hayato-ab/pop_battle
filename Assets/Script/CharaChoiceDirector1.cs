using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharaChoiceDirector1 : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.ChangeBGM(2);

    }
    // Update is called once per frame
    void Update()
    {
    }


    public void BackToTitle()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        //AudioManager.Instance.PlaySEClipFromIndex(0, 0.5f);
        AudioManager.Instance.DestroySingleton();
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        if (PlayerDataDirector.Instance.participantsNumber() >= 2)
        {
            AudioManager.Instance.PlaySEClipFromIndex(0, 0.5f);
            SceneManager.LoadScene("Main");
        }
    }
}
