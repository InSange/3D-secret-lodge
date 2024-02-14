using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iPoint
{
    public float x, y;

    public iPoint(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public static implicit operator Vector3(iPoint p)
    {
        return new Vector3(p.x, p.y);
    }
    public static explicit operator iPoint(Vector3 v)
    {
        return new iPoint(v.x, v.y);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(iPoint p1, iPoint p2)
    {
        return (p1.x == p2.x && p1.y == p2.y);
    }

    public static bool operator !=(iPoint p1, iPoint p2)
    {
        return (p1.x != p2.x || p1.y != p2.y);
    }

    public static iPoint operator +(iPoint p0, iPoint p1)
    {
        iPoint r = new iPoint(p0.x + p1.x, p0.y + p1.y);
        return r;
    }

    public static iPoint operator -(iPoint p0, iPoint p1)
    {
        iPoint r = new iPoint(p0.x - p1.x, p0.y - p1.y);
        return r;
    }

    public static iPoint operator *(iPoint p, float f)
    {
        iPoint r = new iPoint(p.x * f, p.y * f);
        return r;
    }

    public static iPoint operator /(iPoint p, float f)
    {
        iPoint r = new iPoint(p.x / f, p.y / f); 
        return r;
    }

    public float length()
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public void loadIdentity()
    {
        float len = length();
        if (len == 0)
            return;
        x /= len;
        y /= len;
    }
}
