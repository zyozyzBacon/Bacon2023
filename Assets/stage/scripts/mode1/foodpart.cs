using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodpart : MonoBehaviour
{
    public int food = 30;
    public ItemManager.foodColor FoodColor;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //�q��l�X�o�I�Y�쪺 (�򪱮a�Y�����L�� ���a�@���������N�|Ĳ�o)
            if(foodBattleManager.instance != null)
                foodBattleManager.instance.bubbleDetect();
        }

        if (collision.tag == "DeadZone") 
        {
            Destroy(this.gameObject);
        }
    }
}
