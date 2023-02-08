using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private GameObject[] bubbblePlayer = new GameObject[4];
    [SerializeField] private Sprite[] playerIcon =  new Sprite[4];

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

        for (int i = 0; i < playerNum; i++)
        {
            GameObject p = bubbblePlayer[pData.colorList[i]];
            this.gameObject.GetComponent<PlayerInputManager>().playerPrefab = p;
            p = this.gameObject.GetComponent<PlayerInputManager>().JoinPlayer().gameObject;

            playerList.Add(i, p);
            p.transform.position = playerSpawn[i].position;

            p.GetComponent<PlayerStateList>().pause = true;

            if (i % 2 == 0)
            {
                p.transform.localScale = new Vector3(-Mathf.Abs(p.transform.localScale.x), p.transform.localScale.y, p.transform.localScale.z);
            }

            p.GetComponent<PlayerUI>().PlayerIcon = playerIcon[playerNum];
            p.GetComponent<BasicPlayerControll>().ID = i;
            p.GetComponent<BasicPlayerControll>().Color = pData.colorList[i];

            switch (GameMode)
            {
                case gameMode.foodBattle:
                    p.AddComponent<foodBattlePlayer>().init();
                    p.GetComponent<PlayerUI>().init();
                    break;
                case gameMode.fallingBattle:
                    break;
                case gameMode.deathBattle:
                    p.GetComponent<BasicPlayerControll>().allowAttack = true;
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
            case gameMode.tuto:
                TutoGameManager.instance.init(playerNum);
                break;
            case gameMode.foodBattle:
                foodBattleManager.instance.init();
                for (int i = 0; i < playerNum; i++)
                    playerList[i].GetComponent<foodBattlePlayer>().startgame();
                break;
            case gameMode.fallingBattle:
                fallingGameManager.instance.init();
                break;
            case gameMode.deathBattle:
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
                case gameMode.foodBattle:
                    foodBattleManager.instance.endGame(playerList);
                    break;
                case gameMode.fallingBattle:
                    fallingGameManager.instance.endGame();
                    break;
                case gameMode.deathBattle:
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

    public enum gameplayMode
    {
        test,
        longBattle,
        freeplay,
    }
}