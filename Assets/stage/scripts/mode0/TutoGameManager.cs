using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoGameManager : MonoBehaviour
{

    public static TutoGameManager instance;

    public int playerNum;

    private Dictionary<int, GameObject> playerList;
    public bool[] playerReadyState;

    public void Awake()
    {
        instance = this;
    }


    public void init(int _playerNum, Dictionary<int, GameObject> _playerList)
    {
        playerNum = _playerNum;
        playerReadyState = new bool[playerNum];
        playerList = _playerList;
    }


    public void playerreadytoGame() 
    {
        if(playerReadyCheck())
            Debug.Log("完成");
        else
            Debug.Log("還沒完成");
    }

    bool playerReadyCheck()
    {
        int p = 0;

        for (int a = 0; a < playerNum; a++) 
        {
            playerReadyState[a] = playerList[a].GetComponent<tutoPlayer>().ready;

            if (playerReadyState[a])
                p++;
        }

        Debug.Log(p + "," + playerNum);

        if (p >= playerNum)
            return true;
        else
            return false;
    }
}
