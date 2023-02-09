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


    public void init(int playerNum, Dictionary<int, GameObject> _playerList)
    {
        playerReadyState = new bool[playerNum];
        playerList = _playerList;
    }


    public void playerreadytoGame() 
    {
        Debug.Log(playerReadyCheck());
    }

    bool playerReadyCheck()
    {
        int p = 0;

        for (int a = 0; a < playerNum; a++) 
        {
            playerReadyState[a] = playerList[a].GetComponent<tutoPlayer>().ready;
        }

        for (int i = 0; i < playerNum; i++)
        {
            if (playerReadyState[i])
                p++;
        }

        if (p >= playerNum)
            return true;
        else
            return false;
    }
}
