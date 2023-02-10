using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutoPlayer : MonoBehaviour
{
    public bool ready;

    private PlayerUI pUI;

    public void Awake()
    {
        pUI = GetComponent<PlayerUI>();
    }

    public void readyInput()
    {
        ready = !ready;

        pUI.uiPart.GetComponent<Image>().enabled = ready;

        if (TutoGameManager.instance != null)
            TutoGameManager.instance.playerreadytoGame();
    }

}
