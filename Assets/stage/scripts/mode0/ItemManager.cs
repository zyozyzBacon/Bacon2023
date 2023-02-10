using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [Header("�D��}��")]
    [SerializeField][Tooltip("������")] private bool remoteON;

    [Header("�D��ͦ��ɶ�����")]
    [SerializeField][Tooltip("�������ɶ�")] private float remoteTime;

    [Header("�D��C��")]
    [SerializeField] private GameObject remoteObject;


    [Header("�D��ͦ���m�C��")]
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
