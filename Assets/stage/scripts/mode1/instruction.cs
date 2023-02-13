using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instruction : MonoBehaviour
{
    public static instruction instance;

    public int TextPhase;
    int current;
    public bool ActionAllow;
    
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
