using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LeakageEffect : MonoBehaviour
{
    public Light2D light2D;
    public GameObject leakage;
    float lateTime = 1;

    void Start()
    {
        //light2D =gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lateTime -= Time.deltaTime;
        if (lateTime <= 0)
        {
            light2D.intensity = 0f;
            leakage.SetActive(true);
            Invoke("LateTime", 0.2f);
        }
        if (lateTime > 0)
        {
            leakage.SetActive(false);
        }
        Debug.Log(lateTime);
    }

    void LateTime()
    {
        lateTime = Random.Range(1f, 5f);
        light2D.intensity = 0.3f;
    }
   
}
