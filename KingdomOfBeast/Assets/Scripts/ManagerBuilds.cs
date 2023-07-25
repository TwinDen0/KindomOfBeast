using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBuilds : MonoBehaviour
{
    public List<Building> buildings;

    public static ManagerBuilds Instance;
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

    public void DestroyBuild(Building building)
    {
        buildings.Remove(building);
        if(buildings.Count < 1)
        {
            Main.Instance.Lose();
        }
    }
    public void StartBattle()
    {
        foreach (Building building in buildings)
        {
            if (building.GetComponent<Tower>())
            {
                building.GetComponent<Tower>().StartBattle();
            }
            if (building.GetComponent<Casern>())
            {
                building.GetComponent<Casern>().StartBattle();
            }
        }
    }
}
