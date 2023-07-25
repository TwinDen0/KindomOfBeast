using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddBuilds : MonoBehaviour
{
    public List<GameObject> buttons;
    public List<Cell> cells = new List<Cell>();
    public int countBuildings = 0;
    public List<GameObject> buildingsPrefabs;
    public Dictionary<Building.Type, int> buildings = new Dictionary<Building.Type, int>()
    {
        { Building.Type.Tower1, 0 },
        { Building.Type.Tower2, 1 },
        { Building.Type.Tower3, 2 },
        { Building.Type.Tower4, 3 },
        { Building.Type.Tower5, 4 },
        { Building.Type.Casern1, 5 },
        { Building.Type.Casern2, 6 },
        { Building.Type.Casern3, 7 },
        { Building.Type.Casern4, 8 },
        { Building.Type.Casern5, 9 },
    };

    public static AddBuilds Instance;
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

    public void AddBuilding(Building.Type buildingType, int cell = -1)
    {
        GameObject building = buildingsPrefabs[buildings.GetValueOrDefault(buildingType)];
        countBuildings += 1;

        int index = Random.Range(0, cells.Count);
        if (cell != -1)
        {
            index = cell;
        }
        else
        {
            while (cells[index].building)
            {
                index += 1;
                if (index == cells.Count)
                {
                    index = 0;
                }
            }
        }
        Building build = Instantiate(building, cells[index].positionOnTrueSpace, Quaternion.identity).GetComponent<Building>();
        ManagerBuilds.Instance.buildings.Add(build);
        build.lastCell = cells[index];
        cells[index].building = build;
        PlayerPrefs.SetInt("Cell" + index, (int)build.type);
        Debug.Log((int)build.type);
        if (countBuildings == cells.Count)
        {
            foreach (var button in buttons)
            {
                button.SetActive(false);
            }
        }
    }

    public GameObject GetMerge(Building.Type startType, Building build1, Building build2)
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("CombiningSound");
        }
        ManagerBuilds.Instance.buildings.Remove(build1);
        ManagerBuilds.Instance.buildings.Remove(build2);
        Destroy(build1.gameObject);
        Destroy(build2.gameObject);
        countBuildings -= 1;
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
        int index = buildings.GetValueOrDefault(startType);
        if (Main.Instance.openedTypes[(index + 1) / 5] < (index + 1) % 5 + 1)
        {
            Main.Instance.openedTypes[(index + 1) / 5] = (index + 1) % 5 + 1;
            Main.Instance.SetCollection();
        }
        return buildingsPrefabs[index + 1];
    }
    public bool CheckMaxLevel(Building.Type type)
    {
        int index = buildings.GetValueOrDefault(type);
        if ((index + 1) % 5 == 0)
        {
            return false;
        }
        return true;
    }
}
