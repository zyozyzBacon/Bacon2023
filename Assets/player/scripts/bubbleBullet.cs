using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleBullet : MonoBehaviour
{
    public GameObject parent;

    public float speed;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right* speed;
    }
}
