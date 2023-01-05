using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class platformGene : MonoBehaviour
{

    public float moveSpeed;
    public float randomMoveTimer;

    public bool active;

    public GameObject[] platforms = new GameObject[2];

    public GameObject[] sidePoint = new GameObject[2];
    int i;

    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

        transform.position = Vector2.MoveTowards(this.transform.position, sidePoint[i].transform.position, moveSpeed * Time.deltaTime);
    }


    void randomMove() 
    {
    
    }

}
