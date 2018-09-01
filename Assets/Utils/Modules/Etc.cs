using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utils
{
    /// <summary>
    /// Returns a direction from a given angle
    /// </summary>
    public static Vector2 DirFromAngle(float angle, bool isRadians = false)
    {
        //It's already in radians, so go ahead and return it.
        if (isRadians) return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        //It's not radians, so adjust the angle.
        angle -= 90;    //Making the top be the origin
        angle = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    /// <summary>
    /// Returns a point on the edge of a circle
    /// </summary>
    public static Vector2 GetPointInCircle(Vector2 center, Vector2 direction, float dist)
    {
        return center + direction * dist;
    }

    //Sets the vertices of a LineRenderer
    public static void SetVertices(this LineRenderer l, Vector2[] verts)
    {
        l.positionCount = verts.Length;
        l.SetPositions(verts.ToVec3Array());
    }
}