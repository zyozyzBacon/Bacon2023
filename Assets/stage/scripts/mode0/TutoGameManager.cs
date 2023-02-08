using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoGameManager : MonoBehaviour
{

    public static TutoGameManager instance;

    public int playerNum;

    public bool[] playerReadyState;

    public void Awake()
    {
        instance = this;
    }


    public void init(int playerNum)
    {
        playerReadyState = new bool[playerNum];
    }

    public void playerCheck() 
    {
    
    }

    bool playerReadyCheck()
    {
        int p = 0;

        for (int i = 0; i < playerNum; i++)
        {
            if (playerReadyState[i] != false)
                p++;
        }

        if (p >= playerNum)
            return true;
        else
            return false;
    }
}
