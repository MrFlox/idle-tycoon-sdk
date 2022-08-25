using System;
using System.Reflection.Emit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class ResourceZone : MonoBehaviour
{
    public int resourceValue;
    public int maxCapacity;
    public ResourceScriptableObject resource;

    public delegate void OnLoadResources();
    public event OnLoadResources onLoadResources;
    public delegate void OnCollectResources();
    public event OnCollectResources onCollectResources;
    /// <summary>
    /// Собрали полное кол-во ресурсов
    /// </summary>
    public UnityEvent onMaxCapacity;
    //-----------------------------------------------
    GUIStyle style;
    //-----------------------------------------------

    void Awake()
    {
        addResourceVisuals();
    }

    private void addResourceVisuals()
    {
        GameObject obj = Instantiate(resource.visualPrefab);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;

    }


    private void initStyle()
    {
        style = new GUIStyle();
        // style.fontStyle = FontStyle.Bold;
        // style.normal.textColor = Color.red;
        style.alignment = TextAnchor.MiddleCenter;
    }
    void OnDrawGizmos()
    {
        if (style == null) initStyle();
        Handles.Label(transform.position - Vector3.down * .2f, $"{resourceValue}/{maxCapacity}", style);
        if (resource != null)
            Handles.Label(transform.position + Vector3.down * .2f, $"{resource.resourceName}", style);
    }

    public int collect(int value = 1)
    {
        int collectValue;
        if (resourceValue == 0) throw new Exception("Нет ресурсов!");
        if (value > resourceValue) collectValue = resourceValue;
        else collectValue = value;
        resourceValue -= collectValue;
        onCollectResources?.Invoke();
        if (resourceValue < 0) throw new Exception("Отрицательное кол-во ресурсов!");
        return collectValue;
    }
    public bool readyToCollect() => resourceValue > 0;

    int getAvailableCopactiy() => maxCapacity - resourceValue;

    public int addResource(int value = 1)
    {
        int addValue;
        if (value + resourceValue > maxCapacity) addValue = getAvailableCopactiy();
        else addValue = value;
        resourceValue += addValue;
        if (resourceValue > maxCapacity) throw new Exception("Ресурсов больше чем макс. кол-во!");
        onLoadResources?.Invoke();

        if (resourceValue == maxCapacity) onMaxCapacity?.Invoke();

        return addValue;
    }

    public bool isStorageSpace() => resourceValue < maxCapacity;

    internal int freeSpace() => getAvailableCopactiy();
}



