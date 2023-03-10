using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPlayerTrigger : MonoBehaviour
{

    Rigidbody2D rb;

    PlayerStateList playerStateList;
    BasicPlayerControll playerControll;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStateList = this.gameObject.GetComponent<PlayerStateList>();
        playerControll = this.gameObject.GetComponent<BasicPlayerControll>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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

        if (collision.tag == "DangerItem" && !playerStateList.dead)
        {
            if (playerControll != null)
            {
                if (!playerStateList.recoverying)
                    playerControll.damaged(collision.transform.parent.gameObject);

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
                }
                else
                {
                    //死亡玩家吃到時的反應
                    if (collision.transform.parent != this.gameObject.transform && playerControll.deadBubble == null)
                    {
                        if (!collision.transform.parent.CompareTag("GhostPlayer")) 
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
                        }

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
                        playerControll.damaged(collision.transform.gameObject);
                }
                else
                    Debug.LogError("出錯 玩家不正常");
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        BasicPlayerControll playerControll = this.gameObject.GetComponent<BasicPlayerControll>();

        if (collision.tag == "DashAttack" && !playerStateList.dead)
        {
            if (collision.transform.parent.GetComponent<PlayerStateList>().dashing)
            {
                if (playerControll != null)
                {
                    if (!playerStateList.recoverying)
                        playerControll.damaged(collision.transform.parent.gameObject);

                }
                else
                    Debug.LogError("出錯 玩家不正常");

            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {  
        if (collision.gameObject.CompareTag("Platform")) 
        {
            playerStateList.platform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            playerStateList.platform = null;
        }
    }
}
