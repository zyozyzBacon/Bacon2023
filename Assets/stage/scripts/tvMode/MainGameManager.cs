using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static tvPartGameplay;

public class MainGameManager : MonoBehaviour
{

    public static MainGameManager instance;

    public bool gaming;
    
    public Dictionary<int, GameObject> playerList;

    [SerializeField] private int playerNum;
    [SerializeField] private Transform[] playerSpawn;
    [SerializeField] public int[] ammo = new int[4];
    [SerializeField] public GameObject TimerText;
    

    CinemachineTargetGroup.Target[] cameraTarget;

    private void Awake()
    {
        instance = this;
        playerList = new Dictionary<int, GameObject>();
        playerNum = 0;

        cameraTarget = instance.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets;
    }

    private void Start()
    {
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
        playerPrefab.AddComponent<foodBattlePlayer>().init();
        playerNum++;
    }


}