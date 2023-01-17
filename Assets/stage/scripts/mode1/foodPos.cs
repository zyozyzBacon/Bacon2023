using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodPos : MonoBehaviour
{
    public bool playerAround;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerAround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerAround = false;
        }
    }
}
