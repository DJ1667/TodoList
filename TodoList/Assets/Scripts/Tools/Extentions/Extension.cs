using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    #region 列表相关操作

    /// <summary>
    /// 随机取出列表中一个元素
    /// </summary>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Rand<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    #endregion

    #region Transform And GameObject

    /// <summary>
    /// 删除所有子物体  （不包括自己）
    /// </summary>
    /// <param name="t"></param>
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t)
        {
            Object.Destroy(child.gameObject);
        }
    }

    public static void SetActiveEx(this Transform trans, bool active)
    {
        if (trans.gameObject.activeSelf != active)
        {
            trans.gameObject.SetActive(active);
        }
    }

    public static void SetActiveEx(this GameObject go, bool active)
    {
        if (go == null)
            return;

        if (go.activeSelf != active)
        {
            go.SetActive(active);
        }
    }

    /// <summary>
    /// 更改所有子物体的Layer （包括自己）
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="layer"></param>
    public static void SetLayersRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform transform in gameObject.transform)
        {
            transform.gameObject.SetLayersRecursively(layer);
        }
    }

    #endregion

    #region Vector2 And Vector3

    /// <summary>
    /// 三维转二维
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector2 ToV2(this Vector3 input) => new Vector2(input.x, input.y);

    /// <summary>
    /// y方向归零
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector3 Flat(this Vector3 input) => new Vector3(input.x, 0, input.z);

    /// <summary>
    /// 转成int类型数值
    /// </summary>
    /// <param name="vec3"></param>
    /// <returns></returns>
    public static Vector3Int ToveVector3Int(this Vector3 vec3) =>
        new Vector3Int((int) vec3.x, (int) vec3.y, (int) vec3.z);

    /// <summary>
    /// 将一个Vector2旋转degrees度   返回旋转后的结果
    /// </summary>
    /// <param name="v"></param>
    /// <param name="degrees"></param>
    /// <returns></returns>
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    #endregion
}