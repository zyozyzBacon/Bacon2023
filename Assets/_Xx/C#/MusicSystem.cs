using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    AudioSource audio;
    public AudioClip Idle, walk, hit,jump, leaflet;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void IdleMusic()
    {
        audio.PlayOneShot(Idle, 0.7f);
    }
    public void WalkMusic()
    {
        audio.PlayOneShot(walk, 0.7f);
    }
    public void HitMusic()
    {
        audio.PlayOneShot(hit, 0.7f);
    }
    public void JumpMusic()
    {
        audio.PlayOneShot(jump, 0.7f);
    }
    public void Leafletp()
    {
        audio.PlayOneShot(leaflet, 1f);
    }
}
