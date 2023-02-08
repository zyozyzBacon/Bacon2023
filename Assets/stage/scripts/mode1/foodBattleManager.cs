using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using static ItemManager;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("模式玩法相關")]
    [Tooltip("模式時間")][SerializeField] private float time;
    private float _time;

    [Tooltip("珍珠生成時間")][SerializeField] private float bubbleTime;
    [Tooltip("場上珍珠生成的量")][SerializeField] private int BubbleWaveNum;

    public GameObject[] bubblePrefab = new GameObject[2];

    private Transform[] bubblePositon = new Transform[0];

    IEnumerator bubbleCoroutine;

    int c;
    public void Awake()
    {
        instance = this;
    }

    public void init()
    {
        _time = Time.time + time;
        bubblePositon = new Transform[GameObject.Find("###珍珠生成位置###").transform.childCount];

        for (int i = 0; i < bubblePositon.Length; i++)
        {
            bubblePositon[i] = GameObject.Find("###珍珠生成位置###").transform.GetChild(i);
        }

        bubbleCoroutine = bubbleWave(bubbleTime , BubbleWaveNum);

        StartCoroutine(bubbleCoroutine);

        c = 0;
    }

    public void bubbleDetect() 
    {
        int a = 0;
        for (int i = 0; i < bubblePositon.Length; i++)
        {
            if (bubblePositon[i].childCount > 0)
                a++;     
        }

        if (a < BubbleWaveNum + 1) 
        {
            StopCoroutine(bubbleCoroutine);
            bubbleCoroutine = bubbleWave(bubbleTime / 2, BubbleWaveNum - a + 1);
            StartCoroutine(bubbleCoroutine);
        }
    }

    public void endGame() 
    {
        for (int i = 0; i < bubblePositon.Length; i++)
        {
            if(bubblePositon[i].childCount > 0)
                Destroy(bubblePositon[i].GetChild(0).gameObject);
        }
    }



    private IEnumerator bubbleWave(float seconds,int num)
    {
        yield return new WaitForSeconds(seconds);

        
        for (int i = 0; i < num; i++) 
        {
            int r = Random.Range(0, bubblePositon.Length);
  
            if (bubblePositon[r].childCount != 0 || bubblePositon[r].GetComponent<foodPos>().playerAround)
            {
                i--;
            }
            else 
            {
                int w = 0;
                int b = 0;

                for (int a = 0; a < bubblePositon.Length; a++)
                {
                    if (bubblePositon[a].childCount > 0) 
                    {
                        if (bubblePositon[a].GetChild(0).GetComponent<foodpart>().FoodColor == foodColor.white)
                        {
                            w++;
                        }
                        else if(bubblePositon[a].GetChild(0).GetComponent<foodpart>().FoodColor == foodColor.black)
                        {
                            b++;
                        }

                    }   
                }

                Debug.Log(b + "," + w);

                if (b == w) 
                {
                    int a = Random.Range(0, 100);

                    switch (a % 2)
                    {
                        case 0:
                            b++;
                            break;
                        case 1:
                            w++;
                            break;
                    }
                }
                

                if (b > w)
                {
                    Debug.Log("白");
                    GameObject bub = Instantiate(bubblePrefab[0], bubblePositon[r].position, bubblePositon[r].rotation);
                    bub.transform.parent = bubblePositon[r].transform;
                }
                else if (b < w)
                {
                    Debug.Log("黑");
                    GameObject bub = Instantiate(bubblePrefab[1], bubblePositon[r].position, bubblePositon[r].rotation);
                    bub.transform.parent = bubblePositon[r].transform;
                }     
            }
        }
    }
}
