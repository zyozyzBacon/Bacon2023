using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SetButton : MonoBehaviour
{
    public GameObject Set, Button, EventSystem ,One,Two;
    int a = 0;

    public void SetInput(InputAction.CallbackContext context)
    {
        if (a == 0)
        {
            AudioManager.ButtonAudio();
            Set.transform.rotation = Quaternion.Euler(0, 0, 0);
            a = 1;
            Button.SetActive(true);
            EventSystem.SetActive(false);
            One.SetActive(false);
            Two.SetActive(true);
        }
        else if (a == 1)
        {
            AudioManager.ButtonAudio();
            Set.transform.rotation = Quaternion.Euler(0, 0, 45);
            a = 0;
            Button.SetActive(false);
            EventSystem.SetActive(true);
            One.SetActive(true);
            Two.SetActive(false);
        }

    }
    public void PlayGame(string sceneName)//切換場景
    {
        AudioManager.Button2Audio();
        SceneManager.LoadScene(sceneName);
    }
    public void PlaySelect()//切換選擇
    {
        AudioManager.ButtonAudio();
    }
    public void Sound()//聲音
    {

    }


}