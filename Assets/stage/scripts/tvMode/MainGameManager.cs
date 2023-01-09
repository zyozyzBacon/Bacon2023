using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static tvPartGameplay;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager instance;

    public Dictionary<int, GameObject> playerList;

    [SerializeField]private int playerNum;
    [SerializeField] private Transform[] playerSpawn;

    CinemachineTargetGroup.Target[] cameraTarget;

    public enum playerTeam 
    {
        none,
        A,
        B
    }

    private void Awake()
    {
        instance = this;
        playerList = new Dictionary<int, GameObject>();
        playerNum = 0;

        cameraTarget = instance.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets;

        ItemManager.instance.remoteTaken();
    }

    public void AddPlayerToList(GameObject playerPrefab) 
    {
        playerList.Add(playerNum, playerPrefab);
        cameraTarget[playerNum].target = playerPrefab.transform;
        playerPrefab.transform.position = playerSpawn[playerNum].position;
        //playerPrefab.GetComponent<PlayerStateList>().pause = true;
        if (playerNum == 0 || playerNum == 2) 
        {
            playerPrefab.GetComponent<SpriteRenderer>().flipX= true;
        }
       
        playerNum++;
    }


}