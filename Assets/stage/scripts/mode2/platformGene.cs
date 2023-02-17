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

    public GameObject[] platformsArray;
    int p;

    public GameObject[] sidePoint = new GameObject[2];
    int i;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        platformsArray = new GameObject[fallingObject.transform.childCount];

        for (int i = 0; i< fallingObject.transform.childCount; i++) 
        {      
            platformsArray[i] = Instantiate(fallingObject.transform.GetChild(i).gameObject);
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

        if (active) 
        {
            int r = Random.Range(0, fallingObject.transform.childCount);

            while (platformsArray[r].activeSelf == true)
            {
                r = Random.Range(0, fallingObject.transform.childCount);
            }

            platformsArray[r].SetActive(true);
            platformsArray[r].transform.parent =  GameObject.Find("###平台生成位置###").transform;
            platformsArray[r].GetComponent<fallingPart>().active = true;

            p++;
            if (p >= 10) 
            {
                p = 0;
                //platformsArray[r].AddComponent<dangerPlatform>();
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
