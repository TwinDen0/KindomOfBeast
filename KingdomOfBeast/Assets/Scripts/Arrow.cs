using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : MonoBehaviour
{
    [SerializeField] int strong;
    public Enemy enemy;
    Vector3 target;
    [SerializeField] float speed;
    [SerializeField] int offsetAngle = -45;

    void FixedUpdate()
    {
        if (enemy)
        {
            target = enemy.transform.position;
        }
        Vector3 targ = target;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offsetAngle));

        transform.position += (target - transform.position).normalized * speed;
        if (Vector3.Distance(target, transform.position) < 0.1f)
        {
            if (enemy)
            {
                enemy.TakeHit(strong);
            }
            Destroy(gameObject);
        }
    }
}
