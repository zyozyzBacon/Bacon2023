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
    private playerData pData;

    private void Awake()
    {
        instance = this;
        init();
    }

    private void Start()
    {
        ItemManager.instance.remoteTaken();
    }


    private void init()
    {
        pData = GameObject.Find("###PlayerData###").GetComponent<playerData>();

        playerList = new Dictionary<int, GameObject>();
        playerNum = pData.playerNum;

        cameraTarget = instance.gameObject.GetComponent<CinemachineTargetGroup>().m_Targets;

        for (int i = 0; i < playerNum;i++) 
        {
            GameObject p = this.gameObject.GetComponent<PlayerInputManager>().JoinPlayer().gameObject;

            playerList.Add(i, p);
            cameraTarget[i].target = p.transform;
            p.transform.position = playerSpawn[i].position;

            //playerPrefab.GetComponent<PlayerStateList>().pause = true;

            if (i % 2 == 0)
            {
                p.transform.localScale = new Vector3(-Mathf.Abs(p.transform.localScale.x), p.transform.localScale.y, p.transform.localScale.z);
            }

            p.GetComponent<BasicPlayerControll>().ID = i;
            p.GetComponent<BasicPlayerControll>().Color = pData.colorList[i];

            p.AddComponent<foodBattlePlayer>().init();
        }
      
    }
}