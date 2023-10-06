//using UnityEngine;
//using Unity.Mathematics;
//using Unity.Collections;
//using Unity.Entities;
//using System.Collections.Generic;

//public static class PrimitivesExtensions
//{

//    public static float Magnitude(this float2 vector)
//    {
//        return math.sqrt(math.pow(vector.x, 2) + math.pow(vector.y, 2));
//    }
//    public static float SqrMagnitude(this float2 vector)
//    {
//        return math.pow(vector.x, 2) + math.pow(vector.y, 2);
//    }
//    //public static float Magnitude(this int2 vector)
//    //{
//    //    return math.sqrt(math.pow(vector.x, 2) + math.pow(vector.y, 2));
//    //}
//    //public static float SqrMagnitude(this int2 vector)
//    //{
//    //    return math.pow(vector.x, 2) + math.pow(vector.y, 2);
//    //}
//    public static Vector3 ToVector3(this Vector2 vector)
//    {
//        return new Vector3(vector.x, vector.y, 0);
//    }
//    public static Vector3 ToVector3(this float2 vector)
//    {
//        return new Vector3(vector.x, vector.y, 0);
//    }
//    //public static float2 ToFloat2(this Vector3 vector)
//    //{
//    //    return new float2(vector.x, vector.y);
//    //}
//    //public static float2 ToFloat2(this Vector2 vector)
//    //{
//    //    return new float2(vector.x, vector.y);
//    //}
//    public static int2 ToMapPosition(this int index)
//    {
//        return index.ToMapPosition(64, 64);
//    }
//    public static int2 ToMapPosition(this int index,int width, int height)
//    {
//        int x = Mathf.Max(index % width, 0);
//        int y = (index - x) / height;
//        return new int2(x, y);
//    }

//    public static int2 ToInt2(this Vector2 vector)
//    {
//        return new int2((int)vector.x, (int)vector.y);
//    }
//    public static int2 ToInt2(this Vector3 vector)
//    {
//        return new int2((int)vector.x, (int)vector.y);
//    }
//    public static Vector3 ToRealPosition(this float2 vector)
//    {
//        return new Vector3(vector.x, vector.y, vector.y * 10);
//    }

//    public static Vector3 ToRealPosition(this int2 vector)
//    {
//        return new Vector3(vector.x, vector.y, vector.y * 10);
//    }
//    public static int ToMapIndex(this int2 v, int width, int height)
//    {

//        if (v.x >= 0 && v.x < width && v.y >= 0 && v.y < height)
//        {

//            return v.y * width + v.x;
//        }
//        else
//        {

//            return -1;
//        }
//    }
//    public static int ToMapIndex(this float2 vector)
//    {
//        int2 v = (int2)vector;
//        return ToMapIndex(v);
//    }
//    public static int ToMapIndex(this int2 v)
//    {
//        return ToMapIndex(v, 64, 64);
//    }

//    public static bool Contains<T>(this DynamicBuffer<T> buffer, T item) where T : unmanaged
//    {
//        for (int i = 0; i < buffer.Length; i++)
//        {
//            if(buffer[i].Equals(item))
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//    //public static bool Contains<T>(this NativeArray<T> array, T item) where T : struct
//    //{
//    //    foreach (var element in array)
//    //    {

//    //        if(element == item)
//    //        {
//    //            return true;
//    //        }
//    //    }
//    //    return false;
//    //}

//    public static void Randomize(this float2 center, float range)
//    {
//        center.x += UnityEngine.Random.Range(-range, range);
//        center.y += UnityEngine.Random.Range(-range, range);
//    }
//    public static float GetDistance(this int2 start, int2 target)
//    {
//        return (target - start).Magnitude();
//    }
//    public static float GetSqrDistance(this int2 start, int2 target)
//    {
//        return (target - start).SqrMagnitude();
//    }

//    public static void Clear(this Texture2D texture)
//    {
//        for (int x = 0; x < texture.width; x++)
//        {
//            for (int y = 0; y < texture.height; y++)
//            {
//                texture.SetPixel(x, y, UnityEngine.Color.clear);
//            }
//        }
//        texture.Apply();
//    }
//    public static void SetAlpha(this Texture2D texture, float a)
//    {
//        for (int x = 0; x < texture.width; x++)
//        {
//            for (int y = 0; y < texture.height; y++)
//            {
//                UnityEngine.Color color = texture.GetPixel(x, y);
//                texture.SetPixel(x, y, new UnityEngine.Color(color.r, color.g, color.b,a));
//            }
//        }
//        texture.Apply();
//    }

//    public static T[] ToOneDimension<T>(this T[,] array2d)
//    {
//        var array1d = new T[array2d.GetLength(0) * array2d.GetLength(1)];
//        for (int x = 0; x < array2d.GetLength(0); x++)
//        {
//            for (int y = 0; y < array2d.GetLength(1); y++)
//            {
//                array1d[ y * array2d.GetLength(0) + x] = array2d[x,y];
//            }
//        }
//        return array1d;
//    }
//    public static List<T> GetBorder<T>(this T[,] map)
//    {
//        var list = new List<T>();
//        for (int x = 0; x < map.GetLength(0); x++)
//        {
//            for (int y = 0; y < map.GetLength(1); y++)
//            {
//                if (x == 0 || y == 0 || x == map.GetLength(0) - 1 || y == map.GetLength(1) - 1)
//                {
//                    list.Add(map[x, y]);
//                } 

//            }

//        }
//        return list;
//    }
//}
