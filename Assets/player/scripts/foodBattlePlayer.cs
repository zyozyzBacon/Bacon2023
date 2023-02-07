using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Tooltip("���j�״�֭˼Ʈɶ�")][SerializeField] public float hurgryAttackTime = 10;
    [Tooltip("���j�״�ֶq")][SerializeField] public int hurgryAttack = 20;

    private BasicPlayerControll pControll;
    private PlayerStateList pState;
    [SerializeField] private bool eatfood;

    IEnumerator hungryCoroutine;

    public void init()
    {
        food = foodLimit;
        pControll = GetComponent<BasicPlayerControll>();
        pState = GetComponent<PlayerStateList>();
        hungryCoroutine = hurgry(hurgryAttackTime);
    }

    public void startgame() 
    {
        StartCoroutine(hungryCoroutine);
    }

    public void eating(GameObject foodObject) 
    {
        if (!eatfood)
        {
            eatfood = true;
            food = food + foodObject.GetComponent<foodpart>().food;

            StartCoroutine(fooding(0.02f));

            StopCoroutine(hungryCoroutine);
            StartCoroutine(hungryCoroutine);
        }

    }

    private IEnumerator fooding(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        eatfood = false;
    }

    private IEnumerator hurgry(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        food = food - hurgryAttack;
        StartCoroutine(hungryCoroutine);
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
        StartCoroutine(hungryCoroutine);
    }

}
