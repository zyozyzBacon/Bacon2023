using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerPlatform : MonoBehaviour
{
    public bool active;

    public void dangerOn()
    {
        if (!active) 
        {
            active = true;
        }
    }

}
