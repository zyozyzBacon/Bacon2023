using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleItem : MonoBehaviour, IitemInterface
{

    public Sprite Icon;
    public float seconds;

    void IitemInterface.ItemTrigger(GameObject player)
    {
        //this.transform.position = Vector3.zero;
        StartCoroutine(timer(seconds));
    }

    IEnumerator timer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
