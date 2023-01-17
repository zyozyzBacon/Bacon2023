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
                Debug.LogError("�X�� �D�㤣���`");

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
                    Debug.LogError("�X�� ���a�����`");

            }
        }

        if (collision.tag == "Bubble")
        {
            if (playerControll != null) 
            {
                if (collision.transform.parent.transform.tag == "ItemLocation")
                {
                    //�q��l�X�o�I�Y�쪺 (�򪱮a�Y�����L�� ���a�@���������N�|Ĳ�o)
                    foodBattleManager.instance.bubbleDetect();
                }

                if (!playerStateList.dead)
                {
                    //�Y�쭹�������a������ (�e���O�٬��۪����a)
                    this.gameObject.GetComponent<foodBattlePlayer>().eating(collision.gameObject);
                    Destroy(collision.gameObject);
                }
                else
                {
                    //���`���a�Y��ɪ�����
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
