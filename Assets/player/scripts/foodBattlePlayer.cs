using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class foodBattlePlayer : MonoBehaviour
{
    [Header("�Ҧ����k����")]
    [Tooltip("������")]
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


    [Tooltip("�����׳̤j�W��")][SerializeField] public int foodLimit = 100;
    [Tooltip("���j�״�֭˼Ʈɶ�")][SerializeField] public float hurgryAttackTime;
    [Tooltip("���j�ר��ܽG")][SerializeField] public bool sick;
    [Tooltip("�X����A��W�O�_���ï]")][SerializeField] public GameObject bubble;

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

        //�p�G�Y���C�⤣��
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
