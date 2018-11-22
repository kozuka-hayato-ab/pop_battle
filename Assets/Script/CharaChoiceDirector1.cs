//title遷移時ぬるぽ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharaChoiceDirector1 : MonoBehaviour
{
    private int nowParticipate;
    private bool nullpoDestroy;

    private void Start()
    {
        nullpoDestroy = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (nullpoDestroy)
        {
            nowParticipate = PlayerDataDirector.Instance.participantsNumber();//titleに遷移する前にDestroyしてるからぬるぽ
        }
    }


    public void BackToTitle()
    {
        nullpoDestroy = false;
        PlayerDataDirector.Instance.DestroySingleton();
        //AudioManager.Instance.PlaySEClipFromIndex(0, 0.5f);
        AudioManager.Instance.DestroySingleton();
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        if (nowParticipate == 2 || nowParticipate == 4 || nowParticipate == 3)
        {
            AudioManager.Instance.PlaySEClipFromIndex(0, 0.5f);
            SceneManager.LoadScene("Main");
        }
    }
}
