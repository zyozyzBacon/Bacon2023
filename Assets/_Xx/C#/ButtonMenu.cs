using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    
    public void PlayGame(string sceneName)//¤Á´«³õ´º
    {
        AudioManager.Button2Audio();
        SceneManager.LoadScene(sceneName);
    }
    public void PlaySelect()//¤Á´«¿ï¾Ü
    {
        AudioManager.ButtonAudio();
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
        
    }
    public void Comics(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SKipInput(InputAction.CallbackContext context)
    {
        anim.SetBool("Skip", true);

    }
       
}

