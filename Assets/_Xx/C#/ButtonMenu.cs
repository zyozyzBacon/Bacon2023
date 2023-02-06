using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    public GameObject SetButton, SoundButton;
    public void SetSelect()
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        SoundButton.SetActive(true);
    }
    public void SetClose()
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 45);
        SoundButton.SetActive(false);
    }
    public void Sound()
    {

    }

    public void GameStart(string sceneName)
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

