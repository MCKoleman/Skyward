using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtils : MonoBehaviour
{
    /// <summary>
    /// Converts a Vector3 to a Vector2, removing the z value
    /// </summary>
    /// <param name="vector3">Vector3 to convert</param>
    /// <returns>Resultant Vector2 (x and y of original)</returns>
    public static Vector2 Vector3To2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.y);
    }

    /// <summary>
    /// Converts a Vector2 to a Vector3, adding 0 for the z value
    /// </summary>
    /// <param name="vector2">Vector2 to convert</param>
    /// <returns>Resultant Vector3 (x and y of original, z = 0)</returns>
    public static Vector3 Vector2To3(Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0.0f);
    }

    /// <summary>
    /// Returns the direction that the float is facing (-1 or 1) or 0 if not facing anywhere
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <returns>Direction that the value is facing (-1 or 1)</returns>
    public static float GetDirection(float value)
    {
        // Nested ternary operation. If value is equal to zero, return 0, otherwise check if positive or negative
        return (value == 0.0f) ? 0.0f : ((value > 0.0f) ? 1.0f : -1.0f);
    }

    /// <summary>
    /// Returns whether the given value is almost equal to zero
    /// </summary>
    /// <param name="value">Value to check</param>
    /// <param name="order">Zero approximation (10^-x) to check against (default 3 or 0.001)</param>
    /// <returns>Whether the value is close enough to zero to be considered equal to zero.</returns>
    public static bool AlmostZero(float value, int order = 3)
    {
        //Debug.Log($"Comparing [{Mathf.Abs(value)}] to [{Mathf.Pow(10, -order)}]. Result: [{Mathf.Abs(value) < Mathf.Pow(10, -order)}]");
        return Mathf.Abs(value) < Mathf.Pow(10, -order);
    }

    /// <summary>
    /// Returns whether all elements of the given vector are almost equal to zero
    /// </summary>
    /// <param name="value">Vector2 to check</param>
    /// <param name="order">Zero approximation (10^-x) to check against (default 3 or 0.001)</param>
    /// <returns>Whether all values are close enough to zero to be considered equal to zero.</returns>
    public static bool AlmostZero(Vector2 value, int order = 3)
    {
        return AlmostZero(value.x, order) && AlmostZero(value.y, order);
    }
    /// <summary>
    /// Returns whether all elements of the given vector are almost equal to zero
    /// </summary>
    /// <param name="value">Vector3 to check</param>
    /// <param name="order">Zero approximation (10^-x) to check against (default 3 or 0.001)</param>
    /// <returns>Whether all values are close enough to zero to be considered equal to zero.</returns>
    public static bool AlmostZero(Vector3 value, int order = 3)
    {
        return AlmostZero(value.x, order) && AlmostZero(value.y, order) && AlmostZero(value.z, order);
    }

    /// <summary>
    /// Clamps the vector to the given min and max value for both x and y
    /// </summary>
    /// <param name="vector">Vector to clamp</param>
    /// <param name="minValue">Min value for x and y</param>
    /// <param name="maxValue">Max value for x and y</param>
    /// <returns>Clamped vector</returns>
    public static Vector2 ClampToValue(Vector2 vector, float minValue, float maxValue)
    {
        return new Vector2(Mathf.Clamp(vector.x, minValue, maxValue), Mathf.Clamp(vector.y, minValue, maxValue));
    }

    /// <summary>
    /// Clamps the given vector to be within bounds
    /// </summary>
    /// <param name="pos">Vector to clamp</param>
    /// <param name="bottomBound">Bottom left bound</param>
    /// <param name="topBound">Top right bound</param>
    /// <returns>Clamped vector</returns>
    public static Vector2 ClampToBounds(Vector2 pos, Vector2 bottomBound, Vector2 topBound)
    {
        return new Vector2(Mathf.Clamp(pos.x, bottomBound.x, topBound.x), Mathf.Clamp(pos.y, bottomBound.y, topBound.y));
    }

    /// <summary>
    /// Checks if the position is within top and bottom bounds
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <param name="bottomBound">Bottom left bound</param>
    /// <param name="topBound">Top right bound</param>
    /// <returns>Is position inside the given bounds?</returns>
    public static bool IsWithinBounds(Vector2 pos, Vector2 bottomBound, Vector2 topBound)
    {
        return (pos.x >= bottomBound.x && pos.y >= bottomBound.y && pos.x <= topBound.x && pos.y <= topBound.y);
    }

    /// <summary>
    /// Checks if the position is range [0, 1] in both x and y
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <returns>Is position is in range [0, 1]?</returns>
    public static bool IsWithinBounds01(Vector2 pos)
    {
        return (pos.x >= 0.0f && pos.y >= 0.0f && pos.x <= 1.0f && pos.y <= 1.0f);
    }

    /// <summary>
    /// Checks if the position is between top and bottom bounds
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <param name="bottomBound">Bottom bound</param>
    /// <param name="topBound">Top bound</param>
    /// <returns>Is position is in range [bottomBound, topBound]?</returns>
    public static bool IsWithinBounds(float pos, float bottomBound, float topBound)
    {
        return (pos >= bottomBound && pos <= topBound);
    }

    /// <summary>
    /// Checks if the position is in range [0,1]
    /// </summary>
    /// <param name="pos">Position to check</param>
    /// <returns>Is position is in range [0,1]?</returns>
    public static bool IsWithinBounds01(float pos)
    {
        return (pos >= 0.0f && pos <= 1.0f);
    }

    /// <summary>
    /// Generates a random vector3 with all values in range [min, max]
    /// </summary>
    /// <param name="min">Minimum vector values</param>
    /// <param name="max">Maximum vector values</param>
    /// <returns>Vector3 with all values in range [min, max]</returns>
    public static Vector3 RandomInRange(Vector3 min, Vector3 max)
    {
        return new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
    }

    /// <summary>
    /// Returns a coordinate in range ([-1, 1], [-1, 1], 0)
    /// </summary>
    /// <returns>Coordinate in or on the unit circle in x and y</returns>
    public static Vector3 RandomInUnitSphere2D()
    {
        Vector2 unitCircle = UnityEngine.Random.insideUnitCircle;
        return new Vector3(unitCircle.x, unitCircle.y, 0.0f);
    }

    /// <summary>
    /// Converts a bool into an integer value (0 or 1)
    /// </summary>
    /// <param name="value">Boolean to convert</param>
    /// <returns></returns>
    public static int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }

    /// <summary>
    /// Converts an int to a bool value (Equal to zero or not equal to zero)
    /// </summary>
    /// <param name="value">Integer value to convert</param>
    /// <returns></returns>
    public static bool IntToBool(int value)
    {
        return value != 0;
    }

    /// <summary>
    /// Multiplies a boolean (0 or 1) with the given value
    /// </summary>
    /// <param name="multiplier">Boolean to multiplier</param>
    /// <param name="value">Value to multiply</param>
    /// <returns>Returns value or 0</returns>
    public static int BoolMultiply(bool multiplier, int value)
    {
        if (!multiplier)
            return 0;
        else
            return value;
    }

    /// <summary>
    /// Multiplies a boolean (0 or 1) with the given value
    /// </summary>
    /// <param name="multiplier">Boolean to multiplier</param>
    /// <param name="value">Value to multiply</param>
    /// <returns>Returns value or 0</returns>
    public static float BoolMultiply(bool multiplier, float value)
    {
        if (!multiplier)
            return 0.0f;
        else
            return value;
    }
    
    /// <summary>
    /// Clamps the value to the nearest integer, towards zero. Effectively this means that values are floored if greater than zero and ceiled if less than zero
    /// </summary>
    /// <param name="value">Value to convert to int</param>
    /// <returns>Clamped value</returns>
    public static int FloorToZero(float value)
    {
        // If the value is greater than zero, floor
        if (value >= 0)
            return Mathf.FloorToInt(value);
        // If the value is less than zero, ceil
        else
            return Mathf.CeilToInt(value);
    }

    /// <summary>
    /// Linearly interpolates between two colors
    /// </summary>
    /// <param name="a">First color</param>
    /// <param name="b">Target color</param>
    /// <param name="t">Time constant</param>
    /// <returns>Interpolated value, equals to a + (b - a) * t</returns>
    public static Color LerpColor(Color a, Color b, float t)
    {
        return Vector4.Lerp(a, b, t);
    }

    /// <summary>
    /// Casts a color to its Vector4 representation
    /// </summary>
    /// <param name="color">Color to cast</param>
    /// <returns>Vector4 of all the color information</returns>
    public static Vector4 ColorToVector4(Color color)
    {
        return new Vector4(color.r, color.g, color.b, color.a);
    }

    /// <summary>
    /// Checks whether colors are almost equal
    /// </summary>
    /// <param name="a">First color</param>
    /// <param name="b">Second color</param>
    /// <param name="order">Order of equalness (default 3)</param>
    /// <returns></returns>
    public static bool IsAlmostColor(Color a, Color b, int order = 3)
    {
        return AlmostZero(Mathf.Abs(a.r - b.r), order)
            && AlmostZero(Mathf.Abs(a.g - b.g), order)
            && AlmostZero(Mathf.Abs(a.b - b.b), order)
            && AlmostZero(Mathf.Abs(a.a - b.a), order);
    }
}
