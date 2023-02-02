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
            //從原始出發點吃到的 (跟玩家吃食物無關 玩家一拿走食物就會觸發)
            if(foodBattleManager.instance != null)
                foodBattleManager.instance.bubbleDetect();
        }

        if (collision.tag == "DeadZone") 
        {
            Destroy(this.gameObject);
        }
    }
}
