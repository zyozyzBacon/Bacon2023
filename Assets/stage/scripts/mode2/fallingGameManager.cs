using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingGameManager : MonoBehaviour
{
    public static fallingGameManager instance;

    [Header("模式玩法相關")]
    [Tooltip("模式時間")][SerializeField] private float time;
    private float _time;

    [Tooltip("第一個平台")][SerializeField] private GameObject FirstPlatform;
    [Tooltip("平台啟動點")][SerializeField] private GameObject PlatCore;

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
