using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Tank : UnitBase
{
    AIPath refAIPath;
    Vector3 directionVector;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite TankLeft;
    [SerializeField] Sprite TankRight;

    void Start()
    {
        refAIPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateSprite", .1f, .5f);

        this.unitType = UnitType.Tank;
        this.unitDescription = "This tank is made of extremely sturdy Mars metal (stainless steel, doesn't hold fingerprints)";
    }

    void UpdateSprite()
    {
        directionVector = refAIPath.desiredVelocity;

        if (directionVector.magnitude == 0)
        {
            
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) < 90) // GOING LEFT
        {
            spriteRenderer.sprite = TankLeft;
        }
        else
        {
            spriteRenderer.sprite = TankRight;
        }
    }

    public Sprite GetPortraitSprite()
    {
        return TankRight;
    }
}
