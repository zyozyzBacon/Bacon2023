using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brokenPart : MonoBehaviour
{
    public float time;
    Rigidbody2D rb;

    bool triggerOn;
    bool brokenOn;
    Collider2D Collider;

    GameObject partL;
    GameObject partR;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();

        partL = this.gameObject.transform.GetChild(0).gameObject;
        partR = this.gameObject.transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggerOn) 
            {
                triggerOn = true;
                StartCoroutine(timer(time));
            }
        }
    }


    IEnumerator timer(float sceonds)
    {
        yield return new WaitForSeconds(sceonds);
        brokenOn = true;

        partL.GetComponent<Rigidbody2D>().velocity = rb.velocity * 2;
        partR.GetComponent<Rigidbody2D>().velocity = rb.velocity * 2;

        Destroy(Collider);

    }

    private void Update()
    {
        if (brokenOn) 
        {
            partL.transform.Rotate(0,0,5);
            partR.transform.Rotate(0, 0, -5);
        }
    }
}
