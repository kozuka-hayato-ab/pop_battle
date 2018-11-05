using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudOption : MonoBehaviour {
    private Material[] myMaterials;
    private bool isFadeOut;

    private const float fadeTime = 7f;
    private float currentRemainTime;

    public void StateInit()
    {
        isFadeOut = false;
        currentRemainTime = fadeTime;
        foreach(Material i in myMaterials)
        {
            var color = i.color;
            color.a = 1;
            i.color = color;
        }
    }

    private void Awake()
    {
        myMaterials = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
        StateInit();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isFadeOut)
        {
            currentRemainTime -= Time.deltaTime;

            if(currentRemainTime <= 0f)
            {
                isFadeOut = false;
                CloudDirector.Instance.RepopCloud(this);
                return;
            }

            foreach(Material i in myMaterials)
            {
                float alphaValue = currentRemainTime / fadeTime;
                var color = i.color;
                color.a = alphaValue;
                i.color = color;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isFadeOut = true;
        }
    }
}
