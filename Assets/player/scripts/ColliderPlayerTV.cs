using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayerTV : MonoBehaviour
{

    [Header("[�Ű�]����������")]
    [SerializeField] private PlayerStateList pState;

    //Rigidbody2D rb;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AnswerBox" && pState.tvModeOn)
        {
            pState.currentAnswer = collision.gameObject.GetComponent<tvAnswerBox>().Answer;
        }
    }


}
