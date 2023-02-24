using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    AudioSource audio;
    public AudioClip walk1, walk2, hit,jump;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void StopMusic()
    {
        audio.Stop();
    }
    public void walk1Music()
    {
        audio.PlayOneShot(walk1, 0.7f);
    }
    public void walk2Music()
    {
        audio.PlayOneShot(walk2, 0.7f);
    }
    public void HitMusic()
    {
        audio.PlayOneShot(hit, 0.7f);
    }
    public void JumpMusic()
    {
        audio.PlayOneShot(jump, 0.7f);
    }
    
}
