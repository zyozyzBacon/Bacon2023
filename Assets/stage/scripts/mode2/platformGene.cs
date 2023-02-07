using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class platformGene : MonoBehaviour
{

    public float moveSpeed;
    public float randomMoveTimer;
    public float ptTimer;

    private bool active;

    public GameObject fallingObject;

    public GameObject[] platformsArray = new GameObject[10];
    int p;

    public GameObject[] sidePoint = new GameObject[2];
    int i;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i< 10; i++) 
        {
            platformsArray[i] = Instantiate(fallingObject);
            platformsArray[i].transform.parent = this.transform;
            platformsArray[i].transform.position = this.transform.position;
            platformsArray[i].GetComponent<fallingPart>().parent = this.gameObject;
            platformsArray[i].SetActive(false);
        }


        p = 0;
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
        moveSpeed = Random.Range(5,15);
        randomMoveTimer = Random.Range(2, 3);
        transform.position = new Vector3(
            Random.Range(sidePoint[0].transform.position.x, sidePoint[1].transform.position.x),
            transform.position.y,
            transform.position.z);
    }

    IEnumerator platform(float sceonds) 
    {
        yield return new WaitForSeconds(sceonds);

        ptTimer = Random.Range(0.75f, 2f);

        if (active) 
        {
            platformsArray[p].SetActive(true);
            platformsArray[p].transform.parent =  GameObject.Find("###平台生成位置###").transform;
            platformsArray[p].GetComponent<fallingPart>().active = true;


            p++;
            if (p >= 10) 
            {
                p = 0;
                //將平台改造成危險平台
            }            
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
