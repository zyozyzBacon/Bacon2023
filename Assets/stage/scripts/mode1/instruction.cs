using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class instruction : MonoBehaviour
{
    public static instruction instance;

    public int TextPhase;
    int current;
    public bool ActionAllow;

    public GameObject[] ReadyIcon;

    public GameObject InstructionPanel;
    public GameObject[] Text;
    public GameObject ReadyGoPanel;

    private void Awake()
    {
        instance = this;
    }

    public void init() 
    {
        current = 0;
        Text[current].SetActive(true);
        ActionAllow = false;
    }

    public void playerReaded(int id) 
    {
        ReadyIcon[id].GetComponent<Image>().enabled = true;

        int r = 0;

        for (int i = 0;i < MainGameManager.instance.playerList.Count;i++) 
        {
            if (MainGameManager.instance.playerList[i].GetComponent<PlayerStateList>().readed)
                r++;
        }

        if (r >= MainGameManager.instance.playerList.Count) 
        {
            for (int i = 0; i < MainGameManager.instance.playerList.Count; i++)
            {
                MainGameManager.instance.playerList[i].GetComponent<PlayerStateList>().readed = false;
                ReadyIcon[i].GetComponent<Image>().enabled = false;
            }
            nextPhase();
        }
    }

    public void nextPhase() 
    {
        if (ActionAllow) 
        {
            current++;
            if (current < TextPhase)
            { 
                Text[current - 1].SetActive(false);
                Text[current].SetActive(true);
                ActionAllow = false;
            }
            else 
            {
                InstructionPanel.SetActive(false);
                readyPhase();
            }
        }
    }

    void readyPhase() 
    {
        ReadyGoPanel.SetActive(true);
    }

}
