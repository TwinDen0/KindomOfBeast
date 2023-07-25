using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Casern : MonoBehaviour
{
    float timeLeftBeforeShot = 0;
    [SerializeField] float speed;
    public GameObject prefabWarrior;
    [SerializeField] Transform spawnPosition;
    [SerializeField] int numberWarriors = 10;
    private void Start()
    {
        if (Main.Instance.king == Main.King.Frog)
        {
            speed = (int)(speed * Main.Instance.kingPower);
        }
    }
    void FixedUpdate()
    {
        if (timeLeftBeforeShot > 0)
        {
            timeLeftBeforeShot -= 0.02f;
            if (timeLeftBeforeShot <= 0)
            {
                Spawn();
            }
        }
    }
    public void StartBattle()
    {
        timeLeftBeforeShot = Random.Range(0, speed / 4);
    }
    void Spawn()
    {
        if(numberWarriors > 0)
        {
            timeLeftBeforeShot = speed;
            Instantiate(prefabWarrior, spawnPosition.position, Quaternion.Euler(0, 0, 0));
            numberWarriors -= 1;
        }
        else
        {
            timeLeftBeforeShot = -1;
        }
    }
}
