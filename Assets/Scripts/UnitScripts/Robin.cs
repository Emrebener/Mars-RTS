using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Robin : UnitBase
{
    AIPath refAIPath;
    Vector3 directionVector;
    SpriteRenderer spriteRenderer;

    [SerializeField] Sprite RobinUp;
    [SerializeField] Sprite RobinDown;
    [SerializeField] Sprite RobinLeft;
    [SerializeField] Sprite RobinRight;

    void Start()
    {
        refAIPath = GetComponent<AIPath>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateSprite", .1f, .5f);

        this.unitType = UnitType.Robin;
        this.unitDescription = "He is just Robin.";
    }

    void UpdateSprite()
    {
        directionVector = refAIPath.desiredVelocity;

        if (directionVector.magnitude == 0)
        {
            
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) < 45) // GOING LEFT
        {
            spriteRenderer.sprite = RobinLeft;
        }
        else if (Vector3.Angle(new Vector3(-90, 0, 0), directionVector) > 135) // GOING RIGHT
        {
            spriteRenderer.sprite = RobinRight;
        }
        else if (directionVector.y > 0) // GOING UP
        {
            spriteRenderer.sprite = RobinUp;
        }
        else // GOING DOWN
        {
            spriteRenderer.sprite = RobinDown;
        }
    }

    public Sprite GetPortraitSprite()
    {
        return RobinDown;
    }
}
