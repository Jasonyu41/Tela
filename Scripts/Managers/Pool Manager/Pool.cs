using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Pool
{
    public GameObject Prefab => prefab;

    [SerializeField] GameObject prefab;
    [SerializeField] int size = 1;

    Queue<GameObject> queue;

    Transform parent;

    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;

        for(int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }

    GameObject Copy()
    {
        GameObject copy = GameObject.Instantiate(prefab, parent);

        copy.SetActive(false);

        return copy;
    }

    GameObject AvailableObject()
    {
        GameObject availableObject = null;

        if(queue.Count > 0 && !queue.Peek().activeSelf)
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;
    }

    public GameObject PreparedObject()
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);

        return preparedObject;
    }

    public GameObject PreparedObject(Transform parent)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.parent = parent;
        preparedObject.transform.localPosition = Vector3.zero;
        preparedObject.transform.localRotation = Quaternion.identity;
        preparedObject.transform.localScale = Vector3.one;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Transform parent)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.parent = parent;
        preparedObject.transform.position = position;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;

        return preparedObject;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        GameObject preparedObject = AvailableObject();

        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localScale;

        return preparedObject;
    }
}
