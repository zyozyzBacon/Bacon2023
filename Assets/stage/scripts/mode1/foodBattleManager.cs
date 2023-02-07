using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("�Ҧ����k����")]
    [Tooltip("�Ҧ��ɶ�")][SerializeField] private float time;
    private float _time;

    [Tooltip("�ï]�ͦ��ɶ�")][SerializeField] private float bubbleTime;
    [Tooltip("�@�i�ï]�ͦ����q")][SerializeField] private int BubbleWaveNum;

    public GameObject bubblePrefab;

    private Transform[] bubblePositon = new Transform[0];

    public Sprite[] bubbleColor = new Sprite[2];

    IEnumerator bubbleCoroutine;
    public void Awake()
    {
        instance = this;
    }

    public void init()
    {
        _time = Time.time + time;
        bubblePositon = new Transform[GameObject.Find("###�ï]�ͦ���m###").transform.childCount];

        for (int i = 0; i < bubblePositon.Length; i++)
        {
            bubblePositon[i] = GameObject.Find("###�ï]�ͦ���m###").transform.GetChild(i);
        }

        bubbleCoroutine = bubbleWave(bubbleTime);

        StartCoroutine(bubbleCoroutine);

    }

    public void bubbleDetect() 
    {
        int a = 0;
        for (int i = 0; i < bubblePositon.Length; i++)
        {
            if (bubblePositon[i].childCount > 0)
                a++;  
        }

        Debug.Log(a);

        if (a <= 1) 
        {
            StopCoroutine(bubbleCoroutine);
            bubbleCoroutine = bubbleWave(bubbleTime / 2);
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



    private IEnumerator bubbleWave(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        int c = 0;
        for (int i = 0; i < BubbleWaveNum; i++) 
        {
            int r = Random.Range(0, bubblePositon.Length);
            c++;

            if (bubblePositon[r].childCount != 0 || bubblePositon[r].GetComponent<foodPos>().playerAround)
            {
                i--;
            }
            else 
            {
                GameObject bub = Instantiate(bubblePrefab, bubblePositon[r].position, bubblePositon[r].rotation);
                bub.transform.parent = bubblePositon[r].transform;

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
