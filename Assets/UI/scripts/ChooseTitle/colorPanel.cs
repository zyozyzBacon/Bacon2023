using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorPanel : MonoBehaviour
{
    public bool select;
    public Sprite[] phase;

    public void phaseChage() 
    {

        if (!select) 
        {
            int c = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).childCount != 0)
                    c++;


                if (c != 0)
                    this.GetComponent<Image>().sprite = phase[1];
                else
                    this.GetComponent<Image>().sprite = phase[0];
            }

        }else
            this.GetComponent<Image>().sprite = phase[2];

    }
}
