using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
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
                killPlayer();
            }
                
        }
    }
    

    [Tooltip("�����׳̤j�W��")][SerializeField] public int foodLimit = 100;
    [Tooltip("���j�״�֭˼Ʈɶ�")][SerializeField] public float hurgryAttackTime;
    [Tooltip("�X����A��W�O�_���ï]")][SerializeField] public GameObject bubble;
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
        
        Debug.Log("�Y�쭹���F");
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
