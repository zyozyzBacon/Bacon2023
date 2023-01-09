using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;


    [Header("�D��ͦ��ɶ�����")]
    [SerializeField][Tooltip("�������ɶ�")] private float remoteTime;

    [Header("�D��C��")]
    [SerializeField] private GameObject remoteObject;


    [Header("�D��ͦ���m�C��")]
    [SerializeField] private Transform remotePostion;

    CinemachineTargetGroup.Target[] cameraTarget;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        cameraTarget = instance.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets;
    }

    public void remoteTaken() 
    {
        StartCoroutine(remoteCount(remoteTime));
    }

    private IEnumerator remoteCount(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(remoteObject,remotePostion);
        cameraTarget[4].target = remoteObject.transform;
    }
    
}
