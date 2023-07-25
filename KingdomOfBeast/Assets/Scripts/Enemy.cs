using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    [SerializeField] int hp;
    public int strong;
    [SerializeField] float distanceForHit;
    [SerializeField] Building targetBuilding;
    [SerializeField] Warrior targetWarrior;
    bool firstHit = false;
    bool battle = false;
    bool defence = false;
    [SerializeField] int moneyForOneHit;

    Vector3 lastStep;
    int angleMove = 20;

    [SerializeField] SpriteRenderer shape;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        transform.rotation = Quaternion.Euler(0,0,0);
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
                if (defence)
                {
                    if (targetWarrior)
                    {
                        angle = GetAngle(Vector3.up, targetWarrior.transform.position, transform.position);
                    }
                    else
                    {
                        SearchTarget();
                    }
                }
                else
                {
                    if (targetBuilding)
                    {
                        angle = GetAngle(Vector3.up, targetBuilding.transform.position, transform.position);
                    }
                    else
                    {
                        SearchTarget();
                    }
                }
                if (startAngle != angle)
                {
                    animator.Play("Hit" + angle);
                }
                break;
        }
        if (!battle)
        {
            return;
        }
        if (!targetBuilding && battle)
        {
            SearchTarget();
            return;
        }
        if (Vector3.Distance(transform.position, targetBuilding.transform.position) <= distanceForHit && firstHit && !defence)
        {
            animator.Play("Hit" + GetAngle(Vector3.up, targetBuilding.transform.position, transform.position));
            return;
        }
    }
    public void Hit()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("EnmyHitSound");
        }
        
        if (defence)
        {
            GetHitWarrior();
            return;
        }
        GetHitBuilding();
    }
    public void GetHitBuilding()
    {
        firstHit = false;
        if (!targetBuilding)
        {
            SearchTarget();
            return;
        }
        if (targetBuilding.TakeHit(strong))
        {
            SearchTarget();
        }
    }
    public void GetHitWarrior()
    {
        firstHit = false;
        if (!targetWarrior)
        {
            defence = false;
            SearchTarget();
            return;
        }
        if (targetWarrior.TakeHit(strong))
        {
            defence = false;
            SearchTarget();
        }
    }
    public bool TakeHit(int strong, Warrior warrior = null)
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("EnmyDamageSound");
        }
        StartCoroutine(ColorBuildHit());
        //money add
        Main.Instance.AddMoney(moneyForOneHit * strong);
        Main.Instance.rewards += moneyForOneHit * strong;
        if (Main.Instance.king == Main.King.Lion)
        {
            strong = (int)(strong * Main.Instance.kingPower);
        }

        if (warrior && warrior != targetWarrior && !defence)
        {
            targetWarrior = warrior;
            StartDefence();
        }
        hp -= strong;
        if(hp <= 0)
        {
            Die();
            return true;
        }
        return false;
    }
    IEnumerator ColorBuildHit()
    {
        shape.color = Main.Instance.hitColor;
        yield return new WaitForSecondsRealtime(0.1f);
        shape.color = Color.white;
    }
    void StartDefence()
    {
        animator.Play("Hit" + GetAngle(Vector3.up, targetWarrior.transform.position, transform.position));
        defence = true;
        agent.SetDestination(targetWarrior.transform.position);
    }
    void Die()
    {
        ManagerEnemies.Instance.DieEnemy(this);
        Destroy(gameObject);
    }
    public void SetTarget(Vector3 target)
    {
        agent.SetDestination(target);
    }
    public void StartBattle()
    {
        battle = true;
        SearchTarget();
    }
    public void SearchTarget()
    {
        animator.Play("Run" + GetAngle(Vector2.up, transform.position, lastStep));
        firstHit = true;
        if(ManagerBuilds.Instance.buildings.Count < 1)
        {
            battle = false;
            animator.Play("Idle");
            return;
        }
        targetBuilding = ManagerBuilds.Instance.buildings[0];
        Vector3 target = targetBuilding.transform.position;
        defence = false;
        foreach (var building in ManagerBuilds.Instance.buildings)
        {
            if(Vector3.Distance(target, transform.position) > Vector3.Distance(building.transform.position, transform.position))
            {
                target = building.transform.position;
                targetBuilding = building;
            }
        }
        SetTarget(target);
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
        lastStep = transform.position;
        angleMove = res;
        return res;
    }
}
