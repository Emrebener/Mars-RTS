using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Barracks : Building
{
    [SerializeField] GameObject unit1;
    [SerializeField] GameObject unit2;

    private void Start()
    {
        this.buildingType = BuildingType.Barracks;
        this.buildingDescription = "Produces human units.";
    }

    public void ProduceUnit(int i)
    {
        GameObject newUnit;
        switch (i)
        {
            case 1:
                newUnit = Instantiate(unit1, new Vector3(transform.position.x - 1, transform.position.y - 1), Quaternion.identity);
                newUnit.GetComponent<AIPath>().destination = new Vector2(transform.position.x - 1, transform.position.y - 2);
                newUnit.GetComponent<AIPath>().SearchPath();
                break;
            case 2:
                newUnit = Instantiate(unit2, new Vector3(transform.position.x - 1, transform.position.y - 1), Quaternion.identity);
                newUnit.GetComponent<AIPath>().destination = new Vector2(transform.position.x - 1, transform.position.y - 2);
                newUnit.GetComponent<AIPath>().SearchPath();
                break;
        }
    }
}
