using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudDirector : MonoBehaviour {
    private static CloudDirector instance;
    public static CloudDirector Instance
    {
        get
        {
            return instance;
        }
    }

    private float repopTime = 5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void RepopCloud(CloudOption cloud)
    {
        cloud.gameObject.SetActive(false);
        StartCoroutine(WaitRepopTime(cloud));

    }

    IEnumerator WaitRepopTime(CloudOption cloud)
    {
        yield return new WaitForSeconds(repopTime);
        cloud.gameObject.SetActive(true);
        cloud.StateInit();
    }
}
