using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;


    [Header("道具列表")]
    public GameObject[] ItemList;
    

    [Header("廢案")]
    [SerializeField][Tooltip("遙控器")] private bool remoteON;
    [SerializeField][Tooltip("遙控器時間")] private float remoteTime;
    [SerializeField] private GameObject remoteObject;
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
