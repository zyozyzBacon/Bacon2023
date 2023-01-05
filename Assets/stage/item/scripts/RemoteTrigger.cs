using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteTrigger : MonoBehaviour, IitemInterface
{
    void IitemInterface.ItemTrigger(GameObject player)
    { 
        tvPartGameplay.tvGameplayManager.readyTrans();
        tvPartGameplay.tvGameplayManager.bossReady(player);
        Destroy(this.gameObject);
    }
}
