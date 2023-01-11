using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("模式玩法相關")]
    [Tooltip("模式時間")][SerializeField] private float time;
    private float _time;

    [Tooltip("珍珠生成時間")][SerializeField] private float bubbleTime;
    [Tooltip("一波珍珠生成的量")][SerializeField] private int bubbleWaveNum;

    public GameObject bubblePrefab;

    private Transform[] bubblePositon = new Transform[0];

    public void Start()
    {
        instance = this;
        init();
    }

    void init()
    {
        _time = Time.time + time;
        bubblePositon = new Transform[GameObject.Find("###珍珠生成位置###").transform.childCount];

        for (int i = 0; i < bubblePositon.Length; i++)
        {
            bubblePositon[i] = GameObject.Find("###珍珠生成位置###").transform.GetChild(i);
        }

        StartCoroutine(bubbleWave(bubbleTime));

    }

    public void bubbleDetect() 
    {
        int Last = 0;

        for (int i = 0; i < bubblePositon.Length; i++) 
        {
            if (bubblePositon[i].childCount == 0) 
            {
                Last++;
                
            }
        }

        if (Last == bubblePositon.Length-1 ) 
        {
            StartCoroutine(bubbleWave(bubbleTime));
        }
    }

    private IEnumerator bubbleWave(float seconds)
    {
        yield return new WaitForSeconds(seconds);


        for (int i = 0; i < bubbleWaveNum; i++) 
        {
            int r = Random.Range(0, bubblePositon.Length);

            if (bubblePositon[r].childCount != 0)
            {
                i--;
            }
            else 
            {
                GameObject bub = Instantiate(bubblePrefab, bubblePositon[r].position, bubblePositon[r].rotation);
                bub.transform.parent = bubblePositon[r].transform;
            }
        }
    }
}
