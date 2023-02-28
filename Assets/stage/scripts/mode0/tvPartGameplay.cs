using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using Cinemachine;
using UnityEngine.InputSystem;

public class tvPartGameplay : MonoBehaviour
{

    public static tvPartGameplay tvGameplayManager;

    public tvState TvState;

    [Header("�@���ɶ�����")]
    [SerializeField][Tooltip("�ǳƷm���ɶ�")] private float readyTime;
    [SerializeField][Tooltip("�Ĥ@���m���ɶ�")] private float answerTime;
    [SerializeField][Tooltip("�Ѥj�m���ɶ�")] private float answerTimeBoss;
    [SerializeField][Tooltip("�������A���w�Įɶ�(���ӷ|�O0.5f)")] private float transTime;

    [Header("[�Ű�]UI�������")]
    [SerializeField] private GameObject questionText;
    [SerializeField] private GameObject answerAText;
    [SerializeField] private GameObject answerBText;

    [Header("[�Ű�]����������")]
    [SerializeField] private Transform tvObject;
    [SerializeField] private Transform tvSpawnPoint;

    [Header("[�Ű�]��v���������")]
    [SerializeField]private CinemachineVirtualCamera tvGameplayCamere;



    private Dictionary<int,GameObject> playerList;
    private GameObject bossPlayer;
    private answer correctAnswer;

    private Vector2[] v2;

    public void Awake()
    {
        tvGameplayManager = this;
    }

    public void readyTrans() 
    {
        Debug.Log("������q");
        TvState = tvState.tran;
        tvGameplayCamere.Priority = 11;
        playerList = MainGameManager.instance.playerList;
        v2 = new Vector2[4];

        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<BasicPlayerControll>().frozenForTV();

            if (!playerList[i].GetComponent<PlayerStateList>().dead)
            {
                playerList[i].GetComponent<BasicPlayerControll>().inTvMode();
            }

            playerList[i].GetComponent<PlayerStateList>().currentAnswer = answer.none;
        }

        StartCoroutine(ready(transTime));
    }

    public IEnumerator ready(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("�Ұʹq�����q");

        for (int i = 0; i < playerList.Count; i++)
        {
            v2[i] = new Vector2(playerList[i].transform.position.x, playerList[i].transform.position.y + 0.5f);

            if (!playerList[i].GetComponent<PlayerStateList>().dead)
                playerList[i].transform.position =
                new Vector2(tvSpawnPoint.position.x, tvSpawnPoint.position.y);
        }

        TvState = tvState.ready;
        StartCoroutine(readyCountDown(readyTime));
    }

    public void bossReady(GameObject boss)
    {
        bossPlayer = null;
        bossPlayer = boss;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].GetComponent<PlayerStateList>().dead)
                playerList[i].GetComponent<BasicPlayerControll>().unfrozen();
        }
        bossPlayer.GetComponent<BasicPlayerControll>().frozenForTV();
        bossPlayer.GetComponent<BasicPlayerControll>().tvMoveSpeed *= 3;

        bossPlayer.GetComponent<BasicPlayerControll>().tvModePart.GetComponent<SpriteRenderer>().enabled= false;
    }

    public void bossAnswer() 
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].GetComponent<PlayerStateList>().dead)
                playerList[i].GetComponent<BasicPlayerControll>().frozenForTV();
        }
        bossPlayer.GetComponent<BasicPlayerControll>().unfrozen();
        bossPlayer.GetComponent<BasicPlayerControll>().tvModePart.GetComponent<SpriteRenderer>().enabled = true;
    }

    IEnumerator readyCountDown(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("�ҰʲĤ@���m�����q");
        TvState = tvState.firstAnswer;
        StartCoroutine(endFirst(answerTime));
    }

    IEnumerator endFirst(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("�ҰʦѤj�m�����q");
        TvState = tvState.bossAnswer;
        bossAnswer();
        StartCoroutine(endSecond(answerTimeBoss));
    }

    IEnumerator endSecond(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("�����m�����q");
        TvState = tvState.end;
        tvGameplayCamere.Priority = 9;
        bossPlayer.GetComponent<BasicPlayerControll>().tvMoveSpeed /= 3;
        correctAnswer = bossPlayer.GetComponent<PlayerStateList>().currentAnswer;

        for (int i = 0; i < playerList.Count; i++)
        {
            if (!playerList[i].GetComponent<PlayerStateList>().dead)
            {
                playerList[i].transform.position = new Vector2(v2[i].x, v2[i].y);

                playerList[i].GetComponent<BasicPlayerControll>().outTvMode();
                playerList[i].GetComponent<BasicPlayerControll>().frozenForTV();
            }
        }

        conclusion();
        StartCoroutine(endTrans(transTime));
    }

    IEnumerator endTrans(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("�����m���᪺�w�Ķ��q");
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].GetComponent<BasicPlayerControll>().unfrozen();
        }
    }

    public void conclusion() 
    {
        GameObject[] target = new GameObject[4];
        
        for (int i = 0; i < playerList.Count; i++)
        {
            answer ans = playerList[i].GetComponent<PlayerStateList>().currentAnswer;

            if (ans != correctAnswer)
            {
                target[i] = playerList[i];
            }
        }

    }

    public enum tvState 
    {
        idle,
        tran,
        ready,
        firstAnswer,
        bossAnswer,
        end
    }
    public enum answer
    {
        none,
        answerA,
        answerB
    }
}
