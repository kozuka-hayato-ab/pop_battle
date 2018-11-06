//playerUIごとにスクリプトをアタッチ
//案１キャラを選んでいると同時にplayerを設定する
//案２キャラを選び，決定ボタンを押すとstartボタンが出現する
//とりあえず案１で実装
//問題：Player1とPlayer４が戦う場合コントローラーは変わるのか？
//問題：Playerが３人にできない処理(charachoicedirector1.csの方で参加人数２と４の時のみ遷移可能にした)


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorSelect : MonoBehaviour {

    [SerializeField] Image CharactorImage;
    [SerializeField] Sprite[] charactors;
    [SerializeField] int playerId;
    [SerializeField] float inputInterval;
    private string controllerName;
    private bool enableInput;
    private int charactorNow;

	// Use this for initialization
	void Start () {
        //コントローラーの番号設定
        controllerName = "Gamepad" + playerId + "_";
        enableInput = true;
        charactorNow = 0;
        CharactorImage.sprite = charactors[0];

	}
	
	// Update is called once per frame
	void Update () {
        float axis = Input.GetAxis(controllerName + "Y");
        //上入力
        if(enableInput && axis >= 1.0f){
            //キャラ選択の処理
            //画像の変更処理
            charactorNow++;
            charactorNow %= charactors.Length;
            PlayerCharaChoice(charactorNow);
            enableInput = false;
            StartCoroutine(WaitInput());
            Debug.Log(PlayerDataDirector.Instance.PlayerTypes[playerId - 1]);
        }
        //下入力
        if (enableInput && axis <= -1.0f)
        {
            //キャラ選択の処理
            //画像の変更処理
            charactorNow--;
            charactorNow = (charactorNow + charactors.Length) % charactors.Length; //多分もっといい方法ある
            PlayerCharaChoice(charactorNow);
            enableInput = false;
            StartCoroutine(WaitInput());
            Debug.Log(PlayerDataDirector.Instance.PlayerTypes[playerId - 1]);
        }
	}

    public void PlayerCharaChoice(int charactorNumber)
    {
        switch (charactorNumber)
        {
            case (int)PlayerType.None:
                PlayerDataDirector.Instance.PlayerTypes[playerId - 1] = PlayerType.None;
                break;
            case (int)PlayerType.Charactor1:
                PlayerDataDirector.Instance.PlayerTypes[playerId - 1] = PlayerType.Charactor1;
                break;
            case (int)PlayerType.Charactor2:
                PlayerDataDirector.Instance.PlayerTypes[playerId - 1] = PlayerType.Charactor2;
                break;
            case (int)PlayerType.Charactor3:
                PlayerDataDirector.Instance.PlayerTypes[playerId - 1] = PlayerType.Charactor3;
                break;
            case (int)PlayerType.Charactor4:
                PlayerDataDirector.Instance.PlayerTypes[playerId - 1] = PlayerType.Charactor4;
                break;
        }
        CharactorImage.sprite = charactors[charactorNumber];
    }

    //入力後次の入力までの時間
    IEnumerator WaitInput()
    {
        yield return new WaitForSeconds(inputInterval);
        enableInput = true;
    }
}
