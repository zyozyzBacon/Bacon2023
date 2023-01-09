using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinMananger : MonoBehaviour
{
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        MainGameManager.instance.AddPlayerToList(playerInput.gameObject);
        
    }
}
