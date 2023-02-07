using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("模式玩法相關")]
    [Tooltip("模式時間")][SerializeField] private float time;
    private float _time;

    [Tooltip("珍珠生成時間")][SerializeField] private float bubbleTime;
    [Tooltip("場上珍珠生成的量")][SerializeField] private int BubbleWaveNum;

    public GameObject bubblePrefab;

    private Transform[] bubblePositon = new Transform[0];

    public Sprite[] bubbleColor = new Sprite[2];

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
                GameObject bub = Instantiate(bubblePrefab, bubblePositon[r].position, bubblePositon[r].rotation);
                bub.transform.parent = bubblePositon[r].transform;

                c++;

                if (c % 2 == 0)
                {
                    bub.GetComponent<foodpart>().FoodColor = ItemManager.foodColor.white;
                    bub.GetComponent<SpriteRenderer>().sprite = bubbleColor[0];
                }
                else 
                {
                    bub.GetComponent<foodpart>().FoodColor = ItemManager.foodColor.black;
                    bub.GetComponent<SpriteRenderer>().sprite = bubbleColor[1];
                } 
            }
        }
    }
}
