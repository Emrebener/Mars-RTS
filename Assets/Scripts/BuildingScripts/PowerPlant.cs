using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building
{
    private void Start()
    {
        this.buildingType = BuildingType.PowerPlant;
        this.buildingDescription = "This structure supplies power to the base.";
    }
}