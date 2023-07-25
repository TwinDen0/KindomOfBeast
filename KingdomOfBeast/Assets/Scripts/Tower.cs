using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Tower : MonoBehaviour
{
    [SerializeField] float speed;
    public GameObject prefabArrow;
    [SerializeField] Transform startShot;
    [SerializeField] float shotDistance;
    [SerializeField] Animator animator;

    bool start = false;

    int angleMove = 20;

    Enemy target;
    private void Start()
    {
        if (Main.Instance.king == Main.King.Frog)
        {
            speed = speed * Main.Instance.kingPower;
        }
        animator.speed = speed;
    }
    private void FixedUpdate()
    {
        if (!start)
        {
            return;
        }
        if (ManagerEnemies.Instance.enemies.Count == 0)
        {
            return;
        }
        Enemy target = ManagerEnemies.Instance.enemies[0];
        foreach (var enemy in ManagerEnemies.Instance.enemies)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > Vector3.Distance(enemy.transform.position, transform.position))
            {
                target = enemy;
            }
        }
        this.target = target;
        if (Vector3.Distance(target.transform.position, transform.position) <= shotDistance)
        {
            animator.Play("Hit" + GetAngle(Vector3.up, target.transform.position, transform.position));
        }
    }
    public void StartBattle()
    {
        start = true;
    }
    public void Hit()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("ArcherySound");
        }
        
        if (target)
        {
            AddArrow();
        }
        else
        {
            animator.Play("Idle");
        }
    }
    void AddArrow()
    {
        Arrow arrow = Instantiate(prefabArrow, startShot.position, Quaternion.Euler(0, 0, 0)).GetComponent<Arrow>();
        arrow.enemy = target;
    }
    int GetAngle(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        float angle = Vector2.Angle(start, pos1 - pos2);
        if (pos1.x - pos2.x < 0)
        {
            angle = 360 - angle;
        }
        int res = angleMove;
        if ((angle > 0f && angle <= 20 + 22.5f) || angle > 335 + 22.5f)
        {
            res = 20;
        }
        if (angle > 65 - 22.5f && angle <= 65 + 22.5f)
        {
            res = 65;
        }
        if (angle > 110 - 22.5f && angle <= 110 + 22.5f)
        {
            res = 110;
        }
        if (angle > 155 - 22.5f && angle <= 155 + 22.5f)
        {
            res = 155;
        }
        if (angle > 200 - 22.5f && angle <= 200 + 22.5f)
        {
            res = 200;
        }
        if (angle > 245 - 22.5f && angle <= 245 + 22.5f)
        {
            res = 245;
        }
        if (angle > 290 - 22.5f && angle <= 290 + 22.5f)
        {
            res = 290;
        }
        if (angle > 335 - 22.5f && angle <= 335 + 22.5f)
        {
            res = 335;
        }
        angleMove = res;
        return res;
    }
}
