using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
                if (!pState.dead)
                {
                    killPlayer();
                }
            }

        }
    }


    [Tooltip("飽食度最大上限")][SerializeField] public int foodLimit = 100;
    [Tooltip("飢餓度減少倒數時間")][SerializeField] public float hurgryAttackTime;
    [Tooltip("飢餓度到變瘦")][SerializeField] public bool sick;
    [Tooltip("出局後，手上是否有珍珠")][SerializeField] public GameObject bubble;

    private BasicPlayerControll pControll;
    private PlayerStateList pState;
    [SerializeField] private bool eatfood;

    public void init()
    {
        food = foodLimit;
        hurgryAttackTime = 10f;
        pControll = GetComponent<BasicPlayerControll>();
        pState = GetComponent<PlayerStateList>();
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

        //如果吃到顏色不對
        if (pControll.FoodColor != foodObject.GetComponent<foodpart>().FoodColor)
        {
            pControll.FoodColor = foodObject.GetComponent<foodpart>().FoodColor;
            pControll.bubbles = 0;
        }

        if (!eatfood)
        {
            eatfood = true;
            food = food + foodObject.GetComponent<foodpart>().food;
            pControll.bubbles++;
            StartCoroutine(fooding(0.02f));
        }

        StopCoroutine("hungry");
        StopCoroutine("hungry");
        StartCoroutine(hurgry(hurgryAttackTime));
    }

    private IEnumerator fooding(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        eatfood = false;
    }

    private void killPlayer()
    {
        pControll.retired();
        StopAllCoroutines();
        StopAllCoroutines();
    }

    public void holdOn()
    {
        StopAllCoroutines();
        StopAllCoroutines();
    }

    public void keepGo()
    {
        StartCoroutine(hurgry(hurgryAttackTime));
    }

}
