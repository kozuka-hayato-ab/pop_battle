using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour ,ISelectHandler{

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.Instance.PlaySEClipFromIndex(0, 1f);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
