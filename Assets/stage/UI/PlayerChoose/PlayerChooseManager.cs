using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerChooseManager : MonoBehaviour
{

    public static PlayerChooseManager instance;

    [Header("ª±®a¿ï¾Ü")]
    [SerializeField] private int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerNum = 0;
    }

    public void playerJoinEvent(PlayerInput playerInput) 
    {
        playerNum++;
    }
}
