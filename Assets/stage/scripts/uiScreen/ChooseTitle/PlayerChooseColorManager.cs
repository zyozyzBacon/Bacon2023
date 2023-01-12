using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Drawing;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class PlayerChooseColorManager : MonoBehaviour
{
    public static PlayerChooseColorManager instance;

    public int playerNum;

    public Image[] colorPlace;
    public int[] playerColorList;

    [SerializeField] private GameObject pDataObject;
    public playerData pData;

    [SerializeField]private bool stop;

    private InputActionAsset[] playerInputs = new InputActionAsset[4];

    // Start is called before the first frame update
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
        if (playerNum < 3)
        {
            GameObject player = playerInput.gameObject;

            player.GetComponent<chooseColorControll>().playerID = playerNum;
            player.transform.parent = GameObject.Find("Canvas").transform;

            player.GetComponent<chooseColorControll>().pos = new Image[4];
            for (int i = 0; i < 4;i++) 
            {
                player.GetComponent<chooseColorControll>().pos[i] = colorPlace[i].transform.GetChild(playerNum).GetComponent<Image>();
            }

            player.transform.position = player.GetComponent<chooseColorControll>().pos[0].transform.position;

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
                    SceneManager.LoadScene("TestMain");
                    

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
}
