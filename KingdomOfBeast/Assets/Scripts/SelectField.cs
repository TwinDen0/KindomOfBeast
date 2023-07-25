using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SelectField : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Cell cellSelected;
    public Building buildingSelected;
    Image image;
    private Vector2 mousePosition;
    public float offsetX, offsetY;

    public static SelectField Instance;
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

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void Visible()
    {
        image.enabled = true;
    }
    private void Update()
    {
        if (Main.Instance.battle)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            if (buildingSelected)
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                buildingSelected.transform.position = new Vector3(mousePosition.x - offsetX, mousePosition.y - offsetY, cellSelected.positionOnTrueSpace.z - 0.0001f);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            image.enabled = false;
            if (buildingSelected)
            {
                Building buildingScript = buildingSelected.GetComponent<Building>();
                if (cellSelected.building)
                {
                    if (buildingSelected.type == cellSelected.building.type && AddBuilds.Instance.CheckMaxLevel(buildingSelected.type))
                    {
                        //merge
                        Building newBuilding = Instantiate(AddBuilds.Instance.GetMerge(buildingSelected.type, buildingSelected, cellSelected.building), cellSelected.positionOnTrueSpace, Quaternion.identity).GetComponent<Building>();
                        ManagerBuilds.Instance.buildings.Add(newBuilding);
                        newBuilding.lastCell = cellSelected;
                        cellSelected.building = newBuilding;
                        PlayerPrefs.SetInt("Cell" + cellSelected.numCell, (int)newBuilding.type);
                    }
                    else
                    {
                        buildingScript.lastCell.SetBuilding(buildingSelected);
                        buildingScript.ToLastCell();
                    }
                    buildingSelected = null;
                    return;
                }
                cellSelected.SetBuilding(buildingSelected);
                buildingSelected.transform.position = cellSelected.positionOnTrueSpace;
                buildingScript.lastCell = cellSelected;
                buildingSelected = null;
            }
        }
    }
    public void TranslateOnObject(GameObject obj)
    {
        if (Main.Instance.battle)
        {
            return;
        }
        cellSelected = obj.GetComponent<Cell>();
        transform.position = obj.transform.position;
        if (cellSelected.building && buildingSelected != null)
        {
            if(buildingSelected.type == cellSelected.building.type && AddBuilds.Instance.CheckMaxLevel(buildingSelected.type))
            {
                image.color = new Color(0.5f, 1, 1, 0.7f);
            }
            else
            {
                image.color = new Color(1, 0.5f, 0.5f, 0.7f);
            }
        }
        else
        {
            image.color = new Color(1, 1, 1, 0.7f);
        }
    }
}
