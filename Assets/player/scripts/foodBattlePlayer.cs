using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class foodBattlePlayer : MonoBehaviour
{
    [Header("模式玩法相關")]
    [Tooltip("飽食度")]
    [SerializeField]
    int _food;
    public int food
    {
        get => _food;
        set
        {
            _food = value;
            if (value >= foodLimit)
                _food = foodLimit;
            else if (value <= 0) 
            {
                _food = 0;
                killPlayer();
            }
                
        }
    }
    

    [Tooltip("飽食度最大上限")][SerializeField] public int foodLimit = 100;
    [Tooltip("飢餓度減少倒數時間")][SerializeField] public float hurgryAttackTime;
    [Tooltip("出局後，手上是否有珍珠")][SerializeField] public GameObject bubble;
    private float holdtime;

    private BasicPlayerControll pControll;
    private PlayerStateList pState;
    private bool eatfood;

    public void init()
    {
        food = foodLimit;
        hurgryAttackTime = 3f;
        holdtime = 0f;
        pControll = GetComponent<BasicPlayerControll>();
        pState = GetComponent<PlayerStateList>();
        StartCoroutine(hurgry(hurgryAttackTime));
    }
    
    private IEnumerator hurgry(float seconds) 
    {
        holdtime = Time.time + hurgryAttackTime;
        yield return new WaitForSeconds(seconds);
        food = food - (foodLimit / 5);
        StartCoroutine(hurgry(hurgryAttackTime));
    }

    public void eating(GameObject foodObject) 
    {
        
        Debug.Log("吃到食物了");
        StopCoroutine("hurgry");
        StartCoroutine(hurgry(hurgryAttackTime));

        if (!eatfood)
        {
            eatfood = true;
            food = food + foodObject.GetComponent<foodpart>().food;
            pControll.bubbles++;
            StartCoroutine(fooding(0.02f));
        }
    }

    private IEnumerator fooding(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        eatfood = false;
    }

    private void killPlayer() 
    {
        pControll.retired();
    }

    public void holdOn() 
    {
        holdtime = holdtime + 2 - Time.time;
        StopCoroutine("hurgry");
    }

    public void keepGo() 
    {
        StartCoroutine(hurgry(holdtime));
    }
}
