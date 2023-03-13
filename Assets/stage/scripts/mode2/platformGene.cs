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
    public GameObject brokenFallingObject;
    int brokenRamdon;

    public GameObject[] platformsArray;
    public GameObject[] brokenPlatformsArray;
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


        brokenPlatformsArray = new GameObject[brokenFallingObject.transform.childCount];
        brokenRamdon = 0;
        for (int i = 0; i < brokenFallingObject.transform.childCount; i++)
        {
            brokenPlatformsArray[i] = brokenFallingObject.transform.GetChild(i).gameObject;
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

        int br = Random.Range(brokenRamdon,100);

        if (active) 
        {
            if (br <= 60)
            {
                int r = Random.Range(0, fallingObject.transform.childCount);

                while (platformsArray[r].activeSelf == true)
                {
                    r = Random.Range(0, fallingObject.transform.childCount);
                }

                platformsArray[r].SetActive(true);
                platformsArray[r].transform.parent = GameObject.Find("###���x�ͦ���m###").transform;
                platformsArray[r].GetComponent<fallingPart>().active = true;

                brokenRamdon = brokenRamdon + 5;
            }
            else 
            {
                int r = Random.Range(0, brokenFallingObject.transform.childCount);

                GameObject brokenObject = Instantiate(brokenPlatformsArray[r].gameObject);

                brokenObject.transform.parent = null;
                brokenObject.transform.position = this.transform.position;
                brokenObject.GetComponent<fallingPart>().active = true;

                brokenRamdon = 0;
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
