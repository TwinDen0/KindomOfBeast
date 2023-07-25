using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerEnemies : MonoBehaviour
{
    public List<Enemy> enemies;

    public static ManagerEnemies Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            enemies.Add(transform.GetChild(i).GetComponent<Enemy>());
        }
    }

    public void DieEnemy(Enemy enemy)
    {
        Main.Instance.rewards += 1;
        enemies.Remove(enemy);
        if (enemies.Count < 1)
        {
            Main.Instance.Win();
        }
    }
    public void StartBattle()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.StartBattle();
        }
    }
}
