using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingGameManager : MonoBehaviour
{
    public static fallingGameManager instance;

    [Header("�Ҧ����k����")]
    [Tooltip("�Ҧ��ɶ�")][SerializeField] private float time;
    private float _time;

    [Tooltip("�Ĥ@�ӥ��x")][SerializeField] private GameObject FirstPlatform;
    [Tooltip("���x�Ұ��I")][SerializeField] private GameObject PlatCore;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public void init()
    {
        _time = Time.time + time;
        StartCoroutine(firstplat(5f));
        PlatCore.GetComponent<platCore>().Active = true;
    }

    IEnumerator firstplat(float seconds) 
    {
        yield return new WaitForSeconds(seconds);

        FirstPlatform.GetComponent<fallingPart>().active = true;
    }
}
