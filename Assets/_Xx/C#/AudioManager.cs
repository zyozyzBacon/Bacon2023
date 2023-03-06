using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    static AudioManager music;

    [Header("關卡音樂")]
    public AudioClip Level1Clip; 
    public AudioClip Level2Clip;
    public AudioClip Level3Clip;
    [Header("按鍵音效")]
    public AudioClip ButtonClip;//左右選擇時
    public AudioClip Button2Clip;//按下按鍵時

    AudioSource LevelSource;//關卡音樂
    AudioSource ButtonSource;//按鍵音效
    

    private void Awake()
    {
        music = this;

        DontDestroyOnLoad(gameObject);

        LevelSource = gameObject.AddComponent<AudioSource>();
        ButtonSource= gameObject.AddComponent<AudioSource>();
    }
    public static void Level1Audio()
    {
        music.LevelSource.clip = music.Level1Clip;
        music.LevelSource.loop = true;
        music.LevelSource.volume = 0.5f;
        music.LevelSource.Play();
    }
    public static void ButtonAudio()
    {
        music.ButtonSource.clip = music.ButtonClip;
        music.ButtonSource.Play();
    }
    public static void Button2Audio()
    {
        music.ButtonSource.clip = music.Button2Clip;
        music.ButtonSource.Play();
    }
    public static void StopAudio()
    {
        music.LevelSource.Stop();

    }






}
