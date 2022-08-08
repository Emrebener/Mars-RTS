using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GarbageTruck : UnitBase
{
    AIPath refAIPath;
    Vector3 directionVector;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite GarbageTruckLeft;
    [SerializeField] Sprite GarbageTruckRight;

    void Start()
    {
        refAIPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateSprite", .1f, .5f);

        this.unitType = UnitType.GarbageTruck;
        this.unitDescription = "Mars bases need garbage trucks too.";
    }

    void UpdateSprite()
    {
        directionVector = refAIPath.desiredVelocity;

        if (directionVector.magnitude == 0)
        {
            
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) < 90) // GOING LEFT
        {
            spriteRenderer.sprite = GarbageTruckLeft;
        }
        else
        {
            spriteRenderer.sprite = GarbageTruckRight;
        }
    }

    public Sprite GetPortraitSprite()
    {
        return GarbageTruckRight;
    }
}
