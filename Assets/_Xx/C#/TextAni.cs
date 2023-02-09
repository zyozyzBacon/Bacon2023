using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAni : MonoBehaviour
{

    public GameObject Text_P1;
    public GameObject Text_P2;
    public GameObject GameRulesCanvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ControlPlayback();
    }
    void ControlPlayback()//±±¨î¼½©ñ
    {
        if (Input.GetKey(KeyCode.N))
        {
            Destroy(Text_P1);
            Text_P2.gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.M))
        {
            Destroy(GameRulesCanvas);
        }
    }
}
