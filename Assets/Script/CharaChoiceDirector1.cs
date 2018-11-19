//title遷移時ぬるぽ

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharaChoiceDirector1 : MonoBehaviour
{
    private int nowParticipate;

    // Update is called once per frame
    void Update()
    {
        nowParticipate = PlayerDataDirector.Instance.participantsNumber();//ここがぬるぽ
    }


    public void BackToTitle()
    {
        PlayerDataDirector.Instance.DestroySingleton();
        AudioManager.Instance.DestroySingleton();
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        if(nowParticipate == 2 || nowParticipate == 4){
            SceneManager.LoadScene("Main");
        }
    }
}
