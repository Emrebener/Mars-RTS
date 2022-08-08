using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType
    {
        Barracks,
        PowerPlant,
        Headquarters,
        Hangar
    }

    public BuildingType buildingType;

    public string buildingDescription;

    public List<Vector3Int> OccupiedCells
    {
        get
        {
            List<Vector3Int> _result = new List<Vector3Int>();

            switch (this.buildingType)
            {
                case BuildingType.Barracks:
                    _result.Add(new Vector3Int() { x = -1, y = 1 });
                    _result.Add(new Vector3Int() { x = 0, y = 1 });
                    _result.Add(new Vector3Int() { x = 1, y = 1 });

                    _result.Add(new Vector3Int() { x = -1, y = 0 });
                    _result.Add(new Vector3Int() { x = 0, y = 0 });
                    _result.Add(new Vector3Int() { x = 1, y = 0 });

                    _result.Add(new Vector3Int() { x = -1, y = -1 });
                    _result.Add(new Vector3Int() { x = 0, y = -1 });
                    _result.Add(new Vector3Int() { x = 1, y = -1 });
                    break;

                case BuildingType.Hangar:
                    _result.Add(new Vector3Int() { x = -1, y = 1 });
                    _result.Add(new Vector3Int() { x = 0, y = 1 });
                    _result.Add(new Vector3Int() { x = 1, y = 1 });

                    _result.Add(new Vector3Int() { x = -1, y = 0 });
                    _result.Add(new Vector3Int() { x = 0, y = 0 });
                    _result.Add(new Vector3Int() { x = 1, y = 0 });

                    _result.Add(new Vector3Int() { x = -1, y = -1 });
                    _result.Add(new Vector3Int() { x = 0, y = -1 });
                    _result.Add(new Vector3Int() { x = 1, y = -1 });
                    break;

                case BuildingType.PowerPlant:
                    _result.Add(new Vector3Int() { x = -1, y = 0 });
                    _result.Add(new Vector3Int() { x = 0, y = 0 });
                    _result.Add(new Vector3Int() { x = 1, y = 0 });

                    _result.Add(new Vector3Int() { x = -1, y = -1 });
                    _result.Add(new Vector3Int() { x = 0, y = -1 });
                    _result.Add(new Vector3Int() { x = 1, y = -1 });
                    break;

                case BuildingType.Headquarters:
                    _result.Add(new Vector3Int() { x = -2, y = 2 });
                    _result.Add(new Vector3Int() { x = -1, y = 2 });
                    _result.Add(new Vector3Int() { x = 0, y = 2 });
                    _result.Add(new Vector3Int() { x = 1, y = 2 });
                    _result.Add(new Vector3Int() { x = 2, y = 2 });

                    _result.Add(new Vector3Int() { x = -2, y = 1 });
                    _result.Add(new Vector3Int() { x = -1, y = 1 });
                    _result.Add(new Vector3Int() { x = 0, y = 1 });
                    _result.Add(new Vector3Int() { x = 1, y = 1 });
                    _result.Add(new Vector3Int() { x = 2, y = 1 });

                    _result.Add(new Vector3Int() { x = -2, y = 0 });
                    _result.Add(new Vector3Int() { x = -1, y = 0 });
                    _result.Add(new Vector3Int() { x = 0, y = 0 });
                    _result.Add(new Vector3Int() { x = 1, y = 0 });
                    _result.Add(new Vector3Int() { x = 2, y = 0 });

                    _result.Add(new Vector3Int() { x = -2, y = -1 });
                    _result.Add(new Vector3Int() { x = -1, y = -1 });
                    _result.Add(new Vector3Int() { x = 0, y = -1 });
                    _result.Add(new Vector3Int() { x = 1, y = -1 });
                    _result.Add(new Vector3Int() { x = 2, y = -1 });

                    _result.Add(new Vector3Int() { x = -2, y = -2 });
                    _result.Add(new Vector3Int() { x = -1, y = -2 });
                    _result.Add(new Vector3Int() { x = 0, y = -2 });
                    _result.Add(new Vector3Int() { x = 1, y = -2 });
                    _result.Add(new Vector3Int() { x = 2, y = -2 });
                    break;
            }
            return _result;
        }
    }

    public BuildingType GetBuildingType()
    {
        return this.buildingType;
    }

    public string GetBuildingDescription()
    {
        return this.buildingDescription;
    }
}
