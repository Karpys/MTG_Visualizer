
using UnityEngine;

namespace Script.Helper
{
    public static class Vector3Extensions
    {
        public static Vector3 Y(this Vector3 vec,float y)
        {
            return new Vector3(vec.x, y, vec.z);
        }
    }
}