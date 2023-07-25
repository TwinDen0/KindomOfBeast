using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class Cheat : MonoBehaviour
{
    [Header("Money")]
    public int MoneyAmount;
    public bool AddMoney;
    [Header("Progress")]
    public bool ResetProgress;

    void Update()
    {
        if (AddMoney)
        {
            AddMoney = false;
            Main.Instance.AddMoney(MoneyAmount);
        }

        if (ResetProgress)
        {
            ResetProgress = false;
            PlayerPrefs.DeleteAll();
        }
    }
}
