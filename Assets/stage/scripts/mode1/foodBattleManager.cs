using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("�Ҧ����k����")]
    [Tooltip("�Ҧ��ɶ�")][SerializeField] private float time;
    private float _time;

    [Tooltip("�ï]�ͦ��ɶ�")][SerializeField] private float bubbleTime;
    [Tooltip("�@�i�ï]�ͦ����q")][SerializeField] private int bubbleWaveNum;
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
        bubblePositon = new Transform[GameObject.Find("###�ï]�ͦ���m###").transform.childCount];

        for (int i = 0; i < bubblePositon.Length; i++)
        {
            bubblePositon[i] = GameObject.Find("###�ï]�ͦ���m###").transform.GetChild(i);
        }

        bubbleNum = 0;
        StartCoroutine(bubbleWave(bubbleTime));

    }

    public void bubbleDetect() 
    {
        if (bubbleNum == 0) 
        {
            Debug.Log("�U�@�i�ï]�w��");
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
                Debug.Log("���W�ï]");
            }
            else 
            {
                Debug.Log("�ͦ��@���ï]");
                GameObject bub = Instantiate(bubblePrefab, bubblePositon[r].position, bubblePositon[r].rotation);
                bub.transform.parent = bubblePositon[r].transform;
                bubblePositon[r].GetComponent<bubblePoint>().bubble = true;
                bubbleNum++;
            }
        }
    }
}
