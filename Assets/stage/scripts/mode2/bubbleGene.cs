using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleGene : MonoBehaviour
{

    public float moveSpeed;
    public float randomMoveTimer;
    public float ptTimer;

    private bool active;

    public GameObject AllfallingObject;
    private GameObject[] fallingObject;
   
    public GameObject[] sidePoint = new GameObject[2];
    int i;

    Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        fallingObject = new GameObject[AllfallingObject.transform.childCount];
        for (int f = 0; f < AllfallingObject.transform.childCount; f++)
            fallingObject[f] = AllfallingObject.transform.GetChild(f).gameObject;

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
        transform.position = new Vector3(
            Random.Range(sidePoint[0].transform.position.x, sidePoint[1].transform.position.x),
            transform.position.y,
            transform.position.z);
    }

    IEnumerator platform(float sceonds)
    {
        yield return new WaitForSeconds(sceonds);

        int r = Random.Range(0,AllfallingObject.transform.childCount);

        if (active)
        {
            GameObject item = Instantiate(fallingObject[r],this.gameObject.transform);
            item.transform.parent = GameObject.Find("###¬Ã¯]¥Í¦¨¦ì¸m###").transform;
            item.GetComponent<fallingPart>().active = true;
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
