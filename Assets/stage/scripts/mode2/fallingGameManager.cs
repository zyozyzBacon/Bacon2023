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
    [Tooltip("所有珍珠")][SerializeField] private GameObject allBubble;
    [Tooltip("所有平台")][SerializeField] private GameObject allPlatform;

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

    public void endGame()
    {
        PlatCore.GetComponent<platCore>().Active = false;

        for (int b = 0; b < allBubble.transform.childCount; b++)
            Destroy(allBubble.transform.GetChild(b));

        for (int i = 0; i < allPlatform.transform.childCount; i++)
            allPlatform.transform.GetChild(i).GetComponent<fallingPart>().active = false;
    }

    IEnumerator firstplat(float seconds) 
    {
        yield return new WaitForSeconds(seconds);

        FirstPlatform.GetComponent<fallingPart>().active = true;
    }
}
