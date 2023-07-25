using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Building building = null;
    public Vector3 positionOnTrueSpace;
    public int numCell;

    private void Start()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(transform.position));
        if (Physics.Raycast(ray, out hit))
        {
            positionOnTrueSpace = new Vector3(transform.position.x, transform.position.y, hit.point.z - 0.2f);
        }

        if (PlayerPrefs.HasKey("Cell" + numCell))
        {
            int numBuild = PlayerPrefs.GetInt("Cell" + numCell);
            AddBuilds.Instance.AddBuilding((Building.Type)numBuild, numCell);
        }
    }
    public void SetBuilding(Building building)
    {
        if (Main.Instance.sounds)
        {
            AudioManager.instance.Play("MoveBildSound");
        }
        this.building = building;
        PlayerPrefs.SetInt("Cell" + numCell, (int)building.type);
    }
    public void SelectCell()
    {
        if (Main.Instance.battle)
        {
            return;
        }
        if (building)
        {
            SelectField.Instance.Visible();
            SelectField.Instance.buildingSelected = building;
            SelectField.Instance.offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - building.transform.position.x;
            SelectField.Instance.offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - building.transform.position.y;
            building = null;
            PlayerPrefs.DeleteKey("Cell" + numCell);
        }
    }
}
