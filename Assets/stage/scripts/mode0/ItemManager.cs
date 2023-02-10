using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [Header("道具開關")]
    [SerializeField][Tooltip("遙控器")] private bool remoteON;

    [Header("道具生成時間相關")]
    [SerializeField][Tooltip("遙控器時間")] private float remoteTime;

    [Header("道具列表")]
    [SerializeField] private GameObject remoteObject;


    [Header("道具生成位置列表")]
    [SerializeField] private Transform remotePostion;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void remoteTaken() 
    {
        if(remoteON)
            StartCoroutine(remoteCount(remoteTime));
    }

    private IEnumerator remoteCount(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(remoteObject,remotePostion);
    }

    public enum foodColor 
    {
        none,
        white,
        black,
    }
    
}
