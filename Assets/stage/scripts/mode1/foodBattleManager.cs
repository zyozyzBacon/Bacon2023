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
    public int bubbleNum;

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

        bubbleNum = 0;
        StartCoroutine(bubbleWave(bubbleTime));

    }

    public void bubbleDetect() 
    {
        if (bubbleNum == 0) 
        {
            Debug.Log("下一波珍珠預備");
            StartCoroutine(bubbleWave(bubbleTime));
        }
    }

    private IEnumerator bubbleWave(float seconds)
    {
        yield return new WaitForSeconds(seconds);


        for (int i = 0; i < bubbleWaveNum; i++) 
        {
            int r = Random.Range(0, bubblePositon.Length);

            if (bubblePositon[r].GetComponent<bubblePoint>().bubble)
            {
                i--;
                Debug.Log("撞上珍珠");
            }
            else 
            {
                Debug.Log("生成一顆珍珠");
                GameObject bub = Instantiate(bubblePrefab, bubblePositon[r].position, bubblePositon[r].rotation);
                bub.transform.parent = bubblePositon[r].transform;
                bubblePositon[r].GetComponent<bubblePoint>().bubble = true;
                bubbleNum++;
            }
        }
    }
}
