using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Warrior : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] int hp;
    public int strong;
    [SerializeField] float distanceForHit;
    public Enemy targetEnemy;
    bool firstHit = false;
    bool end = false;

    Vector3 lastStep;
    int angleMove = 20;

    [SerializeField] SpriteRenderer shape;

    void Start()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("RoosterSpawnSound");
        }
        
        lastStep = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        SearchTarget();
    }
    void FixedUpdate()
    {
        switch (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(0, 3))
        {
            case "Run":
                int startAngle = angleMove;
                int angle = GetAngle(Vector2.up, transform.position, lastStep);
                if (startAngle != angle)
                {
                    animator.Play("Run" + angle);
                }
                break;
            case "Hit":
                startAngle = angleMove;
                angle = 0;
                if (targetEnemy)
                {
                    angle = GetAngle(Vector3.up, targetEnemy.transform.position, transform.position);
                }
                else
                {
                    SearchTarget();
                }
                if (startAngle != angle)
                {
                    animator.Play("Hit" + angle);
                }
                break;
        }
        if (end)
        {
            agent.speed = 0;
            return;
        }
        if (!targetEnemy || Vector3.Distance(transform.position, targetEnemy.transform.position) > distanceForHit)
        {
            SearchTarget();
            return;
        }
        if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= distanceForHit && firstHit)
        {
            animator.Play("Hit" + GetAngle(Vector3.up, targetEnemy.transform.position, transform.position));
            return;
        }
    }
    public void Hit()
    {
        GetHit();
    }
    public void GetHit()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("RoosterHitSound");
        }
        firstHit = false;
        if (!targetEnemy)
        {
            SearchTarget();
            return;
        }
        if (targetEnemy.TakeHit(strong, this))
        {
            SearchTarget();
        }
    }
    public bool TakeHit(int strong)
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("RoosterDamageSound");
        }
        StartCoroutine(ColorBuildHit());
        hp -= strong;
        if (hp <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    IEnumerator ColorBuildHit()
    {
        shape.color = shape.color = Main.Instance.hitColor;
        yield return new WaitForSecondsRealtime(0.1f);
        shape.color = Color.white;
    }
    void Die()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("RoosterDestroySound");
        }

        Destroy(gameObject);
    }
    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }
    public void SearchTarget()
    {
        animator.Play("Run" + GetAngle(Vector2.up, transform.position, lastStep));
        firstHit = true;
        if (ManagerEnemies.Instance.enemies.Count < 1)
        {
            animator.Play("Idle");
            end = true;
            return;
        }
        targetEnemy = ManagerEnemies.Instance.enemies[0];
        Vector3 target = targetEnemy.transform.position;
        foreach (var enemy in ManagerEnemies.Instance.enemies)
        {
            if (Vector3.Distance(target, transform.position) > Vector3.Distance(enemy.transform.position, transform.position))
            {
                target = enemy.transform.position;
                targetEnemy = enemy;
            }
        }
        SetTarget(target);
    }
    int GetAngle(Vector3 start, Vector3 pos1, Vector3 pos2)
    {
        float angle = Vector2.Angle(start, pos1 - pos2);
        if(pos1.x - pos2.x < 0)
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
        lastStep = transform.position;
        angleMove = res;
        return res;
    }
}
