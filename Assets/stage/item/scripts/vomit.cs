using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vomit : MonoBehaviour, IitemInterface
{

    public Sprite Icon;
    public float seconds;

    private GameObject Player;
    private GameObject Target;
    private Collider2D Collider;

    void Awake() 
    {
        Collider = this.GetComponent<Collider2D>();
    }

    void IitemInterface.ItemTrigger(GameObject player)
    {
        this.transform.position = player.transform.position;
        Player = player;
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(Collider, playerCollider);
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
