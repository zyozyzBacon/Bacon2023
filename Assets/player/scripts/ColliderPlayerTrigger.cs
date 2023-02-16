using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayerTrigger : MonoBehaviour
{

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerStateList playerStateList = this.gameObject.GetComponent<PlayerStateList>();
        BasicPlayerControll playerControll = this.gameObject.GetComponent<BasicPlayerControll>();

        foodBattlePlayer foodGameplay = null;
        if (this.gameObject.GetComponent<foodBattlePlayer>() != null)
            foodGameplay = this.gameObject.GetComponent<foodBattlePlayer>();

        if (collision.tag == "Item" && !playerStateList.dead)
        {
            IitemInterface itemInterface = collision.gameObject.GetComponent<IitemInterface>();

            if (itemInterface != null)
                itemInterface.ItemTrigger(this.gameObject);
            else
                Debug.LogError("出錯 道具不正常");

        }

        if (collision.tag == "DashAttack" && !playerStateList.dead)
        {
            if (collision.transform.parent.GetComponent<PlayerStateList>().dashing)
            {
                if (playerControll != null) 
                {
                   if (!playerStateList.recoverying)
                        playerControll.damaged(collision.transform.parent.position);

                }          
                else
                    Debug.LogError("出錯 玩家不正常");

            }
        }

        if (collision.tag == "DangerItem" && !playerStateList.dead)
        {
            if (playerControll != null)
            {
                if (!playerStateList.recoverying)
                    playerControll.damaged(collision.transform.parent.position);

            }
            else
                Debug.LogError("出錯 玩家不正常");
        }



        if (collision.tag == "Bubble")
        {
            if (playerControll != null) 
            {
                if (!playerStateList.dead)
                {
                    //吃到食物的玩家的反應 (前提是還活著的玩家)
                    if (!playerStateList.eating) 
                    {
                        playerControll.eating(collision.gameObject);
                        if (foodGameplay != null)
                            foodGameplay.eating(collision.gameObject);
                        Destroy(collision.gameObject);
                    }

                    if (foodBattleManager.instance != null)
                        foodBattleManager.instance.bubbleDetect();
                }
                else
                {
                    //死亡玩家吃到時的反應
                    if (collision.transform.parent != this.gameObject.transform && playerControll.deadBubble == null)
                    {
                        collision.transform.position = playerControll.deadBubbleOnHand.transform.position;
                        collision.transform.parent = playerControll.deadBubbleOnHand.transform;
                        playerControll.deadBubble = collision.gameObject;

                        if (collision.GetComponent<Rigidbody2D>() != null)
                            Destroy(collision.GetComponent<Rigidbody2D>());

                        //第二模式相關
                        if (collision.GetComponent<fallingPart>() != null) 
                        {
                            Destroy(collision.GetComponent<fallingPart>());
                        }

                        if (foodBattleManager.instance != null)
                            foodBattleManager.instance.bubbleDetect();

                    }
                }
            }
        }


        //死亡相關
        if (collision.tag == "DeadZone" && !playerStateList.dead)
        {
            if (playerControll != null) 
            {
                playerControll.retired();
            }
        }

        if (collision.tag == "BubbleBullet" && !playerStateList.dead) 
        {
            if (collision.GetComponent<bubbleBullet>().parent != this.gameObject) 
            {
                if (playerControll != null)
                {
                    if (!playerStateList.recoverying)
                        playerControll.damaged(collision.transform.position);
                }
                else
                    Debug.LogError("出錯 玩家不正常");
            }
        }
    }
}
