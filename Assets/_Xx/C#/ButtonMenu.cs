using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    public GameObject SetButton, SoundButton;
    public void SetSelect()//�]�w���
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 0);
        SoundButton.SetActive(true);
    }
    public void SetClose()//�]�w���}
    {
        SetButton.transform.rotation = Quaternion.Euler(0, 0, 45);
        SoundButton.SetActive(false);
    }
    public void Sound()//�n��
    {

    }
    public void Skip(string sceneName)//���L
    {
        if(Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    public void GameStart(string sceneName)//�C���}�l
    {
        SceneManager.LoadScene(sceneName);
    }
    public void GameSelect(string sceneName)//������d
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

