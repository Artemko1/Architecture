using UnityEngine;

namespace CodeBase.Data
{
    public static class DataExtensions
    {
        public static Vector3Data AsVector3Data(this Vector3 vector3)
        {
            return new Vector3Data(vector3.x, vector3.y, vector3.z);
        }

        public static Vector3 AsUnityVector3(this Vector3Data vector3Data)
        {
            return new Vector3(vector3Data.X, vector3Data.Y, vector3Data.Z);
        }

        public static T ToDeserialized<T>(this string json) => JsonUtility.FromJson<T>(json);
    }
}