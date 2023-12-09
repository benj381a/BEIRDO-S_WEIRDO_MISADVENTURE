using System.Collections;
using UnityEngine;
using System.Linq;
using System;

public static class ExtensionMethods
{
    // Vector2_
    public static Vector3 ToVector3(this Vector2 vector, float z = 0)
    {
        return new Vector3(vector.x, vector.y, z);
    }
    public static Vector2 Random(this Vector2 vector, Vector2 minInclusive, Vector2 maxExclusive)
    {
        vector = new Vector2(UnityEngine.Random.Range(minInclusive.x, maxExclusive.x), UnityEngine.Random.Range(minInclusive.y, maxExclusive.y));
        return vector;
    }

    // Vector2Int_
    public static Vector3Int ToVector3Int(this Vector2Int vector, int z = 0)
    {
        return new Vector3Int(vector.x, vector.y, z);
    }
    public static Vector2Int Random(this Vector2Int vector, Vector2Int minInclusive, Vector2Int maxExclusive)
    {
        vector = new Vector2Int(UnityEngine.Random.Range(minInclusive.x, maxExclusive.x), UnityEngine.Random.Range(minInclusive.y, maxExclusive.y));
        return vector;
    }

    // Vector3_
    public static Vector2 ToVector2(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }
    public static Vector3 Random(this Vector3 vector, Vector3 minInclusive, Vector3 maxExclusive)
    {
        vector = new Vector3(UnityEngine.Random.Range(minInclusive.x, maxExclusive.x), UnityEngine.Random.Range(minInclusive.y, maxExclusive.y), UnityEngine.Random.Range(minInclusive.z, maxExclusive.z));
        return vector;
    }

    // Vector3Int_
    public static Vector2Int ToVector2Int(this Vector3Int vector)
    {
        return new Vector2Int(vector.x, vector.y);
    }
    public static Vector3Int Random(this Vector3Int vector, Vector3Int minInclusive, Vector3Int maxExclusive)
    {
        vector = new Vector3Int(UnityEngine.Random.Range(minInclusive.x, maxExclusive.x), UnityEngine.Random.Range(minInclusive.y, maxExclusive.y), UnityEngine.Random.Range(minInclusive.z, maxExclusive.z));
        return vector;
    }
}