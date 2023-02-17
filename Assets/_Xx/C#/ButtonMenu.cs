using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    public GameObject SetButton, SoundButton;
    public void SetSelect()//設定選擇
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        SoundButton.SetActive(true);
    }
    public void SetClose()//設定離開
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 45);
        SoundButton.SetActive(false);
    }
    public void Sound()//聲音
    {

    }
    public void Skip(string sceneName)//跳過
    {
        if(Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(sceneName);
        }
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
    void Start()
    {

    }

    void Update()
    {

    }

}

