using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoGameManager : MonoBehaviour
{

    public static TutoGameManager instance;

    public int playerNum;

    private Dictionary<int, GameObject> playerList;
    public bool[] playerReadyState;

    public void Awake()
    {
        instance = this;
    }


    public void init(int _playerNum, Dictionary<int, GameObject> _playerList)
    {
        playerNum = _playerNum;
        playerReadyState = new bool[playerNum];
        playerList = _playerList;
    }


    public void playerreadytoGame()
    {
        if (playerReadyCheck())
        {
            StartCoroutine(nextLevel());
        }
    }

    IEnumerator nextLevel() 
    {
        for (int i = 0; i < playerNum; i++) 
        {
            playerList[i].GetComponent<PlayerStateList>().pause = true;
        }

        yield return new WaitForSeconds(1f);

        loadscene();
    }

    void loadscene() 
    {
        SceneManager.LoadScene("Level1");
    }



    bool playerReadyCheck()
    {
        int p = 0;

        for (int a = 0; a < playerNum; a++) 
        {
            playerReadyState[a] = playerList[a].GetComponent<tutoPlayer>().ready;

            if (playerReadyState[a])
                p++;
        }

        Debug.Log(p + "," + playerNum);

        if (p >= playerNum)
            return true;
        else
            return false;
    }
}
