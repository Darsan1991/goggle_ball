using UnityEngine;

public class AdvMath {

    public static void SolveQuadratic(float a, float b, float c,out float solution1,out float solution2)
    {
        var sqrPart = b * b - 4 * a * c;

        if(sqrPart<0)
            throw new NoSolutionException();

        solution1 = (-b + Mathf.Sqrt(sqrPart)) / (2 * a);
        solution2 = (-b - Mathf.Sqrt(sqrPart)) / (2 * a);
    }

    public static float EaseIn(float t) => t * t;

    public static float EaseOut(float t) => 1 - (1-t) * (1-t);

    public static float InverseEaseIn(float normalizedValue) => Mathf.Sqrt(Mathf.Clamp01(normalizedValue));

    public static float InverseEaseOut(float normalizedValue) => 1 - Mathf.Sqrt(1 - Mathf.Clamp01(normalizedValue));
}