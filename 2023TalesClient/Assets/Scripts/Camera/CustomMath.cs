using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumMath : MonoBehaviour
{
    public static bool Approximation(float a, float b, float threshold)
    {
        return (Mathf.Abs(a - b) < threshold); // compare both values and see if the difference is between threshold
    }
}
