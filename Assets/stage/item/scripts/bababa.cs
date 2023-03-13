using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bababa : MonoBehaviour, IitemInterface
{
    public Sprite Icon;
    public float seconds;

    private GameObject Player;
    private GameObject Target;
    private Collider2D Collider;

    private Rigidbody2D rb;

    void Awake()
    {
        Collider = this.GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() 
    {
        rb.velocity = (rb.transform.right * 8);
    }

    void IitemInterface.ItemTrigger(GameObject player)
    {
        transform.position = player.transform.position;
        transform.rotation = player.transform.rotation;
        Player = player;
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(Collider, playerCollider);
        StartCoroutine(timer(seconds));
    }


    IEnumerator timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!collision.GetComponent<PlayerStateList>().dead)
            {
                if (!collision.GetComponent<PlayerStateList>().recoverying)
                    collision.GetComponent<BasicPlayerControll>().damaged(collision.transform.parent.gameObject);
            }
        }
    }
}
