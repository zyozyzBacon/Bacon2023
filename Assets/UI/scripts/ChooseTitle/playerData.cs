using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerData : MonoBehaviour
{
    public int playerNum;
    public int[] colorList = new int[4];

    public MainGameManager.gameplayMode gameplayMode;
    public MainGameManager.gameMode gameMode;
}
