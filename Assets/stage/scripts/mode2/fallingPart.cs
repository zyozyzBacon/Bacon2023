using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPart : MonoBehaviour
{

    public bool active;

    public int speed;

    Rigidbody2D rb;

    public GameObject parent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (active)
            rb.velocity = Vector2.down * speed;
        else
            rb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadZone") 
        {
            if (parent != null)
            {
                this.gameObject.SetActive(false);
                this.gameObject.transform.parent = parent.transform;
                this.gameObject.transform.position = parent.transform.position;
                active = false;
            }
            else
                Destroy(this.gameObject);
        }
    }
}
