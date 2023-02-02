using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleGene : MonoBehaviour
{

    public float moveSpeed;
    public float randomMoveTimer;
    public float ptTimer;

    private bool active;

    public GameObject fallingObject;

   
    public GameObject[] sidePoint = new GameObject[2];
    int i;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(timer(randomMoveTimer));
        StartCoroutine(platform(ptTimer));
    }

    // Update is called once per frame
    void Update()
    {
        active = transform.parent.GetComponent<platCore>().Active;
        if (active)
        {
            move();
        }
    }

    void move()
    {
        if (Vector2.Distance(transform.position, sidePoint[i].transform.position) < 0.01f)
        {
            i++;
            if (i == sidePoint.Length)
                i = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, sidePoint[i].transform.position, moveSpeed * Time.deltaTime);
    }


    void randomMove()
    {
        moveSpeed = Random.Range(5, 20);
        randomMoveTimer = Random.Range(3f, 8);
        transform.position = new Vector3(
            Random.Range(sidePoint[0].transform.position.x, sidePoint[1].transform.position.x),
            transform.position.y,
            transform.position.z);
    }

    IEnumerator platform(float sceonds)
    {
        yield return new WaitForSeconds(sceonds);

        ptTimer = Random.Range(0.75f, 3.0f);

        if (active)
        {
            GameObject bubble = Instantiate(fallingObject,this.gameObject.transform);
            bubble.transform.parent = null;
            bubble.GetComponent<fallingPart>().active = true;
        }

        StartCoroutine(platform(ptTimer));
    }


    IEnumerator timer(float sceonds)
    {
        yield return new WaitForSeconds(sceonds);

        if (active)
            randomMove();

        StartCoroutine(timer(randomMoveTimer));
    }
}
