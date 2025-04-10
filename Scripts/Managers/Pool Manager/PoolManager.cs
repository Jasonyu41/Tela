using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // [SerializeField] Pool[] playerBulletPools;
    [SerializeField] Pool[] playerVFXPools;
    //[SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] enemyAttackRangePools;
    [SerializeField] Pool[] enemyAttackVFXPools;
    [SerializeField] Pool[] buffPools;

    static Dictionary<GameObject, Pool> dictionary;

    private void Awake()
    {
        dictionary = new Dictionary<GameObject, Pool>();

        // Initialize(playerBulletPools);
        Initialize(playerVFXPools);
        //Initialize(enemyPools);
        Initialize(enemyAttackRangePools);
        Initialize(enemyAttackVFXPools);
        Initialize(buffPools);
    }

    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
#if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("Pool Manager 中含有 相同(名字)的預處理物件 名字為:" + pool.Prefab.name);
                continue;
            }
#endif
            dictionary.Add(pool.Prefab, pool);

            Transform poolParent = new GameObject("Pool: " + pool.Prefab.name).transform;

            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Transform parent)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(parent);
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Transform parent)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, parent);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
#if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 中含有 空的預處理物件 或是 未將該物件初始化 未初始化物件為: " + prefab.name);
            return null;
        }
#endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
