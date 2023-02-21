using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    public GameObject SetButton, Button;
    public void SetSelect()//設定選擇
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        Button.SetActive(true);
    }
    public void SetClose()//設定離開
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 45);
        Button.SetActive(false);
    }
    public void Sound()//聲音
    {

    }
    public void Home(string sceneName)//回主頁
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GameStart(string sceneName)//遊戲開始
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GameSelect(string sceneName)//選擇關卡
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Quiit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Skip();
    }
    void Skip()//跳過
    {
        if (Input.GetKey(KeyCode.N))
        {
            anim.SetBool("Skip", true);
        }
    }
}

