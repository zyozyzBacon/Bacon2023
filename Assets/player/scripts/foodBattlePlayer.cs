using System.Collections;
using System.Collections.Generic;
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
                //dead
            }
                
        }
    }
    

    [Tooltip("飽食度最大上限")][SerializeField] public int foodLimit = 100;
    [Tooltip("飢餓度減少倒數時間")][SerializeField] public float hurgryAttackTime;

    private bool eatfood;

    public void init()
    {
        food = foodLimit;
        hurgryAttackTime = 3f;
        StartCoroutine(hurgry(hurgryAttackTime));
    }
    
    private IEnumerator hurgry(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        food = food - (foodLimit / 5);
        StartCoroutine(hurgry(hurgryAttackTime));
    }

    public void eating(GameObject foodObject) 
    { 
        food = food + foodObject.GetComponent<foodpart>().food;
        Debug.Log("吃到食物了");
        StopCoroutine("hurgry");
        StartCoroutine(hurgry(hurgryAttackTime));

        if (foodBattleManager.instance != null && !eatfood)
        {
            eatfood = true;
            foodObject.transform.parent.transform.GetComponent<bubblePoint>().bubble = false;
            foodBattleManager.instance.bubbleNum--;
            foodBattleManager.instance.bubbleDetect();
            StartCoroutine(fooding(0.02f));
        }
    }

    private IEnumerator fooding(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        eatfood = false;
    }

}
