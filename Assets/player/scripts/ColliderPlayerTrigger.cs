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

        if (collision.tag == "Bubble")
        {
            if (playerControll != null) 
            {
                if (collision.transform.parent.transform.tag == "ItemLocation")
                {
                    //從原始出發點吃到的 (跟玩家吃食物無關 玩家一拿走食物就會觸發)
                    foodBattleManager.instance.bubbleDetect();
                }

                if (!playerStateList.dead)
                {
                    //吃到食物的玩家的反應 (前提是還活著的玩家)
                    this.gameObject.GetComponent<foodBattlePlayer>().eating(collision.gameObject);
                    Destroy(collision.gameObject);
                }
                else
                {
                    //死亡玩家吃到時的反應
                    if (collision.transform.parent != this.gameObject.transform && !this.gameObject.GetComponent<foodBattlePlayer>().bubble)
                    {
                        collision.transform.position = this.gameObject.transform.position;
                        collision.transform.parent = this.gameObject.transform;
                        this.gameObject.GetComponent<foodBattlePlayer>().bubble = collision.gameObject;
                    }
                }
            }
        }
    }
}
