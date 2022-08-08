using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class EmreUtils
{
    #region RoundToNearestMidpoint
    public static float RoundToNearestMidpoint(float input)
    {
        input *= 2;
        input = (float)UnityEngine.Mathf.RoundToInt(input);
        input /= 2;
        return input;
    }

    public static Vector2 RoundToNearestMidpoint(Vector2 input)
    {
        input.x = RoundToNearestMidpoint(input.x);
        input.y = RoundToNearestMidpoint(input.y);
        return input;
    }

    public static Vector3 RoundToNearestMidpoint(Vector3 input)
    {
        input.x = RoundToNearestMidpoint(input.x);
        input.y = RoundToNearestMidpoint(input.y);
        input.z = RoundToNearestMidpoint(input.z);
        return input;
    }
    #endregion

    public static Vector2 RoundVector2(Vector2 input)
    {
        input.x = Mathf.Round(input.x);
        input.y = Mathf.Round(input.y);
        return input;
    }

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static Vector3 GetMouseWorldPos()
    {
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldSpace.z = 0;
        return mouseWorldSpace;
    }
}

