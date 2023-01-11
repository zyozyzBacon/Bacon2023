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
        if (collision.tag == "Item" && !this.gameObject.GetComponent<PlayerStateList>().dead)
        {
            IitemInterface itemInterface = collision.gameObject.GetComponent<IitemInterface>();

            if (itemInterface != null)
                itemInterface.ItemTrigger(this.gameObject);
            else
                Debug.LogError("出錯 道具不正常");

        }

        if (collision.tag == "DashAttack" && !this.gameObject.GetComponent<PlayerStateList>().dead)
        {
            if (collision.transform.parent.GetComponent<PlayerStateList>().dashing)
            {
                if (this.gameObject.GetComponent<BasicPlayerControll>() != null) 
                {
                   if (!this.gameObject.GetComponent<PlayerStateList>().recoverying)
                        this.gameObject.GetComponent<BasicPlayerControll>().damaged(collision.transform.parent.position);

                }          
                else
                    Debug.LogError("出錯 玩家不正常");

            }
        }

        if (collision.tag == "Bubble")
        {
            if (this.gameObject.GetComponent<BasicPlayerControll>() != null) 
            {
                if (collision.transform.parent.transform.tag == "ItemLocation")
                {
                    foodBattleManager.instance.bubbleDetect();
                }

                if (this.gameObject.GetComponent<foodBattlePlayer>() != null && foodBattleManager.instance != null)
                {
                    if (!this.gameObject.GetComponent<PlayerStateList>().dead)
                    {
                        this.gameObject.GetComponent<foodBattlePlayer>().eating(collision.gameObject);
                        Destroy(collision.gameObject);
                    }
                    else
                    {
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
}
