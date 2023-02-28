using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;


    [Header("¹D¨ã¦Cªí")]
    public GameObject[] ItemList;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    public enum foodColor 
    {
        none,
        white,
        black,
    }
    
}
