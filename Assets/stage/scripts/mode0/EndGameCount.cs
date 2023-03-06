using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameCount : MonoBehaviour
{
    [SerializeField] private GameObject EndGameCanvas;
    [SerializeField] private GameObject[] panel = new GameObject[4];
    [SerializeField] private TextMeshProUGUI[] bubbleText = new TextMeshProUGUI[4];

    public static EndGameCount intence;

    private void Awake()
    {
        intence = this;
        EndGameCanvas.SetActive(false);
    }

    public void init(playerData pData)
    {
        EndGameCanvas.SetActive(true);

        for (int i = 0; i < 4; i++)
            panel[i].SetActive(false);

        for (int i = 0;i < pData.playerNum;i++)
        {
            panel[i].SetActive(true);
            bubbleText[i].SetText(pData.bubbleNum[i].ToString());
        }
    }
}
