using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager mainGameManager;

    public Dictionary<int, GameObject> playerList;

    private int playerNum;


    public enum playerTeam 
    {
        none,
        A,
        B
    }

    private void Awake()
    {
        mainGameManager = this;
        playerList = new Dictionary<int, GameObject>();
        playerNum = 0;
    }

    public void AddPlayerToList(GameObject playerPrefab) 
    {
        playerList.Add(playerNum, playerPrefab);
        playerNum++;
    }


}