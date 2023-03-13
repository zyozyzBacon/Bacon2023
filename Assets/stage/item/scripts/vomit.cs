using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vomit : MonoBehaviour
{

    public Sprite Icon;
    public float seconds;

    private GameObject Target;

    void Awake() 
    {
        StartCoroutine(timer(seconds));
    }



    IEnumerator timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (Target != null) 
        {
            Target.GetComponent<BasicPlayerControll>().walkSpeed = 9;
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.tag == "Player")
        {
            if (!collision.GetComponent<PlayerStateList>().dead) 
            {
                Target = collision.gameObject;
                Target.GetComponent<BasicPlayerControll>().walkSpeed = 2;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.GetComponent<PlayerStateList>().dead)
            {
                Target = collision.gameObject;
                Target.GetComponent<BasicPlayerControll>().walkSpeed = 9;
            }
        }
    }
}
