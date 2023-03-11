using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager instance;

    public Dictionary<int, GameObject> playerList;

    [SerializeField] public gameMode GameMode;

    [SerializeField] private int playerNum;
    [SerializeField] private Transform[] playerSpawn;
    [SerializeField] public int[] ammo = new int[4];
    [SerializeField] public GameObject TimerText;
    [SerializeField] private bool cameraLocked;
    [SerializeField] private GameObject[] bubbblePlayer = new GameObject[4];
    [SerializeField] private Sprite[] playerIcon = new Sprite[4];
    [SerializeField] public Vector3[] playerIconColor = new Vector3[4];
    [SerializeField] public bool InstructionBool;
    [SerializeField] private GameObject InstructionCanvas;
    [SerializeField] public float totalTime;
    private float timeLeft;
    private bool GameOver;
    [SerializeField] public TextMeshProUGUI textTime;

    private playerData pData;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        init();

        if (InstructionCanvas != null)
        {
            instruction.instance.init();
        }
        else
        {
            startGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Numlock)) 
        {
            AudioManager.StopAudio();
            loadScene();
        }
    }


    void loadScene() 
    {
        if (!GameOver) 
        {
            SceneManager.LoadScene("Home");
        }
        else 
        {
            if (pData.gameplayMode == gameplayMode.longBattle)
            {
                switch (GameMode)
                {
                    case gameMode.tuto:
                        break;
                    case gameMode.foodBattle:
                        SceneManager.LoadScene("Level2");
                        break;
                    case gameMode.fallingBattle:
                        //SceneManager.LoadScene("Level3");
                        break;
                    case gameMode.deathBattle:
                        break;
                }
            }
            else 
            {
                SceneManager.LoadScene("Home");
            }
        }

    }

    private void init()
    {

        if (GameObject.Find("###PlayerData###").GetComponent<playerData>() != null)
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
    
            p.GetComponent<BasicPlayerControll>().ID = i;
            p.GetComponent<BasicPlayerControll>().Color = pData.colorList[i];

            switch (GameMode)
            {
                case gameMode.tuto:
                    p.AddComponent<tutoPlayer>();
                    break;
                case gameMode.foodBattle:
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

            p.GetComponent<PlayerUI>().PlayerIcon = playerIcon[i];
            p.GetComponent<PlayerUI>().init();
        }
    }

    public void startGame() 
    {
        for(int i = 0;i < playerNum; i++)
            playerList[i].GetComponent<PlayerStateList>().pause = false;


        switch (GameMode)
        {
            case gameMode.tuto:
                TutoGameManager.instance.init(playerNum, playerList);
                break;
            case gameMode.foodBattle:
                foodBattleManager.instance.init();
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

        if (totalTime != -1) 
        {
            timeLeft = totalTime;
            StartCoroutine(Countdown());
        }

    }


    IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            textTime.SetText(timeLeft.ToString());
            Debug.Log(timeLeft);
        }

        for (int i = 0; i < playerNum; i++)
            playerList[i].GetComponent<PlayerStateList>().pause = true;

        endGame();
    }

    public void playerDieToEndGame() 
    {
        int l = 0;
        for (int i = 0; i < playerNum; i++) 
        {
            if (playerList[i].GetComponent<PlayerStateList>().dead)
                l++;
        }

        if (l >= playerNum - 1) 
        {
            endGame();
        }
    }

    void endGame() 
    {
        GameOver = true;
        switch (GameMode)
        {
            case gameMode.foodBattle:
                foodBattleManager.instance.endGame();
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

        writeInit();
        EndGameCount.intence.init(pData);
    }

    public void writeInit() 
    {
        for (int i = 0; i < playerNum; i++)
            pData.bubbleNum[i] = playerList[i].GetComponent<BasicPlayerControll>().bubbles;
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