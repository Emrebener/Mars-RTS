using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headquarters : Building
{
    private void Start()
    {
        this.buildingType = BuildingType.Headquarters;
        this.buildingDescription = "Looks important but doesn't do anything.";
    }
}