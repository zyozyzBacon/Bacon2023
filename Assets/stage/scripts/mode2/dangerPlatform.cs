using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dangerPlatform : MonoBehaviour
{
    public bool active;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void dangerOn()
    {
        if (!active) 
        {
            active = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadZone")
        {

        }
    }

}
