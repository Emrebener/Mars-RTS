using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Lynda : UnitBase
{
    AIPath refAIPath;
    Vector3 directionVector;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite LyndaUp;
    [SerializeField] Sprite LyndaDown;
    [SerializeField] Sprite LyndaLeft;
    [SerializeField] Sprite LyndaRight;

    void Start()
    {
        refAIPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateSprite", .1f, .5f);
        
        this.unitType = UnitType.Lynda;
        this.unitDescription = "Lynda is a professional marathon athlete. She always outruns Robin and makes fun of him.";
    }

    void UpdateSprite()
    {
        directionVector = refAIPath.desiredVelocity;

        if (directionVector.magnitude == 0)
        {
            
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) < 45) // GOING LEFT
        {
            spriteRenderer.sprite = LyndaLeft;
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) > 135) // GOING RIGHT
        {
            spriteRenderer.sprite = LyndaRight;
        }
        else if (directionVector.y > 0) // GOING UP
        {
            spriteRenderer.sprite = LyndaUp;
        }
        else // GOING DOWN
        {
            spriteRenderer.sprite = LyndaDown;
        }
    }

    public Sprite GetPortraitSprite()
    {
        return LyndaDown;
    }
}
