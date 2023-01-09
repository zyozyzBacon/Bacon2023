using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p01 : MonoBehaviour, punishmentInterface
{
    private GameObject bullet;
    private Transform[] transforms;

    void punishmentInterface.active(GameObject[] targetPlayer) 
    {
        transforms = new Transform[targetPlayer.Length];

        for (int i = 0; i < targetPlayer.Length;i++) 
        {
            transforms[i] = targetPlayer[i].transform;

            transforms[i].position = new Vector3(
                transforms[i].position.x,
                transforms[i].position.y + 5,
                transforms[i].position.z);

            Instantiate(bullet, transforms[i].position, transforms[i].rotation);
        }
    }

    void Awake() 
    {
        bullet = Resources.Load<GameObject>("punishmentPrefabs/REDFUCK");
    }

}

