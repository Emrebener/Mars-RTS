using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum UnitType
    {
        Robin,
        Lynda,
        Tank,
        GarbageTruck
    }

    public UnitType unitType;
    public string unitDescription;

    public UnitType GetUnitType()
    {
        return this.unitType;
    }

    public string GetUnitDescription()
    {
        return this.unitDescription;
    }
}
