using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAddBuilding : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI priceText;
    public int price = 250;
    [SerializeField] int priceGrowth = 100;
    [SerializeField] Building.Type building;
    [SerializeField] Type type;
    [SerializeField] Sprite disableImg;

    enum Type
    {
        Casern,
        Tower
    }

    private void Start()
    {
        switch (type)
        {
            case Type.Casern:
                if (PlayerPrefs.HasKey("CasernPrice"))
                {
                    price = PlayerPrefs.GetInt("CasernPrice");
                }
                break;
            case Type.Tower:
                if (PlayerPrefs.HasKey("TowerPrice"))
                {
                    price = PlayerPrefs.GetInt("TowerPrice");
                }
                break;
        }
        SetPrice();
        DisableButton();
    }
    public void Buy()
    {
        if (Main.Instance.money >= price)
        {
            Main.Instance.AddMoney(-price);
            PlayerPrefs.SetInt("Money", Main.Instance.money);
            price += priceGrowth;
            switch (type)
            {
                case Type.Casern:
                    PlayerPrefs.SetInt("CasernPrice", price);
                    break;
                case Type.Tower:
                    PlayerPrefs.SetInt("TowerPrice", price);
                    break;
            }
            SetPrice();
            AddBuilds.Instance.AddBuilding(building);
            CheckOtherButtons();
        }
    }
    public void CheckOtherButtons()
    {
        if (Main.Instance.money < price)
        {
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                ButtonAddBuilding bab;
                if (bab = transform.parent.GetChild(i).GetComponent<ButtonAddBuilding>())
                {
                    bab.DisableButton();
                }
            }
        }
    }
    public void DisableButton()
    {
        if (Main.Instance.money < price)
        {
            transform.GetComponent<Image>().sprite = disableImg;
            transform.GetComponent<Button>().enabled = false;
        }
    }
    void SetPrice()
    {
        priceText.text = price.ToString();
    }
}
