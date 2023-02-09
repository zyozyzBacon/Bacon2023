using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Drawing;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.InputSystem.XInput;

public class PlayerChooseColorManager : MonoBehaviour
{
    public static PlayerChooseColorManager instance;

    public int playerNum;

    public Image[] colorPlace;
    public int[] playerColorList;

    public MainGameManager.gameplayMode gameplayMode;
    public MainGameManager.gameMode gameMode;

    [SerializeField] private GameObject pDataObject;
    public playerData pData;

    [SerializeField]private bool stop;

    private InputActionAsset[] playerInputs = new InputActionAsset[4];
    [SerializeField] private Sprite[] playerSprites = new Sprite[4];
    [SerializeField] public Vector3[] playerIconColor = new Vector3[4];

    void Awake()
    {
        instance = this;

        playerColorList = new int[4];
        for (int i = 0 ; i < playerColorList.Length; i++) 
        {
            playerColorList[i] = -1;

        }

        DontDestroyOnLoad(pDataObject);
    }

    public void AddPlayerToList(PlayerInput playerInput)
    {
        if (playerNum < 4)
        {
            GameObject player = playerInput.gameObject;

            player.GetComponent<chooseColorControll>().playerID = playerNum;
            player.GetComponent<Image>().sprite = playerSprites[playerNum];

            player.GetComponent<chooseColorControll>().pos = new Image[4];
            for (int i = 0; i < 4;i++) 
            {
                player.GetComponent<chooseColorControll>().pos[i] = colorPlace[i].transform.GetChild(playerNum).GetComponent<Image>();
            }

            player.transform.position = player.GetComponent<chooseColorControll>().pos[0].transform.position;

            player.transform.parent = player.GetComponent<chooseColorControll>().pos[0].transform;
            player.GetComponent<chooseColorControll>().colorPanelCheck(-1,0);

            playerInputs[playerNum] = player.GetComponent<PlayerInput>().actions;

            playerNum++;


        }
        else 
        {
            Debug.Log("已達四位玩家 人數上限");
        }
    }

    public void readyToGame() 
    {
        if (playerNum > 0)
        {
            if (playerReadyCheck())
            {
                if (!stop) 
                {
                    stop = true;
                    Debug.Log("開始遊戲");
                    loadgame();

                    pData.playerNum = playerNum;
                    for (int i = 0; i < playerNum; i++)
                    {
                        pData.colorList[i] = playerColorList[i];
                    }
                }

            }
            else 
            {
                Debug.Log("有人還沒選顏色");
            }
        }
        else 
        {
            Debug.Log("需要四個人才能遊玩");
        }
    
    }

    public bool colorCheck(int color) 
    {
        int p = 0;

        for (int i = 0; i < playerColorList.Length; i++)
        {
            if (playerColorList[i] == color)
                p++;
        }


        if (p == 0)
            return true;
        else
            return false;
    }


    bool playerReadyCheck() 
    {
        int p = 0;

        for (int i = 0; i < playerColorList.Length; i++)
        {
            if (playerColorList[i] != -1)
                p++;
        }

        if (p >= playerNum)
            return true;
        else
            return false;
    }

    public void loadgame() 
    {
        pData.gameplayMode = gameplayMode;

        if (gameplayMode != MainGameManager.gameplayMode.longBattle)
        {
            pData.gameMode = gameMode;

            switch (gameMode)
            {
                case MainGameManager.gameMode.tuto:
                    SceneManager.LoadScene("Level0");
                    break;
                case MainGameManager.gameMode.foodBattle:
                    SceneManager.LoadScene("Level1");
                    break;
                case MainGameManager.gameMode.fallingBattle:
                    SceneManager.LoadScene("betaStage02");
                    break;
                case MainGameManager.gameMode.deathBattle:
                    SceneManager.LoadScene("betaStage03");
                    break;
                default:
                    Console.WriteLine("未鎖定");
                    break;
            }
        }
        else 
        {
            pData.gameMode = MainGameManager.gameMode.tuto;
            SceneManager.LoadScene("Level0");
        }
    }


}
