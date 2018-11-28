using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackgroundController : MonoBehaviour {
	
    [SerializeField] float scrollSpeed;
    private RectTransform rt;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }
    private void FixedUpdate()
    {
        Vector2 pos = rt.anchoredPosition;
        if (pos.x <= -800.0f)
        {
            pos.x = 800.0f;
        }
        pos.x -= scrollSpeed;
        rt.anchoredPosition = pos;
    }
}
