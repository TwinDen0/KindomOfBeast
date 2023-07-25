using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForAnimEvents : MonoBehaviour
{
    public void Hit()
    {
        if (!transform.parent)
        {
            return;
        }
        Enemy enemy;
        if (enemy = transform.parent.GetComponent<Enemy>()){
            enemy.Hit();
        }
        Warrior warrior;
        if (warrior = transform.parent.GetComponent<Warrior>())
        {
            warrior.Hit();
        }
        if (!transform.parent.parent)
        {
            return;
        }
        Tower tower;
        if (tower = transform.parent.parent.GetComponent<Tower>())
        {
            tower.Hit();
        }
    }
}
