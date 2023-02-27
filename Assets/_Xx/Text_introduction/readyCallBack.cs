using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyCallBack : MonoBehaviour
{
    public void callReady()
    {
        MainGameManager.instance.startGame();
        instruction.instance.ReadyGoPanel.SetActive(true);
    }
    public void level1Music()
    {
        AudioManager.Level1Audio();
    }
}
