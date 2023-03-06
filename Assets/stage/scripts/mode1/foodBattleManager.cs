using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemManager;

public class foodBattleManager : MonoBehaviour
{

    public static foodBattleManager instance;

    [Header("�Ҧ����k����")]

    [Tooltip("���x�Ұ��I")][SerializeField] private GameObject PlatCore;

    public void Awake()
    {
        instance = this;
    }


    public void init()
    {
        PlatCore.GetComponent<platCore>().Active = true;
    }

    public void endGame() 
    {
        PlatCore.GetComponent<platCore>().Active = false;
    }

}
