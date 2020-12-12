using UnityEngine;
using System.Collections;

public class HelpFunctions
{
    public static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.5;
    }
}
