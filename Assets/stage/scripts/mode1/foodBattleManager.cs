using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemManager;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("模式玩法相關")]

    [Tooltip("平台啟動點")][SerializeField] private GameObject PlatCore;

    public GameObject[] bubblePrefab = new GameObject[2];
    private Transform[] bubblePositon = new Transform[0];



    public void Awake()
    {
        instance = this;
    }


    public void init()
    {
        PlatCore.GetComponent<platCore>().Active = true;
    }

    public void endGame(Dictionary<int, GameObject> playerList) 
    {
        for (int i = 0; i < bubblePositon.Length; i++)
        {
            if(bubblePositon[i].childCount > 0)
                Destroy(bubblePositon[i].GetChild(0).gameObject);
        }

        for (int p = 0;p < playerList.Count; p++) 
        {
            playerList[p].GetComponent<foodBattlePlayer>().StopAllCoroutines();
        }

        PlatCore.GetComponent<platCore>().Active = false;
    }

}
