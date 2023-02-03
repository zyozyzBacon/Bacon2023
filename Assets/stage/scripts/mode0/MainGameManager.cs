using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager instance;

    public Dictionary<int, GameObject> playerList;

    [SerializeField] private gameMode GameMode;

    [SerializeField] private int playerNum;
    [SerializeField] private Transform[] playerSpawn;
    [SerializeField] public int[] ammo = new int[4];
    [SerializeField] public GameObject TimerText;
    [SerializeField] private bool cameraLocked;


    CinemachineTargetGroup.Target[] cameraTarget;
    private playerData pData;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(timer());
    }

    IEnumerator timer()
    {
        init();
        yield return new WaitForSeconds(3f);
        ItemManager.instance.remoteTaken();
        startGame();
    }


    private void init()
    {
        if(GameObject.Find("###PlayerData###").GetComponent<playerData>() != null)
            pData = GameObject.Find("###PlayerData###").GetComponent<playerData>();

        playerList = new Dictionary<int, GameObject>();
        playerNum = pData.playerNum;

        cameraTarget = instance.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets;

        for (int i = 0; i < playerNum; i++)
        {
            GameObject p = this.gameObject.GetComponent<PlayerInputManager>().JoinPlayer().gameObject;

            playerList.Add(i, p);
            p.transform.position = playerSpawn[i].position;

            p.GetComponent<PlayerStateList>().pause = true;

            if (i % 2 == 0)
            {
                p.transform.localScale = new Vector3(-Mathf.Abs(p.transform.localScale.x), p.transform.localScale.y, p.transform.localScale.z);
            }

            if (!cameraLocked)
                cameraTarget[i].target = p.transform;

            p.GetComponent<BasicPlayerControll>().ID = i;
            p.GetComponent<BasicPlayerControll>().Color = pData.colorList[i];

            switch (GameMode)
            {
                case MainGameManager.gameMode.foodBattle:
                    p.AddComponent<foodBattlePlayer>().init();
                    break;
                case MainGameManager.gameMode.fallingBattle:
                    break;
                case MainGameManager.gameMode.deathBattle:
                    break;
                default:
                    Console.WriteLine("未鎖定");
                    break;
            }
        }
    }

    void startGame() 
    {
        for(int i = 0;i< playerNum; i++)
            playerList[i].GetComponent<PlayerStateList>().pause = false;


        switch (GameMode)
        {
            case MainGameManager.gameMode.foodBattle:
                foodBattleManager.instance.init();
                break;
            case MainGameManager.gameMode.fallingBattle:
                fallingGameManager.instance.init();
                break;
            case MainGameManager.gameMode.deathBattle:
                break;
            default:
                Console.WriteLine("未鎖定");
                break;
        }

    }

    public void gameOver() 
    {
        int Last = 0;

        for (int i = 0; i < playerNum; i++)
        {
            if (playerList[i].GetComponent<PlayerStateList>().dead)
            {
                Last++;
            }
        }

        if (Last == playerNum - 1)
        {
            Debug.Log("遊戲結束");

            for (int i = 0; i < playerNum; i++)
                playerList[i].GetComponent<PlayerStateList>().pause = true;

            switch (GameMode)
            {
                case MainGameManager.gameMode.foodBattle:
                    foodBattleManager.instance.endGame();
                    break;
                case MainGameManager.gameMode.fallingBattle:
                    fallingGameManager.instance.endGame();
                    break;
                case MainGameManager.gameMode.deathBattle:
                    break;
                default:
                    Console.WriteLine("未鎖定");
                    break;
            }
        }
    }

    public enum gameMode 
    {
        none,
        tuto,
        foodBattle,
        fallingBattle,
        deathBattle,
    }
}