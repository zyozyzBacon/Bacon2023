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

    [Header("作答時間相關")]
    [SerializeField][Tooltip("準備搶答時間")] private float readyTime;
    [SerializeField][Tooltip("第一次搶答時間")] private float answerTime;
    [SerializeField][Tooltip("老大搶答時間")] private float answerTimeBoss;
    [SerializeField][Tooltip("切換狀態的緩衝時間(應該會是0.5f)")] private float transTime;

    [Header("[勿動]UI元件相關")]
    [SerializeField] private GameObject questionText;
    [SerializeField] private GameObject answerAText;
    [SerializeField] private GameObject answerBText;

    [Header("[勿動]抓取元件相關")]
    [SerializeField] private Transform tvObject;
    [SerializeField] private Transform tvSpawnPoint;

    [Header("[勿動]攝影機元件相關")]
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
        Debug.Log("轉場階段");
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
        Debug.Log("啟動電視階段");

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
        Debug.Log("啟動第一次搶答階段");
        TvState = tvState.firstAnswer;
        StartCoroutine(endFirst(answerTime));
    }

    IEnumerator endFirst(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("啟動老大搶答階段");
        TvState = tvState.bossAnswer;
        bossAnswer();
        StartCoroutine(endSecond(answerTimeBoss));
    }

    IEnumerator endSecond(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("結束搶答階段");
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
        Debug.Log("結束搶答後的緩衝階段");
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
