using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public Type type;
    public Cell lastCell;
    bool goStart = false;
    [SerializeField] int hp;
    [SerializeField] Image hpline;
    [SerializeField] GameObject hplineBack;
    [SerializeField] float upForHpLine = 0;
    float shotLenght = 0;
    [SerializeField] SpriteRenderer shape;

    public enum Type
    {
        Tower1,
        Tower2,
        Tower3,
        Tower4,
        Tower5,
        Casern1,
        Casern2,
        Casern3,
        Casern4,
        Casern5,
        Castle
    }
    private void Start()
    {
        shotLenght = 1.0f / hp;
        if(Main.Instance.king == Main.King.Pig)
        {
            hp = (int)(hp * Main.Instance.kingPower);
        }
    }
    private void Update()
    {
        Vector3 positionHp = Main.Instance.cam.WorldToScreenPoint(transform.position + Vector3.up * upForHpLine);
        if(hpline.gameObject.transform.position != positionHp)
        {
            hpline.gameObject.transform.position = positionHp;
            hplineBack.transform.position = positionHp;
        }
    }
    private void FixedUpdate()
    {
        if (goStart)
        {
            if((transform.position - lastCell.positionOnTrueSpace).magnitude > 1f)
            {
                transform.position += (transform.position - lastCell.positionOnTrueSpace).normalized * -0.3f;
            }
            else
            {
                transform.position = lastCell.positionOnTrueSpace;
                goStart = false;
            }
        }
    }
    public bool TakeHit(int strong)
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("BildingDamageSound");
        }
        hp -= strong;
        StartCoroutine(ColorBuildHit());
        if(hpline.fillAmount == 1)
        {
            hpline.enabled = true;
            hplineBack.SetActive(true);
        }
        hpline.fillAmount = hp * shotLenght;
        if (hp <= 0)
        {
            Destroy();
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
    void Destroy()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("BildingDestroySound");
        }
        ManagerBuilds.Instance.DestroyBuild(this);
        if(type != Type.Castle)
        {
            Destroy(gameObject, 0.1f);
        }
        else
        {
            Main.Instance.Lose();
        }
    }
    public void ToLastCell()
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("ErrorSound");
        }
        goStart = true;
    }
}
